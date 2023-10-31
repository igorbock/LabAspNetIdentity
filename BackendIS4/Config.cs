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
