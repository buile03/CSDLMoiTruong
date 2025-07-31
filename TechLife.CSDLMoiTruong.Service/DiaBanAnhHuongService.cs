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
using TechLife.CSDLMoiTruong.Model.DiaBanAnhHuong;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface IDiaBanAnhHuongService
    {
        Task<PagedResult<DiaBanAnhHuongVm>> GetPagings(GetPagingRequest request);
        Task<List<DiaBanAnhHuongVm>> GetAll();
        Task<DiaBanAnhHuongVm> GetById(int id);
        Task<Result<int>> Create(DiaBanAnhHuongCreateRequest request);
        Task<Result<int>> Update(DiaBanAnhHuongUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
    }
    public class DiaBanAnhHuongService : BaseService, IDiaBanAnhHuongService
    {
        private readonly AppDbContext _context;

        public DiaBanAnhHuongService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<DiaBanAnhHuongVm>> GetPagings(GetPagingRequest request)
        {
            try
            {
                var query = from g in _context.DiaBanAnhHuongs
                            where !g.IsDelete
                            && (string.IsNullOrEmpty(request.Keyword) ||
                               (g.Description.Contains(request.Keyword) || g.Name.Contains(request.Keyword)))
                            select new DiaBanAnhHuongVm()
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                Description = g.Description,
                                Order = g.Order,
                                IsStatus = g.IsStatus,
                            };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync();

                return new PagedResult<DiaBanAnhHuongVm>()
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

        public async Task<List<DiaBanAnhHuongVm>> GetAll()
        {
            try
            {
                var query = from g in _context.DiaBanAnhHuongs
                            where !g.IsDelete && g.IsStatus
                            select new DiaBanAnhHuongVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
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

        public async Task<DiaBanAnhHuongVm> GetById(int id)
        {
            try
            {
                var query = from g in _context.DiaBanAnhHuongs
                            where !g.IsDelete && g.Id == id
                            select new DiaBanAnhHuongVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
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

        public async Task<Result<int>> Create(DiaBanAnhHuongCreateRequest request)
        {
            try
            {
                _action = $"Thêm địa bàn ảnh hưởng \"{request.Name}\"";

                if (await _context.DiaBanAnhHuongs.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error(_action, $"Địa bàn ảnh hưởng \"{request.Name}\" đã tồn tại");

                int total = await _context.DiaBanAnhHuongs.CountAsync();

                var obj = new DiaBanAnhHuong()
                {
                    Name = request.Name,
                    Code = request.Code,
                    Description = request.Description,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    CreateByUserId = null,
                    LastModifiedByUserId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now,
                };

                _context.DiaBanAnhHuongs.Add(obj);
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

        public async Task<Result<int>> Update(DiaBanAnhHuongUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật thông tin địa bàn ảnh hưởng với Id: \"{id}\"";

                var obj = await _context.DiaBanAnhHuongs.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy địa bàn ảnh hưởng cần sửa");

                _action = $"Cập nhật thông tin địa bàn ảnh hưởng \"{obj.Name}\"";

                obj.Name = request.Name.TrimSpace();
                obj.Code = request.Code;
                obj.Description = request.Description.TrimSpace();
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.DiaBanAnhHuongs.Update(obj);
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
                _action = $"Xóa địa bàn ảnh hưởng với Id: \"{id}\"";

                var obj = await _context.DiaBanAnhHuongs.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy địa bàn ảnh hưởng cần xóa", id);

                _action = $"Xóa địa bàn ảnh hưởng \"{obj.Name}\"";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.DiaBanAnhHuongs.Update(obj);
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
                _action = $"Cập nhật vị trí hiển thị địa bàn ảnh hưởng với Id: \"{id}\"";

                var obj = await _context.DiaBanAnhHuongs.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy địa bàn ảnh hưởng cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị địa bàn ảnh hưởng \"{obj.Name}\"";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.DiaBanAnhHuongs.Update(obj);
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
                _action = $"Cập nhật trạng thái áp dụng địa bàn ảnh hưởng với Id: \"{id}\"";

                var obj = await _context.DiaBanAnhHuongs.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy địa bàn ảnh hưởng cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng địa bàn ảnh hưởng \"{obj.Name}\"";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.DiaBanAnhHuongs.Update(obj);
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
