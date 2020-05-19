namespace WebApi.Models.JWT
{
    public class Token
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Origin { get; set; }

        public string Secret { get; set; }
    }
}