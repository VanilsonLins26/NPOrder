using System.Security.Claims;
using IdentityModel;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServerAspNetIdentity;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        // 1. Criação do Escopo (Sintaxe Simplificada)
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();


                Log.Information("Iniciando Migração do Banco de Dados...");
                await context.Database.MigrateAsync();
                Log.Information("Migração concluída.");

                var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

                var adminRole = "Admin";
                if (!await roleMgr.RoleExistsAsync(adminRole))
                {
                    await roleMgr.CreateAsync(new IdentityRole(adminRole));
                    Log.Information("Role 'Admin' criada.");
                }


                var adminUser = await userMgr.FindByNameAsync("admin");
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        Name = "admin",
                        UserName = "admin@teste.com",
                        Email = "admin@teste.com",
                        EmailConfirmed = true,
                    };

                    var result = await userMgr.CreateAsync(adminUser, "Admin@123");

                    if (!result.Succeeded)
                    {
                        throw new Exception("Falha ao criar Admin: " + result.Errors.First().Description);
                    }


                    await userMgr.AddClaimsAsync(adminUser, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Administrador Master"),
                        new Claim(JwtClaimTypes.GivenName, "Admin"),
                        new Claim(JwtClaimTypes.FamilyName, "Sistema"),
                        new Claim(JwtClaimTypes.Role, "Admin")
                    });

                    await userMgr.AddToRoleAsync(adminUser, adminRole);
                    Log.Information("Usuário Admin criado e vinculado à role.");
                }
                else
                {
                    Log.Information("Usuário Admin já existe. Pulando seed.");
                }
            }
            catch (Exception ex)
            {
              
                Log.Error(ex, "ERRO CRÍTICO DURANTE O SEED DO BANCO.");
                throw; 
            }
        }
    }
}
