﻿namespace WebApi.Controllers
{
    using Helpers;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using WebApi.Models;

    public class UsuariosController : ApiController
    {
        #region Variables
        private BDD_HRVEntities db = new BDD_HRVEntities(); 
        #endregion

        #region Metodos
        [HttpGet]
        [Route("api/login/{username}/{password}")]
        public HttpResponseMessage GetLogin(string username, string password)
        {
            try
            {
                var user = db.Usuarios.Where(u => u.login_usuario == username && u.password_usuario == password &&
                u.Perfiles.nombre_perfil == "Tecnico").FirstOrDefault();

                if (user == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.OK, "Login Incorrecto..!");
                }
                else
                {
                    Usuario _usuario = new Usuario()
                    {
                        UserId = user.id_usuario,
                        UserName = user.nombre_usuario,
                        UserLastName = user.apellido_usuario,
                        Password = user.password_usuario,
                        ImagenPath = user.imagen_usuario
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, _usuario);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.ToString());
            }
        }
        public IQueryable<Usuarios> GetUsuarios()
        {
            return db.Usuarios;
        }

        // GET: api/Usuarios/5
        [ResponseType(typeof(Usuarios))]
        public IHttpActionResult GetUsuarios(string id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return Ok(usuarios);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuarios(string id, UserApi user)
        {
            Usuarios _original = db.Usuarios.Where(u => u.id_usuario == user.UserId).FirstOrDefault();
            db.Usuarios.Attach(_original);
            _original.nombre_usuario = user.UserName.ToUpper();
            _original.apellido_usuario = user.UserLastName.ToUpper();

            if (user.ImagenUser != null && user.ImagenUser.Length > 0)
            {
                var stream = new MemoryStream(user.ImagenUser);
                var guid = Guid.NewGuid().ToString();
                var file = string.Format("{0}.jpg", guid);
                var folder = @"~/Content/PhotoUser/";
                var fullPath = string.Format("{0}/{1}", folder, file);
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                //var totlapath = @"http://www.miwebapi.com/Content/PhotoUser/" + file;
                var totlapath = @"http://localhost:44323/Content/PhotoUser/" + file;
                //var totlapath = @"http://localhost/WS_WorkOrders/Content/PhotoUser/" + file;

                if (response)
                {
                    _original.imagen_usuario = totlapath;
                }
            }

            db.Entry(_original).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.OK);

        }

        [HttpPost]
        [Route("api/GetUsuario")]
        public async Task<IHttpActionResult> GetUser(JObject usermodel)
        {
            int userid = 0;
            dynamic jsonObject = usermodel;
            //dynamic data = JObject.Parse(usermodel);
            try
            {
                userid = usermodel["UserId"].Value<int>();
            }
            catch (Exception ex)
            {
                return BadRequest("Missing parameter..!");
                throw;
            }
            var user = await db.Usuarios.AsNoTracking().Where(u => u.id_usuario == userid).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            Usuario _usuario = new Usuario();
            {
                _usuario.UserId = user.id_usuario;
                _usuario.UserName = user.nombre_usuario;
                _usuario.UserLastName = user.apellido_usuario;
                _usuario.Password = user.password_usuario;
                _usuario.ImagenPath = user.imagen_usuario;
            }

            return Ok(_usuario);
        }

        [HttpPost]
        [Route("api/CambiarPassWord")]
        public async Task<IHttpActionResult> ChangePassword(JObject passwordmodel)
        {
            int userid = 0;
            var nuevopassword = string.Empty;
            dynamic jsonObject = passwordmodel;

            try
            {
                userid = passwordmodel["UserId"].Value<int>();
                nuevopassword = jsonObject.NuevoPassword.Value;
            }
            catch
            {
                return BadRequest("Jason Icorrecto..!");
            }

            var user = await db.Usuarios.AsNoTracking().Where(u => u.id_usuario == userid).FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("No Existen Datos");
            }

            db.Usuarios.Attach(user);
            user.password_usuario = nuevopassword;
            await db.SaveChangesAsync();

            return Ok("OK");
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

        private bool UsuariosExists(string id)
        {
            return db.Usuarios.Count(e => e.login_usuario == id) > 0;
        } 
        #endregion
    }
}