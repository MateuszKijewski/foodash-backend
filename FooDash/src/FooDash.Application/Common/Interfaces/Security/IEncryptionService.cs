namespace FooDash.Application.Common.Interfaces.Security
{
    public interface IEncryptionService
    {
        string Encrypt(string input);

        string Decrypt(string encryptedData);
    }
}