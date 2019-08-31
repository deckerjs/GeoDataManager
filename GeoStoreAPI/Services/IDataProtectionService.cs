namespace GeoStoreAPI.Services
{
    public interface IDataProtectionService
    {
        string GetPasswordHash(string passwordClear);
        bool PasswordMatchesHash(string passwordClear, string hashed);
    }
}