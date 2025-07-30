using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Common.Enums
{
    public class EnumDocument
    {
        public enum DocumentType
        {
            [StringValue(@"Văn bản đến")]
            Incoming = 1,
            [StringValue(@"Văn bản đi")]
            Outgoing
        }
        public enum DocumentSecurityLevel
        {
            [StringValue(@"Bình thường")]
            Normal = 1,
            [StringValue(@"Mật")]
            Secret,
            [StringValue(@"Tối mật")]
            TopSecret,
            [StringValue(@"Tuyệt mật")]
            Inscrutability
        }
    }
}
