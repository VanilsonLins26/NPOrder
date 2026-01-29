using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServerAspNetIdentity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "User Roles", new[] { "role" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("nporder_api", "API do NPOrder")
            {
                UserClaims = new List<string> { "phone_number", "mobile_phone" }
            },
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },

            new Client
        {
            ClientId = "swagger_ui",
            ClientName = "Swagger UI da NP_Encomendas_BackEnd",
            ClientSecrets = { new Secret("minha_senha_super_secreta_120127".Sha256()) }, // Pode ser qualquer senha

            AllowedGrantTypes = GrantTypes.Code, // Padrão para apps web
            RedirectUris = { "https://backend-api-tk7o.onrender.com/swagger/oauth2-redirect.html" }, // URL do seu Swagger (NP_Encomendas_BackEnd)
            PostLogoutRedirectUris = { "https://backend-api-tk7o.onrender.com/swagger/" },

            AllowedCorsOrigins = { "https://backend-api-tk7o.onrender.com" },

            RequirePkce = false,

            AllowedScopes = new List<string>
            {
                "openid",
                "profile",
                "nporder_api" // O escopo da sua API
            }
        },
            new Client
            {
                ClientId = "angular_client",
                ClientName = "Swagger UI da NP_Encomendas_BackEnd",
                RequireClientSecret = false,

                AllowedGrantTypes = GrantTypes.Code, 
                RedirectUris = { "https://localhost:4200" },
                PostLogoutRedirectUris = { "https://localhost:4200" },
                AllowOfflineAccess = true,



               AllowedCorsOrigins = { "https://localhost:4200" },

               RequirePkce = true,

               AllowedScopes = new List<string>
               {
                IdentityServerConstants.StandardScopes.OpenId,  
                IdentityServerConstants.StandardScopes.Profile, 
                "nporder_api",
                "roles"
               }

            }
        };

    public static IEnumerable<ApiResource> ApiResources =>
    new List<ApiResource>
    {

        new ApiResource("nporder_api", "API do NPOrder")
        {
            Scopes = { "nporder_api" } ,
            UserClaims = { JwtClaimTypes.Role }
        }
    };
}
