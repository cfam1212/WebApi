namespace WebApi.Models
{
    public class UserApi
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string Password { get; set; }
        public byte[] ImagenUser { get; set; }
    }
}