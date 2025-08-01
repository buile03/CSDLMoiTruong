using System;
using System.Collections.Generic;
using System.Text;
using TechLife.CSDLMoiTruong.Common.Enums;

namespace TechLife.CSDLMoiTruong.Common.Enums
{
    public enum HeThong : int
    {
        [StringValue(@"Trang chủ")]
        home = 1,
        [StringValue(@"Sinh vật gây hại")]
        sinhvatgayhai,
        [StringValue(@"Thời tiết & sinh trưởng")]
        thoitietvasinhtruong,
        [StringValue(@"Chất lượng sản phẩm")]
        chatluongsanpham,
        [StringValue(@"Cở sở giống cây trồng")]
        cosogiongcaytrong,
        [StringValue(@"Hệ thống")]
        hethong,
    }
   
    public enum LoginType : int
    {
        [StringValue(@"Đăng nhập chuẩn")]
        None = 0,
        [StringValue(@"SSO Huế")]
        SSOHue,
        [StringValue(@"SSO HueS - VNeID")]
        SSOHueS,
        [StringValue(@"SSO Cơ quan/Tổ chức")]
        SSOOrganHueS
    }
    public enum MucDoNhiem : int
    {
        [StringValue(@"Mức độ nhẹ")]
        nhe = 1,
        [StringValue(@"Mức độ trung bình")]
        trungbinh = 2,
        [StringValue(@"Mức độ nặng")]
        nang = 3
    }
}
