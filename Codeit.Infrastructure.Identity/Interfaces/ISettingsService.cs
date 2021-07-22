/// <summary>
/// 
/// </summary>
namespace Codeit.Infrastructure.Identity.Interfaces
{
    using Codeit.Infrastructure.Identity.Model.Dtos;
    using System.Threading.Tasks;

    public interface ISettingsService
    {
        Task<SettingsDTO> GetById(int id);

        Task<bool> Update(SettingsDTO settings);
    }
}