/// <summary>
/// 
/// </summary>
namespace Pandora.Infrastructure.Identity.Interfaces
{
    using Pandora.Infrastructure.Identity.Model.Dtos;
    using System.Threading.Tasks;

    public interface ISettingsService
    {
        Task<SettingsDTO> GetById(int id);

        Task<bool> Update(SettingsDTO settings);
    }
}