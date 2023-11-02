using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PasswordManagerApp.Components.Dialog;
using RestSharp;
using System.Net;

namespace PasswordManagerApp.Pages
{
    public class IndexComponent : ComponentBase
    {
        public string apiUrl = "https://localhost:7045/Password";

        public Modal modal;
        public bool visualize = false;

        [Inject]
        public IDialogService DialogService { get; set; }

        public string MainPassword { get; set; }
        public string NameAccount { get; set; }
        public string PasswordAccount { get; set; }
        public List<PasswordObj> Passwords { get; set; }

        public RestClient RestClient { get; set; }

        public void AddPassword(string name, string password)
        {
            var passwordRequest = new RestRequest();
            passwordRequest.AddBody(new
            {
                name = name,
                password = password
            });

            var passwordResponse = RestClient.ExecutePost(passwordRequest);

            if (!string.IsNullOrEmpty(passwordResponse.Content))
            {
                var response = JsonConvert.DeserializeObject<Response>(passwordResponse.Content);

                if (response != null)
                {
                    Passwords.Add(new PasswordObj
                    {
                        Id = int.Parse(response.msg),
                        Name = name
                    });

                    OnHideModalClick();
                    StateHasChanged();
                }
            }
        }

        public void DeletePassword(int passwordId)
        {
            var httpStatus = RestClient.Delete(new RestRequest
            {
                Resource = apiUrl + $"?passwordId={passwordId}"
            });

            if (httpStatus.StatusCode == HttpStatusCode.OK)
            {
                var selectedPassword = Passwords.FirstOrDefault(x => x.Id == passwordId);
                Passwords.Remove(selectedPassword);
                StateHasChanged();
            }
            else
                DialogService.DisplayConfirm("Erro", $"", "", "OK");
        }

        public async Task OnHideModalClick()
        {
            await modal?.HideAsync();
        }

        public async Task OnShowModalClick()
        {
            await modal?.ShowAsync();
        }

        public async Task OnShowModalPassword()
        {
            await DialogService.DisplayConfirm("Senha", $"{MainPassword}", "", "OK");
        }

        public async Task VisualizePassword(int passwordId)
        {
            visualize = true;

            var passwordsRequest = RestClient.GetJson<Response>(apiUrl + $"/password?passwordId={passwordId}");
            if (!string.IsNullOrEmpty(passwordsRequest.msg))
            {
                MainPassword = passwordsRequest.msg;
                await OnShowModalPassword();
            }

            visualize = false;
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            RestClient = new RestClient(apiUrl);
            var request = new RestRequest();

            var passwordsRequest = RestClient.ExecuteGet(request);
            if (passwordsRequest.Content != null)
            {
                var passwordsJsonMessage = JsonConvert.DeserializeObject<Response>(passwordsRequest.Content);

                var passwordsJson = JsonConvert.DeserializeObject<List<PasswordObj>>(passwordsJsonMessage.msg);
                if (passwordsJson != null && passwordsJson.Count() > 0)
                    Passwords = passwordsJson;
                else
                    Passwords = new List<PasswordObj>();
            }

            base.OnInitialized();
        }
    }

    public class PasswordObj
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Response
    {
        public string msg { get; set; }
    }
}