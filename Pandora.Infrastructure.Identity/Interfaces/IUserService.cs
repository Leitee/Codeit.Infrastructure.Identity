/// <summary>
/// 
/// </summary>
namespace Pandora.Infrastructure.Identity.Interfaces
{
    using Pandora.Infrastructure.Identity.Model.Dtos;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<List<UserDTO>> GetAll(bool includeDeleted = false);
        Task<UserDTO> GetById(string id, bool includeDeleted = false);
        Task<UserDTO> GetByLogin(string login, bool includeDeleted = false);
        Task<bool> Delete(int id);
        Task<bool> Update(UserDTO dto);
        Task<byte[]> GetUserPhoto(string userId);
    }
}