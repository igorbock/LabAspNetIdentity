namespace ShieldJWT.Extensions;

public static class ShieldClaimExtensions
{
    public static IEnumerable<Claim> ToSystemClaim(this IEnumerable<ShieldClaim> shieldClaims)
    {
        var returnClaims = new List<Claim>();

        foreach (var item in shieldClaims)
            returnClaims.Add(new Claim(item.Name, item.Value));

        return returnClaims;
    }
}
