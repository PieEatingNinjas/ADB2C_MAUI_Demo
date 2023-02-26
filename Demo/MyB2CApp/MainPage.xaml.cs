using Microsoft.Identity.Client;

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
            AuthenticationResult ar = await app.AcquireTokenInteractive(scopes)
            .ExecuteAsync();

            var claims = ar.ClaimsPrincipal.Claims.ToList();

            var email = claims.Single(c => c.Type.Equals("emails", StringComparison.InvariantCultureIgnoreCase)).Value;

            await DisplayAlert("Welcome!", $"Hi {email}", "OK");

        }
    }
}