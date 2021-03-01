
namespace WebApi.Models
{
    public class Usuarios
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string Password { get; set; }
        public string ImagenPath { get; set; }
    }
}