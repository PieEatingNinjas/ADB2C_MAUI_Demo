using Android.App;
using Android.Content;
using Microsoft.Identity.Client;


namespace MyB2CApp.Platforms.Android
{

    [Activity(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataHost = "loggedin", DataScheme = "mymauiapp")]
    //DataScheme and DataHost should match the provided RedirectUri:
    //[DataScheme]://[DataHost]
    public class MsalActivity : BrowserTabActivity
    {
    }
}
