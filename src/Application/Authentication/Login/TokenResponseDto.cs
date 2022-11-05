namespace Authentication.Login
{
    public class TokenResponseDto
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; } = "Bearer";
        public double expires_in { get; set; } = 60 * 60;
    }
}
