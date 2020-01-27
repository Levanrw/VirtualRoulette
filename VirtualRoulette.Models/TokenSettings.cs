namespace VirtualRoulette.Models
{
    /// <summary>
    /// Object for generated token
    /// </summary>
    public class TokenSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessExpiration { get; set; }
    }
}
