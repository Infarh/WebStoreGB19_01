using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebStore.DAL.Context;

namespace WebStore.ServiceHosting.Controllers
{
    [Produces("application/json")]
    [Route("api/roles")]
    public class RolesController : Controller
    {
        private readonly RoleStore<IdentityRole> _RoleStore;

        public RolesController(WebStoreContext context)
        {
            _RoleStore = new RoleStore<IdentityRole>(context);
        }

        [HttpPost]
        public async Task<bool> CreateAsync(IdentityRole role)
        {
            var result = await _RoleStore.CreateAsync(role);
            return result.Succeeded;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync(IdentityRole role)
        {
            var result = await _RoleStore.UpdateAsync(role);
            return result.Succeeded;
        }

        [HttpPost("delete")]
        public async Task<bool> DeleteAsync(IdentityRole role)
        {
            var result = await _RoleStore.DeleteAsync(role);
            return result.Succeeded;
        }

        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync(IdentityRole role)
        {
            var result = await _RoleStore.GetRoleIdAsync(role);
            return result;
        }

        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync(IdentityRole role)
        {
            var result = await _RoleStore.GetRoleNameAsync(role);
            return result;
        }
        
        [HttpPost("SetRoleName/{roleName}")]
        public Task SetRoleNameAsync(IdentityRole role, string roleName)
        {
            return _RoleStore.SetRoleNameAsync(role, roleName);
        }

        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(IdentityRole role)
        {
            var result = await _RoleStore.GetRoleNameAsync(role);
            return result;
        }

        [HttpPost("SetNormalizedRoleName/{normalizedName}")]
        public Task SetNormalizedRoleNameAsync(IdentityRole role, string
            normalizedName)
        {
            return _RoleStore.SetNormalizedRoleNameAsync(role, normalizedName);
        }

        [HttpGet("FindById/{roleId}")]
        public async Task<IdentityRole> FindByIdAsync(string roleId)
        {
            var result = await _RoleStore.FindByIdAsync(roleId);
            return result;
        }

        [HttpGet("FindByName/{normalizedRoleName}")]
        public async Task<IdentityRole> FindByNameAsync(string
            normalizedRoleName)
        {
            var result = await _RoleStore.FindByNameAsync(normalizedRoleName);
            return result;
        }
    }
}