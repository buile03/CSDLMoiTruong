using System;
using TechLife.CSDLMoiTruong.Common.Enums;

namespace TechLife.CSDLMoiTruong.App.Models
{
    public class UserLoginRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string HoVaTen { get; set; }
        public string AvartarUrl { get; set; }
        public int OrganId { get; set; }
        public string OrganName { get; set; }
        public string OrganCode { get; set; }
        public string IdToken { get; set; }
        public bool IsSupperUser { get; set; }
        public LoginType LoginType { get; set; }
        public string AccessToken { get; set; }
    }
}
