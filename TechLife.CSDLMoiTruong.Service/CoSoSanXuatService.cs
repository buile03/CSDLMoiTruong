using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Data.EF;
using TechLife.CSDLMoiTruong.Data.Entities;
using TechLife.CSDLMoiTruong.Model.CoSoSanXuat;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface ICoSoSanXuatService
    {
        Task<PagedResult<CoSoSanXuatVm>> GetPagings(CoSoSanXuatGetPagingRequest request);
        Task<List<CoSoSanXuatVm>> GetAll();
        Task<CoSoSanXuatVm> GetById(int id);
        Task<Result<int>> Create(CoSoSanXuatCreateRequest request);
        Task<Result<int>> Update(CoSoSanXuatUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
    }

    public class CoSoSanXuatService : BaseService, ICoSoSanXuatService
    {
        private readonly AppDbContext _context;
        public CoSoSanXuatService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<CoSoSanXuatVm>> GetPagings(CoSoSanXuatGetPagingRequest request)
        {
            try
            {
                var query = from g in _context.CoSoSanXuat
                            where !g.IsDelete
                            && (string.IsNullOrEmpty(request.Keyword) || g.DiaChi.Contains(request.Keyword) || g.DienThoai.Contains(request.Keyword))
                           select new CoSoSanXuatVm()
                           {
                               Id = g.Id,
                               Name = g.Name,
                               Code = g.Code,
                               DiaChi = g.DiaChi,
                               DienThoai = g.DienThoai,
                               Email = g.Email,
                               ChuCoSo = g.ChuCoSo,
                               MaSoThue = g.MaSoThue,
                               GhiChu = g.GhiChu,
                               Order = g.Order,
                               IsStatus = g.IsStatus,
                           };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                var pagedResult = new PagedResult<CoSoSanXuatVm>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };
                return pagedResult;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<CoSoSanXuatVm>> GetAll()
        {
            try
            {
                var query = from g in _context.CoSoSanXuat
                            where !g.IsDelete && g.IsStatus
                            select new CoSoSanXuatVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                DiaChi = g.DiaChi,
                                DienThoai = g.DienThoai,
                                Email = g.Email,
                                ChuCoSo = g.ChuCoSo,
                                MaSoThue = g.MaSoThue,
                                GhiChu = g.GhiChu,
                                Order = g.Order,
                                IsStatus = g.IsStatus,
                            };

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<CoSoSanXuatVm> GetById(int id)
        {
            try
            {
                var query = from g in _context.CoSoSanXuat
                            where !g.IsDelete && g.Id == id
                            select new CoSoSanXuatVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                DiaChi = g.DiaChi,
                                DienThoai = g.DienThoai,
                                Email = g.Email,
                                ChuCoSo = g.ChuCoSo,
                                MaSoThue = g.MaSoThue,
                                GhiChu = g.GhiChu,
                                Order = g.Order,
                                IsStatus = g.IsStatus,
                            };

                return await query.FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Create(CoSoSanXuatCreateRequest request)
        {
            try
            {
                _action = $"Thêm cơ sở sản xuất \"{request.Name}\"";

                if (await _context.CoSoSanXuat.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error(_action, $"Cơ sở sản xuất \"{request.Name}\" đã tồn tại");

                int total = await _context.CoSoSanXuat.CountAsync();

                var obj = new CoSoSanXuat()
                {
                    Name = request.Name,
                    DiaChi = request.DiaChi,
                    Code = request.Code,
                    DienThoai = request.DienThoai,
                    Email = request.Email,
                    ChuCoSo = request.ChuCoSo,
                    MaSoThue = request.MaSoThue,
                    GhiChu = request.GhiChu,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    CreateByUserId = null,
                    LastModifiedByUserId = null,
                    OrganId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now,
                };
                _context.CoSoSanXuat.Add(obj);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return Result<int>.Success(_action, obj.Id);

                return Result<int>.Error(_action);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Update(CoSoSanXuatUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();

                _action = $"Cập nhật thông tin cơ sở sản xuất với Id: \"{id}\"";

                var obj = await _context.CoSoSanXuat.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy cơ sở sản xuất cần sửa");

                _action = $"Cập nhật thông tin cơ sở sản xuất \"{obj.Name}\"";

                obj.Name = request.Name.TrimSpace();
                obj.Code = request.Code;
                obj.DiaChi = request.DiaChi;
                obj.DienThoai = request.DienThoai;
                obj.Email = request.Email;
                obj.ChuCoSo = request.ChuCoSo;
                obj.MaSoThue = request.MaSoThue;
                obj.GhiChu = request.GhiChu;
                obj.LastModifiedByUserId = null;
                obj.OrganId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.CoSoSanXuat.Update(obj);
                var result = await base.SaveChange();

                if (result > 0)
                    return Result<int>.Success(_action, id);

                return Result<int>.Error(_action, id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Delete(DeleteRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();

                _action = $"Xóa cơ sở sản xuất với Id: \"{id}\"";

                var obj = await _context.CoSoSanXuat.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy cơ sở sản xuất cần xóa", id);

                _action = $"Xóa cơ sở sản xuất \"{obj.Name}\"";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.CoSoSanXuat.Update(obj);

                var result = await base.SaveChange();
                if (result > 0)
                    return Result<int>.Success(_action, id);

                return Result<int>.Error(_action, id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> UpdateOrder(UpdateOrderRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();

                _action = $"Cập nhật vị trí hiển thị cơ sở sản xuất với Id: \"{id}\"";

                var obj = await _context.CoSoSanXuat.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy cơ sở sản xuất cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị cơ sở sản xuất \"{obj.Name}\"";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.CoSoSanXuat.Update(obj);

                var result = await base.SaveChange();
                if (result > 0)
                    return Result<int>.Success(_action, id);

                return Result<int>.Error(_action, id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> UpdateStatus(UpdateStatusRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();

                _action = $"Cập nhật trạng thái áp dụng cơ sở sản xuất với Id: \"{id}\"";

                var obj = await _context.CoSoSanXuat.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy cơ sở sản xuất cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng cơ sở sản xuất \"{obj.Name}\"";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.CoSoSanXuat.Update(obj);

                var result = await base.SaveChange();
                if (result > 0)
                    return Result<int>.Success(_action, id);

                return Result<int>.Error(_action, id);
            }
            catch
            {
                throw;
            }
        }
    }
}