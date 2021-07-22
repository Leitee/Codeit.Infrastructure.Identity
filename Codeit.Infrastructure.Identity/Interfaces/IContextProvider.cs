/// <summary>
/// 
/// </summary>
namespace Codeit.Infrastructure.Identity.Interfaces
{
    using Codeit.Infrastructure.Identity.Model;

    public interface IContextProvider
    {
        ContextSession GetCurrentContext();
    }
}
