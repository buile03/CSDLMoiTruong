using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Model
{
    public class NguoiDungVm
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("fullName")]
        public string HoTen { get; set; }
    }
}

