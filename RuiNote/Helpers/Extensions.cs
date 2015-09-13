using Cimbalino.Toolkit.Services;

namespace RuiNote.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Removes all back stack.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        public static void RemoveAllBackStack(this INavigationService navigationService)
        {
            while (navigationService.CanGoBack)
            {
                navigationService.RemoveBackEntry();
            }
        }
    }
}
