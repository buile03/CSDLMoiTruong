using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Data.EF;

namespace TechLife.CSDLMoiTruong.Service
{
    public class BaseService
    {
        private readonly AppDbContext _context;
        private const string USER_CONTENT_FOLDER_NAME = "contents";
        public string _action;

        public BaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
