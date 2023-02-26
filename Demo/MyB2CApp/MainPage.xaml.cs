using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace MyB2CApp
{
    public partial class MainPage : ContentPage
    {

        readonly IPublicClientApplication app;

        const string tenantName = "[Your Tenant Name]"; //The name of your tenant
        const string clientId = "[Your Client ID]"; //The Application (client) Id of the AppRegistration you want to use
        const string signUpSignInFlowName = "B2C_1_SignUp_SignIn"; //The Sign Up and Sign In you want to use
        const string redirectUri = "mymauiapp://loggedin"; //One of the RedirectUri's registered with your AppRegistration

        string[] scopes = new string[] { "openid", "offline_access" };
        public MainPage()
        {
            InitializeComponent();

            app = PublicClientApplicationBuilder.Create(clientId)
            .WithB2CAuthority($"https://{tenantName}.b2clogin.com/tfp/{tenantName}.onmicrosoft.com/{signUpSignInFlowName}/oauth2/v2.0/token")
            .WithRedirectUri(redirectUri)
            .Build();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            await InitTokenCache();

            var accounts = await app.GetAccountsAsync(signUpSignInFlowName);

            AuthenticationResult ar = null;

            try
            {
                ar = await app.AcquireTokenSilent(scopes, accounts?.FirstOrDefault())
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                ar = await app.AcquireTokenInteractive(scopes)
                .ExecuteAsync();
            }

            if (ar is not null)
            {
                var claims = ar.ClaimsPrincipal.Claims.ToList();
                var email = claims.Single(c => c.Type.Equals("emails", StringComparison.InvariantCultureIgnoreCase)).Value;

                await DisplayAlert("Welcome!", $"Hi {email}", "OK");
            }
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