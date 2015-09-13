using System.Threading.Tasks;
using RuiNote.Services.Model;

namespace RuiNote.Services.Interfaces
{
    /// <summary>
    /// The SessionService interface.
    /// </summary>
    public interface IAuthenticationSessionService
    {
        /// <summary>
        /// The get authentication session.
        /// </summary>
        /// <returns>
        /// The <see cref="AuthenticationSession"/>.
        /// </returns>
        AuthenticationSession GetAuthenticationSession();

        /// <summary>
        /// The login async.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task<bool?> LoginAsync(string provider);

        /// <summary>
        /// The logout.
        /// </summary>
        void Logout();
    }
}
