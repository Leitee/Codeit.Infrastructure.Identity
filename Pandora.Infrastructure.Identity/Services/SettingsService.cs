/// <summary>
/// 
/// </summary>
namespace Pandora.Infrastructure.Identity.Services
{
    using Pandora.Infrastructure.Identity.DAL.Context;
    using Pandora.Infrastructure.Identity.Interfaces;
    using Pandora.Infrastructure.Identity.Model.Dtos;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;

    public class SettingsService : BaseService, ISettingsService
    {
        public SettingsService(IContextProvider contextProvider, ILoggerFactory loggerFactory, IdentityDBContext context) 
            : base(contextProvider, loggerFactory, context)
        {

        }

        public async Task<bool> Update(SettingsDTO dto)
        {
            var settings = _context.Settings.AsTracking().FirstOrDefault(x => x.Id == dto.Id);
            settings?.Update(dto.ThemeName);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<SettingsDTO> GetById(int id)
        {
            var settings = await _context.Settings.AsTracking().FirstOrDefaultAsync(x => x.Id == id);
            return settings.ConvertToDto();
        }
    }
}
