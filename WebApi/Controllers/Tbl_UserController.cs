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
    

    public class Tbl_UserController : ApiController
    {
        private Base_TitulacionEntities _dtb = new Base_TitulacionEntities();

        // GET: api/Tbl_User
        //public IQueryable<Tbl_User> GetTbl_User()
        //{
        //    return db.Tbl_User;
        //}
        [HttpGet]
        [Route("api/login/{username}/{password}")]
        public HttpResponseMessage GetLogin(string username, string password)
        {
            try
            {
                var user = _dtb.Tbl_User.Where(u => u.User_Login == username && u.User_Password == password &&
                u.Tbl_Perfil.Perfil_Name == "Operarios").FirstOrDefault();

                if (user == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.OK, "Login Incorrecto..!");
                }
                else
                {
                    Usuarios _usuario = new Usuarios()
                    {
                        UserId = user.UserId,
                        UserName = user.User_Name,
                        UserLastName = user.User_LastName,
                        Password = user.User_Password,
                        ImagenPath = user.User_ImagenPath
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, _usuario);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.ToString());
            }
        }

        // GET: api/Tbl_User/5
        //[ResponseType(typeof(Tbl_User))]
        //public IHttpActionResult GetTbl_User(string id)
        //{
        //    Tbl_User tbl_User = _dtb.Tbl_User.Find(id);
        //    if (tbl_User == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(tbl_User);
        //}

        //// PUT: api/Tbl_User/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutTbl_User(string id, Tbl_User tbl_User)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tbl_User.User_Login)
        //    {
        //        return BadRequest();
        //    }

        //    _dtb.Entry(tbl_User).State = EntityState.Modified;

        //    try
        //    {
        //        _dtb.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!Tbl_UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Tbl_User
        //[ResponseType(typeof(Tbl_User))]
        //public IHttpActionResult PostTbl_User(Tbl_User tbl_User)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _dtb.Tbl_User.Add(tbl_User);

        //    try
        //    {
        //        _dtb.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (Tbl_UserExists(tbl_User.User_Login))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = tbl_User.User_Login }, tbl_User);
        //}

        //// DELETE: api/Tbl_User/5
        //[ResponseType(typeof(Tbl_User))]
        //public IHttpActionResult DeleteTbl_User(string id)
        //{
        //    Tbl_User tbl_User = _dtb.Tbl_User.Find(id);
        //    if (tbl_User == null)
        //    {
        //        return NotFound();
        //    }

        //    _dtb.Tbl_User.Remove(tbl_User);
        //    _dtb.SaveChanges();

        //    return Ok(tbl_User);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dtb.Dispose();
            }
            base.Dispose(disposing);
        }

        //private bool Tbl_UserExists(string id)
        //{
        //    return _dtb.Tbl_User.Count(e => e.User_Login == id) > 0;
        //}
    }
}