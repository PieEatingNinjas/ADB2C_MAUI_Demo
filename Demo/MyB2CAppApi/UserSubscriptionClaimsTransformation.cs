using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using MyB2CAppApi.Services;
using System.Security.Claims;

public class UserSubscriptionClaimsTransformation : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        var claimType = "subscriptions";

        var ps = new UserSubscriptionService(); //should be injected
        var subscriptions = ps.GetUserSubscriptions(principal.FindFirst(ClaimConstants.ObjectId)!.Value);

        foreach (var sub in subscriptions)
        {
            claimsIdentity.AddClaim(new Claim(claimType, sub));
        }

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}