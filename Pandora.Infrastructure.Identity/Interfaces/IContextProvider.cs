/// <summary>
/// 
/// </summary>
namespace Pandora.Infrastructure.Identity.Interfaces
{
    using Pandora.Infrastructure.Identity.Model;

    public interface IContextProvider
    {
        ContextSession GetCurrentContext();
    }
}
