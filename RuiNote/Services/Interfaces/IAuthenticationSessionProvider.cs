using System.Threading.Tasks;
using RuiNote.Services.Model;

namespace RuiNote.Services.Interfaces
{
    /// <summary>
    /// The AuthenticationSessionProvider interface.
    /// </summary>
    /// 
    public interface IAuthenticationSessionProvider
    {
        /// <summary>
        /// The login sync.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task<AuthenticationSession> LoginAsync();

        /// <summary>
        /// The logout.
        /// </summary>
        void Logout();
    }
}
