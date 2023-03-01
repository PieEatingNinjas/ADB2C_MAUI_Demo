using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Net.Http.Headers;

namespace MyB2CApp
{
    public partial class MainPage : ContentPage
    {

        readonly IPublicClientApplication app;

        const string tenantName = "[Your Tenant Name]"; //The name of your tenant
        const string clientId = "[Your Client ID]"; //The Application (client) Id of the AppRegistration you want to use
        const string signUpSignInFlowName = "B2C_1_SignUp_SignIn"; //The Sign Up and Sign In you want to use
        const string redirectUri = "mymauiapp://loggedin"; //One of the RedirectUri's registered with your AppRegistration

        string[] scopes = new string[] { "openid", "offline_access", "https://[Your Tenant Name].onmicrosoft.com/.../[Scope Name]" }; //Name of your API scope
       
        IAccount account = null;
        
        public MainPage()
        {
            InitializeComponent();

            app = PublicClientApplicationBuilder.Create(clientId)
            .WithB2CAuthority($"https://{tenantName}.b2clogin.com/tfp/{tenantName}.onmicrosoft.com/{signUpSignInFlowName}/oauth2/v2.0/token")
            .WithRedirectUri(redirectUri)
            .WithParentActivityOrWindow(() => PlatformConfig.Instance.ParentWindow)
            .Build();
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            await InitTokenCache();

            var accounts = await app.GetAccountsAsync(signUpSignInFlowName);
            account = accounts?.FirstOrDefault();

            AuthenticationResult ar;

            try
            {
                ar = await app.AcquireTokenSilent(scopes, account)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                ar = await app.AcquireTokenInteractive(scopes)
                .ExecuteAsync();

                account = ar.Account;
            }

            if (ar is not null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ar.AccessToken);

                var resultJson = await client.GetStringAsync("https://localhost:7133/UserSubscriptions");

                if (resultJson.Contains("paid", StringComparison.InvariantCultureIgnoreCase))
                    CounterBtn.IsEnabled = true;
            }
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var client = new HttpClient();

            var ar = await app.AcquireTokenSilent(scopes, account).ExecuteAsync();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ar.AccessToken);

            var resultJson = await client.GetStringAsync("https://localhost:7133/Weatherforecast");

            await DisplayAlert("WeatherForecast", resultJson, "ok");
        }

        private async Task InitTokenCache()
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                var storageProperties = new StorageCreationPropertiesBuilder("tokencache.data",
                    FileSystem.Current.CacheDirectory).Build();

                var msalcachehelper = await MsalCacheHelper.CreateAsync(storageProperties);
                msalcachehelper.RegisterCache(app.UserTokenCache);

                await app.GetAccountsAsync().ConfigureAwait(false);
            }
        }
    }
}