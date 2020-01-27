namespace VirtualRoulette.Service.SignManager
{
    public interface ISignInManager
    {
        string GetPassword(string flatPassword, string salt);
        string GenerateToken(string UserName, string UserId);
    }
}
