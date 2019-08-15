
using GeoStoreAPI.Models;

namespace GeoStoreAPI.Repositories
{
    public interface IRoleRepository
    {
        void CreateRole(AppRole role);
        AppRole GetRole(string roleId);
        void UpdateRole(AppRole role);
        void RemoveRole(string roleID);
    }

}