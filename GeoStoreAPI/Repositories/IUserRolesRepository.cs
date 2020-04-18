using GeoStoreAPI.Models;

namespace GeoStoreAPI.Repositories
{
    public interface IUserRolesRepository
    {
        void CreateUserRoles(AppUserRoles userRoles);
        AppUserRoles GetUserRoles(string userID);
        void RemoveUserRoles(string userID);
        void UpdateUserRoles(string id, AppUserRoles userRoles);
    }
}