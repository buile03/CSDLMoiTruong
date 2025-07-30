using System.Security.Claims;
using TechLife.CSDLMoiTruong.App.ApiClients;

namespace TechLife.CSDLMoiTruong.App.Extensions.Authorizations
{
    public class CustomClaimsPrincipal : ClaimsPrincipal
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IUserApiClient _userService;

        public CustomClaimsPrincipal(IUserApiClient userService, ClaimsPrincipal principal) : base(principal)
        {
            _userService = userService;
            _principal = principal;
        }

        public override bool IsInRole(string role)
        {
            try
            {
                if (!_principal.Identity.IsAuthenticated) return false;

                return _userService.IsInRole(_principal.GetUser().Id, role);
            }
            catch
            {
                return false;
            }
        }

        public bool IsInRoles(params string[] roles)
        {
            try
            {
                if (!_principal.Identity.IsAuthenticated) return false;

                return _userService.IsInRoles(_principal.GetUser().Id, roles);

            }
            catch
            {
                return false;
            }
        }
    }
}