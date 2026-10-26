namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(string email, string userId, string role);
    }
}
