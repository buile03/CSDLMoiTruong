using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.SoLieuSinhTruong
{
    public class ImportExcelRequest : RequestBase
    {
        public IFormFile File { get; set; }
        public int CayTrongId { get; set; }
    }

}
