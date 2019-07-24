namespace GeoStoreAPI.Services
{
    public interface IUserRepository
    {
        bool ValidateCredentials(string username, string password);

        AppUser FindBySubjectId(string subjectId);

        AppUser FindByUsername(string username);
    }
}
