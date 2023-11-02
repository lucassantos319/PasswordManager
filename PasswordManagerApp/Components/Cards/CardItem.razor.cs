using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PasswordManagerApp.Pages;
using RestSharp;

namespace PasswordManagerApp.Components.Cards
{
    public class CardItemComponent : ComponentBase
    {
        public string apiUrl = "https://localhost:7045/Password/password";

        protected BlazorBootstrap.Modal modal = default!;
        public string MainPassword { get; set; }

        [Parameter]
        public PasswordObj Password { get; set; }

        public RestClient RestClient { get; set; }

        public async Task VisualizePassword()
        {
            RestClient = new RestClient();

            var passwordsRequest = RestClient.GetJson<Response>(apiUrl + $"?passwordId={Password.Id}");
            if (!string.IsNullOrEmpty(passwordsRequest.msg))
            {
                MainPassword = passwordsRequest.msg;
                await OnShowModalClick();
            }
        }

        protected async Task OnHideModalClick()
        {
            await modal.HideAsync();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected async Task OnShowModalClick()
        {
            await modal.ShowAsync();
        }
    }

    public class Response
    {
        public string msg { get; set; }
    }
}