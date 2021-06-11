using System.Threading.Tasks;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace SampleBluetooth
{
    public interface IPageNavService
    {
        Task PopAsync();
        Task PopModalAsync();
        Task PopAsync(IPopupNavigation pop);
        Task PushAsync(Page destPage);
        Task PushAsync(IPopupNavigation popNav, PopupPage destPage);
        Task PushModalAsync(Page destPage);
        Task<bool> DisplayAlert(string title, string msg, string ok, string cancel, bool allowBackgroundExit = false);
        Task DisplayAlert(string title, string msg, string ok, bool allowBackgroundExit = false);
    }

    //TODO: Should make this support popAsync for popupNavigation as well.


    /// <summary>
    /// decouple view model dependency from xamarin page navigation
    /// </summary>
    public class PageNavService : IPageNavService
    {

        private Page _page;

        public PageNavService(Page page)
        {
            _page = page;
        }

        public async Task<bool> DisplayAlert(string title, string msg, string ok, string cancel, bool allowBackgroundExit = false)
        {
            if (allowBackgroundExit)
            {
                return await _page.DisplayAlert(title, msg, ok, cancel);
            }
            return await UserDialogs.Instance.ConfirmAsync(msg, title, ok, cancel);
        }

        public async Task DisplayAlert(string title, string msg, string ok, bool allowBackgroundExit = false)
        {
            if (allowBackgroundExit)
            {
                await _page.DisplayAlert(title, msg, ok);
            }
            await UserDialogs.Instance.AlertAsync(msg, title, ok);
        }

        public async Task PopAsync()
        {
            await _page.Navigation.PopAsync();
        }

        public async Task PopAsync(IPopupNavigation pop)
        {
            await pop.PopAsync();
        }

        public async Task PopModalAsync()
        {
            await _page.Navigation.PopModalAsync();
        }

        public async Task PushAsync(Page destPage)
        {
            await _page.Navigation.PushAsync(destPage);
        }

        public async Task PushAsync(IPopupNavigation popNav, PopupPage destPage)
        {
            await popNav.PushAsync(destPage);
        }

        public async Task PushModalAsync(Page destPage)
        {
            await _page.Navigation.PushModalAsync(destPage);
        }
    }
}