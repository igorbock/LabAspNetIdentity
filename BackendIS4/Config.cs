namespace BackendIS4;

public static class Config
{
    public static IEnumerable<ApiScope> ApiScopes
    {
        get
        {
            return new List<ApiScope>
            {
                new ApiScope("API-LAB", "API Laboratório")
                {
                    UserClaims = new[] { "username" }
                }
            };
        }
    }

    public static IEnumerable<Client> Clients
    {
        get
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ClientLab",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("lab_segredo".Sha256())
                    },

                    AllowedScopes = { "API-LAB" }
                },
                new Client
                {
                    ClientId = "ClientLab2",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    //AccessTokenLifetime = 120, //86400,
                    //IdentityTokenLifetime = 120, //86400,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    SlidingRefreshTokenLifetime = 30,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AlwaysSendClientClaims = true,
                    ClientSecrets=  new List<Secret> { new Secret("lab_segredo".Sha256()) },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "API-LAB"
                    }
                }
            };
        }
    }

    public static IEnumerable<ApiResource> ApiResources
    {
        get
        {
            return new List<ApiResource>
            {
                new ApiResource("API-Resource-LAB", "API Resource Laboratório")
            };
        }
    }

    public static IEnumerable<IdentityResource> IdentityResources
    {
        get
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
    }

    public static List<TestUser> TestUsers
    {
        get
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = new Guid().ToString(),
                    Username = "Igor",
                    Password = "Igor123@",
                    IsActive = true
                },
                new TestUser
                {
                    SubjectId = new Guid().ToString(),
                    Username = "Rafaela",
                    Password = "Rafa123@",
                    IsActive = true
                }
            };
        }
    }
}
