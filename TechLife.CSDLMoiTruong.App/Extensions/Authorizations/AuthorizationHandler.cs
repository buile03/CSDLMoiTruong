using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Models;

namespace TechLife.CSDLMoiTruong.App.Extensions.Authorizations
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        private readonly IUserApiClient _userService;
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(IUserApiClient userService,
            ILogger<AuthorizationHandler> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            try
            {
                var pendingRequirements = context.Requirements.ToList();
                foreach (var requirement in pendingRequirements)
                {
                    if (requirement is RolesAuthorizationRequirement)
                    {
                        if (IsInRoleRequirement(context.User.GetUser(), context.Resource, requirement))
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                        }
                    }
                }
            }
            catch
            {
                context.Fail();
            }
            await Task.CompletedTask;
        }

        private bool IsInRoleRequirement(UserLoginRequest user, object resource, IAuthorizationRequirement requirement)
        {
            try
            {
                var require = requirement as RolesAuthorizationRequirement;
                if (require == null)
                {
                    _logger.LogWarning("Requirement is not RolesAuthorizationRequirement");
                    return false;
                }

                if (_userService == null)
                {
                    _logger.LogWarning("_userService is null");
                    return false;
                }
                if (user == null) return false;

                return _userService.IsInRoles(user.Id, require.AllowedRoles.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xác thực vai trò");
                return false;
            }
        }
    }
}