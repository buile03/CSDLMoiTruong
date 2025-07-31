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
using TechLife.CSDLMoiTruong.Model.SinhVatGayHai;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface ISinhVatGayHaiService
    {
        Task<PagedResult<SinhVatGayHaiVm>> GetPagings(SinhVatGayHaiGetPagingRequest request);
        Task<List<SinhVatGayHaiVm>> GetAll();
        Task<SinhVatGayHaiVm> GetById(int id);
        Task<Result<int>> Create(SinhVatGayHaiCreateRequest request);
        Task<Result<int>> Update(SinhVatGayHaiUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
    }

    public class SinhVatGayHaiService : BaseService, ISinhVatGayHaiService
    {
        private readonly AppDbContext _context;
        public SinhVatGayHaiService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<SinhVatGayHaiVm>> GetPagings(SinhVatGayHaiGetPagingRequest request)
        {
            try
            {
                var query = from sv in _context.SinhVatGayHais
                            join lct in _context.LoaiCayTrongs on sv.LoaiCayTrongId equals lct.Id
                            where !sv.IsDelete
                            && (request.LoaiCayTrongId == null || sv.LoaiCayTrongId == request.LoaiCayTrongId)
                            && (string.IsNullOrEmpty(request.Keyword) || (sv.Name.Contains(request.Keyword)))
                            select new SinhVatGayHaiVm()
                            {
                                Id = sv.Id,
                                Name = sv.Name,
                                Code = sv.Code,
                                Description = sv.Description,
                                LoaiCayTrongId = sv.LoaiCayTrongId,
                                LoaiCayTrongName = lct.Name,
                                Order = sv.Order,
                                IsStatus = sv.IsStatus,
                            };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                return new PagedResult<SinhVatGayHaiVm>()
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

        public async Task<List<SinhVatGayHaiVm>> GetAll()
        {
            try
            {
                var query = from sv in _context.SinhVatGayHais
                            where !sv.IsDelete && sv.IsStatus
                            select new SinhVatGayHaiVm
                            {
                                Id = sv.Id,
                                Name = sv.Name,
                                Code = sv.Code,
                                Description = sv.Description,
                                Order = sv.Order,
                                IsStatus = sv.IsStatus,
                            };

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<SinhVatGayHaiVm> GetById(int id)
        {
            try
            {
                var query = from sv in _context.SinhVatGayHais
                            where !sv.IsDelete && sv.Id == id
                            select new SinhVatGayHaiVm
                            {
                                Id = sv.Id,
                                Name = sv.Name,
                                Code = sv.Code,
                                Description = sv.Description,
                                Order = sv.Order,
                                IsStatus = sv.IsStatus,
                                LoaiCayTrongId = sv.LoaiCayTrongId,
                            };

                return await query.FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Create(SinhVatGayHaiCreateRequest request)
        {
            try
            {
                _action = $"Thêm sinh vật gây hại \"{request.Name}\"";

                if (await _context.SinhVatGayHais.AnyAsync(x => x.Name == request.Name && x.LoaiCayTrongId == request.LoaiCayTrongId))
                    return Result<int>.Error(_action, $"Sinh vật gây hại \"{request.Name}\" đã tồn tại cho loại cây trồng này");

                int total = await _context.SinhVatGayHais.CountAsync();

                var obj = new SinhVatGayHai()
                {
                    Name = request.Name,
                    Code = request.Code,
                    Description = request.Description,
                    LoaiCayTrongId = request.LoaiCayTrongId,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    CreateByUserId = null,
                    LastModifiedByUserId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now,
                };
                _context.SinhVatGayHais.Add(obj);
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

        public async Task<Result<int>> Update(SinhVatGayHaiUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật thông tin sinh vật gây hại với Id: \"{id}\"";

                var obj = await _context.SinhVatGayHais.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy sinh vật gây hại cần sửa");

                _action = $"Cập nhật thông tin sinh vật gây hại \"{obj.Name}\"";

                obj.Name = request.Name.TrimSpace();
                obj.Code = request.Code;
                obj.Description = request.Description.TrimSpace();
                obj.LoaiCayTrongId = request.LoaiCayTrongId;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SinhVatGayHais.Update(obj);
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
                _action = $"Xóa sinh vật gây hại với Id: \"{id}\"";

                var obj = await _context.SinhVatGayHais.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy sinh vật gây hại cần xóa", id);

                _action = $"Xóa sinh vật gây hại \"{obj.Name}\"";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SinhVatGayHais.Update(obj);

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
                _action = $"Cập nhật vị trí hiển thị sinh vật gây hại với Id: \"{id}\"";

                var obj = await _context.SinhVatGayHais.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy sinh vật gây hại cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị sinh vật gây hại \"{obj.Name}\"";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SinhVatGayHais.Update(obj);

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
                _action = $"Cập nhật trạng thái áp dụng sinh vật gây hại với Id: \"{id}\"";

                var obj = await _context.SinhVatGayHais.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy sinh vật gây hại cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng sinh vật gây hại \"{obj.Name}\"";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SinhVatGayHais.Update(obj);

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