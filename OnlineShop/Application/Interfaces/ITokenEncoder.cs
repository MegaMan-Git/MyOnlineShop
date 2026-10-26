namespace Application.Interfaces
{
    public interface ITokenEncoder
    {
        string Encode(string token);
        string Decode(string token);
    }
}
