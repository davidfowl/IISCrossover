using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IISCrossover.Authentication.Core
{
    public class CrossoverAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserManager<PocoUser> _userManager;
        private readonly RoleManager<PocoRole> _roleManager;
        private readonly SignInManager<PocoUser> _signInManager;

        public CrossoverAuthenticationMiddleware(RequestDelegate next,
                                                 UserManager<PocoUser> userManager,
                                                 RoleManager<PocoRole> roleManager,
                                                 SignInManager<PocoUser> signInManager)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<PocoUser> GetOrCreateUserAsync(string userName, string password)
        {
            PocoUser user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new PocoUser
                {
                    UserName = userName
                };
                await _userManager.CreateAsync(user, password);
            };

            return user ?? await _userManager.FindByNameAsync(userName);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userName = context.GetServerVariable(CrossoverAuthenticationVars.UserName);

            if (!string.IsNullOrWhiteSpace(userName))
            {
                const string password = "[PLACEHOLDER]-1a";
                var user = await GetOrCreateUserAsync(userName, password);
                
                var rolesText = context.GetServerVariable(CrossoverAuthenticationVars.Roles);
                if (!string.IsNullOrWhiteSpace(rolesText))
                {
                    // TODO - discuss how to make this more safe without bloating the project
                    foreach (var role in rolesText.Split(','))
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            await _roleManager.CreateAsync(new PocoRole(role));
                        }

                        if (!await _userManager.IsInRoleAsync(user, role))
                        {
                            try
                            {
                                var result = await _userManager.AddToRoleAsync(user, role);
                                if (!result.Succeeded)
                                {
                                    await context.Response.WriteAsync($"There were errors:{Environment.NewLine}");
                                    foreach (var error in result.Errors)
                                    {
                                        await context.Response.WriteAsync($"   {error}");
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                await context.Response.WriteAsync($"Unhandled exception: {Environment.NewLine}");
                                await context.Response.WriteAsync($"    {ex.Message}{Environment.NewLine}");
                                await context.Response.WriteAsync($"    {ex.StackTrace}{Environment.NewLine}");
                            }
                        }
                    }
                }

                var signInResult = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: true, lockoutOnFailure: false);

                #region AWESOME middleware debugging
                //await context.Response.WriteAsync($"Hello {userName}{Environment.NewLine}");
                //if (signInResult.Succeeded)
                //{
                //    await context.Response.WriteAsync($"Signin was SUCCESSFULL!!{Environment.NewLine}");
                //}
                //else
                //{
                //    await context.Response.WriteAsync($"Signin did not succeed... bummer{Environment.NewLine}");
                //}

                ////var user2 = await _userManager.FindByNameAsync(userName);
                //if (user.Roles == null || user.Roles.Count == 0)
                //{
                //    await context.Response.WriteAsync($"   No roles assigned to user{Environment.NewLine}");
                //}
                //foreach (var role in user.Roles)
                //{
                //    await context.Response.WriteAsync($"    User has role {role}{Environment.NewLine}");
                //}

                //await IsInRole(context, "Administrator");
                //await IsInRole(context, "WeatherReader");
                #endregion
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

        private async Task IsInRole(HttpContext context, string roleName)
        {
            var isInRole = context.User.IsInRole(roleName);
            await context.Response.WriteAsync($"user isInRole '{roleName}': {isInRole}{Environment.NewLine}");
        }
    }
}
