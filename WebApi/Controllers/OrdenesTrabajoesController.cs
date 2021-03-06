namespace WebApi.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using WebApi.Models;
    public class OrdenesTrabajoesController : ApiController
    {
        private BDD_HRVEntities db = new BDD_HRVEntities();

        [HttpGet]
        [Route("api/ordenes/{tecnico}")]
        public HttpResponseMessage GetOrdenes(int tecnico)
        {
            db.Configuration.LazyLoadingEnabled = false;

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
            //db.Configuration.LazyLoadingEnabled = false;
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

        [HttpGet]
        [Route("api/listatrabajo")]        
        public HttpResponseMessage GetListaTrabajo()
        {
            //db.Configuration.LazyLoadingEnabled = false;
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
    }
}