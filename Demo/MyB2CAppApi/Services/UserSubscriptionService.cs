namespace MyB2CAppApi.Services;

public class UserSubscriptionService
{
    public string[] GetUserSubscriptions(string sid)
    {
        //query DB, load subscriptions of user...
        return new string[] { "Free", "Paid" };
    }
}
