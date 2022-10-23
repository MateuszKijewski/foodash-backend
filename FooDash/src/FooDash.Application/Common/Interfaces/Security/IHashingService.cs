namespace FooDash.Application.Common.Interfaces.Security
{
    public interface IHashingService
    {
        byte[] GetSalt();

        string Hash(string input, byte[] salt);
    }
}