namespace WebApi.Controllers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using WebApi.Models;
    using Helpers;
    using System.Data.Entity;
    using System.Threading.Tasks;

    public class OrdenesTrabajoesController : ApiController
    {
        #region Variables
        private BDD_HRVEntities db = new BDD_HRVEntities();
        string _carpeta, _nombrearchivo, _rutaimagen; 
        #endregion

        #region Metodos
        [HttpGet]
        [Route("api/ordenes/{tecnico}")]
        public HttpResponseMessage GetOrdenes(int tecnico)
        {
            try
            {
                var result = db.FunGetOrdenesPorTecnico(tecnico, string.Empty, 0);

                db = new BDD_HRVEntities();
                db.FunConsultaDatos(2, tecnico, string.Empty, string.Empty);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                var message = string.Format(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
        }

        [Route("api/parametro/{nameparametro}")]
        public HttpResponseMessage GetParametros(string nameparametro)
        {
            try
            {
                var result = db.FunGetParametrosGene(0, nameparametro, string.Empty, string.Empty, 0, 0);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                var message = string.Format(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
        }


        [Route("api/listatrabajo")]
        public HttpResponseMessage GetListaTrabajo()
        {
            try
            {
                var result = db.FunGetListaTrabajo(0, string.Empty, string.Empty, 0, 0).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                var message = string.Format(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
        }

        [HttpPost]
        [Route("api/ordencabedeta")]
        public async Task<IHttpActionResult> SaveOrderWorks(Orden ordencabedeta)
        {
            try
            {
                OrdenesTrabajo _originalcabecera = db.OrdenesTrabajo.Where(o => o.id_orden == ordencabedeta.IdOrden).FirstOrDefault();
                db.OrdenesTrabajo.Attach(_originalcabecera);
                _originalcabecera.id_orden = ordencabedeta.IdOrden;
                _originalcabecera.orden_estado = "FIN";
                _originalcabecera.orden_fechainiciotr = DateTime.ParseExact(ordencabedeta.FechaInicioTR,
                    "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _originalcabecera.orden_fechafintr = DateTime.ParseExact(ordencabedeta.FechaFinalTR,
                    "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _originalcabecera.orden_observaciontr = ordencabedeta.Observacion.ToUpper();
                _originalcabecera.orden_longitud = ordencabedeta.Logintud;
                _originalcabecera.orden_latitud = ordencabedeta.Latitud;
                _originalcabecera.orden_auxvar = string.Empty;
                _originalcabecera.orden_auxint = 0;

                if (ordencabedeta.ImagenTR != null && ordencabedeta.ImagenTR.Length > 0)
                {
                    _carpeta = @"~/Content/Equipos/";

                    if (!Directory.Exists(_carpeta))
                    {
                        Directory.CreateDirectory(_carpeta);
                    }

                    _nombrearchivo = string.Format("{0}.jpg", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                    var _stream = new MemoryStream(ordencabedeta.ImagenTR);
                    var _response = FilesHelper.UploadPhoto(_stream, _carpeta, _nombrearchivo);

                    _rutaimagen = @"http://localhost:44323/Content/Equipos/" + _nombrearchivo;

                    if (_response)
                    {
                        _originalcabecera.orden_rutaimagen = _rutaimagen;
                    }
                }

                Equipos _orignalequipos = db.Equipos.Where(e => e.id_equipo == ordencabedeta.IdEquipo).FirstOrDefault();

                db.Equipos.Attach(_orignalequipos);
                _orignalequipos.marca_quipo = ordencabedeta.MarcaId;
                _orignalequipos.modelo_equipo = ordencabedeta.ModeloId;
                _orignalequipos.voltaje_equipo = ordencabedeta.Voltaje;
                _orignalequipos.amperaje_equipo = ordencabedeta.Amperaje;
                _orignalequipos.presion_equipo = ordencabedeta.Presion;

                foreach (OrdenesTrabajoDetalle _detalletrabajo in ordencabedeta.OrdenDetalles)
                {
                    _detalletrabajo.auxi_ordendetalle = 0;
                    _detalletrabajo.auxv_ordendetalle = string.Empty;
                    db.Entry(_detalletrabajo).State = EntityState.Added;
                }

                await db.SaveChangesAsync();

                return Ok("OK");
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
                throw;
            }
        }
        #endregion

        #region Disponsed
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrdenesTrabajoExists(int id)
        {
            return db.OrdenesTrabajo.Count(e => e.id_orden == id) > 0;
        } 
        #endregion
    }
}