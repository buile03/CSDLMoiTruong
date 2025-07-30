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
        [StringValue(@"Bài viết đã xuất bản")]
        baivietdaxuatban,
        [StringValue(@"Tin bài")]
        tinbai,
        [StringValue(@"Ban biên tập")]
        banbientap,
        [StringValue(@"Lương cơ sở")]
        mucluongcoso,
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

    //Chức vụ ban biên tập
    public enum ChucVuBanBienTap : int
    {

        [StringValue(@"Trưởng ban")]
        TruongBan = 1,

        [StringValue(@"Phó trưởng ban")]
        PhoTruongBan = 2,

        [StringValue(@"Thành viên")]
        ThanhVien = 3,
    }
    public enum PhanLoaiTin : int
    {
        [StringValue(@"Tin")]
        Tin = 1,
        [StringValue(@"Bài viết ngắn")]
        BaiVietNgan = 2
    }
}
