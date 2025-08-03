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
using TechLife.CSDLMoiTruong.Model.DonViCongBo;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface IDonViCongBoService
    {
        Task<PagedResult<DonViCongBoVm>> GetPagings(DonViCongBoGetPagingRequest request);
        Task<List<DonViCongBoVm>> GetAll();
        Task<DonViCongBoVm> GetById(int id);
        Task<Result<int>> Create(DonViCongBoCreateRequest request);
        Task<Result<int>> Update(DonViCongBoUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
    }

    public class DonViCongBoService : BaseService, IDonViCongBoService
    {
        private readonly AppDbContext _context;
        public DonViCongBoService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<DonViCongBoVm>> GetPagings(DonViCongBoGetPagingRequest request)
        {
            try
            {
                var query = from g in _context.DonViCongBo
                            where !g.IsDelete
                            && (string.IsNullOrEmpty(request.Keyword) || (g.Description.Contains(request.Keyword) || g.Name.Contains(request.Keyword) || g.Code.Contains(request.Keyword) || g.DiaChi.Contains(request.Keyword)))
                            select new DonViCongBoVm()
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                DiaChi = g.DiaChi,
                                SoDienThoai = g.SoDienThoai,
                                Email = g.Email,
                                Description = g.Description,
                                Order = g.Order,
                                IsStatus = g.IsStatus,
                            };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync();

                return new PagedResult<DonViCongBoVm>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DonViCongBoVm>> GetAll()
        {
            try
            {
                var query = from g in _context.DonViCongBo
                            where !g.IsDelete && g.IsStatus
                            select new DonViCongBoVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                DiaChi = g.DiaChi,
                                SoDienThoai = g.SoDienThoai,
                                Email = g.Email,
                                Description = g.Description,
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

        public async Task<DonViCongBoVm> GetById(int id)
        {
            try
            {
                var query = from g in _context.DonViCongBo
                            where !g.IsDelete && g.Id == id
                            select new DonViCongBoVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                DiaChi = g.DiaChi,
                                SoDienThoai = g.SoDienThoai,
                                Email = g.Email,
                                Description = g.Description,
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

        public async Task<Result<int>> Create(DonViCongBoCreateRequest request)
        {
            try
            {
                _action = $"Thêm đơn vị tự công bố \"{request.Name}\"";

                if (await _context.DonViCongBo.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error(_action, $"Đơn vị \"{request.Name}\" đã tồn tại");

                int total = await _context.DonViCongBo.CountAsync();

                var obj = new DonViCongBo()
                {
                    Name = request.Name,
                    Code = request.Code,
                    DiaChi = request.DiaChi,
                    SoDienThoai = request.SoDienThoai,
                    Email = request.Email,
                    Description = request.Description,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    CreateByUserId = null,
                    LastModifiedByUserId = null,
                    OrganId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now,
                };
                _context.DonViCongBo.Add(obj);
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

        public async Task<Result<int>> Update(DonViCongBoUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật thông tin đơn vị tự công bố với Id: \"{id}\"";

                var obj = await _context.DonViCongBo.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy đơn vị cần sửa");

                _action = $"Cập nhật thông tin đơn vị \"{obj.Name}\"";

                obj.Name = request.Name.TrimSpace();
                obj.Code = request.Code;
                obj.DiaChi = request.DiaChi.TrimSpace();
                obj.SoDienThoai = request.SoDienThoai;
                obj.Email = request.Email;
                obj.Description = request.Description.TrimSpace();
                obj.OrganId = null;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.DonViCongBo.Update(obj);
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
                _action = $"Xóa đơn vị tự công bố với Id: \"{id}\"";

                var obj = await _context.DonViCongBo.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy đơn vị cần xóa", id);

                _action = $"Xóa đơn vị \"{obj.Name}\"";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.DonViCongBo.Update(obj);

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
                _action = $"Cập nhật vị trí hiển thị đơn vị tự công bố với Id: \"{id}\"";

                var obj = await _context.DonViCongBo.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy đơn vị cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị đơn vị \"{obj.Name}\"";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.DonViCongBo.Update(obj);

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
                _action = $"Cập nhật trạng thái áp dụng đơn vị tự công bố với Id: \"{id}\"";

                var obj = await _context.DonViCongBo.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy đơn vị cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng đơn vị \"{obj.Name}\"";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.DonViCongBo.Update(obj);

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