namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using WebApi.Models;

    public class UsuariosController : ApiController
    {
        private BDD_HRVEntities db = new BDD_HRVEntities();

        // GET: api/Usuarios

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
        public IHttpActionResult PutUsuarios(string id, Usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarios.login_usuario)
            {
                return BadRequest();
            }

            db.Entry(usuarios).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Usuarios
        [ResponseType(typeof(Usuarios))]
        public IHttpActionResult PostUsuarios(Usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuarios.Add(usuarios);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UsuariosExists(usuarios.login_usuario))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = usuarios.login_usuario }, usuarios);
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuarios))]
        public IHttpActionResult DeleteUsuarios(string id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuarios);
            db.SaveChanges();

            return Ok(usuarios);
        }

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
    }
}