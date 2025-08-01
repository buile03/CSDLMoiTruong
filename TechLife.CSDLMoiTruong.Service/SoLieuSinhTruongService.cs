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
using TechLife.CSDLMoiTruong.Model.SoLieuSinhTruong;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface ISoLieuSinhTruongService
    {
        Task<PagedResult<SoLieuSinhTruongVm>> GetPagings(SoLieuSinhTruongGetPagingRequest request);
        Task<List<SoLieuSinhTruongVm>> GetAll();
        Task<SoLieuSinhTruongVm> GetById(int id);
        Task<Result<int>> Create(SoLieuSinhTruongCreateRequest request);
        Task<Result<int>> Update(SoLieuSinhTruongUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
    }

    public class SoLieuSinhTruongService : BaseService, ISoLieuSinhTruongService
    {
        private readonly AppDbContext _context;
        public SoLieuSinhTruongService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<SoLieuSinhTruongVm>> GetPagings(SoLieuSinhTruongGetPagingRequest request)
        {
            try
            {
                var query = from s in _context.SoLieuSinhTruong
                            join c in _context.LoaiCayTrong on s.CayTrongId equals c.Id
                            where !s.IsDelete
                            && (request.CayTrongId == null || s.CayTrongId == request.CayTrongId)
                            && (string.IsNullOrEmpty(request.Keyword) || s.MoTa.Contains(request.Keyword))
                            select new SoLieuSinhTruongVm()
                            {
                                Id = s.Id,
                                CayTrongId = s.CayTrongId,
                                TenCayTrong = c.Name,
                                TuNgay = s.TuNgay,
                                DenNgay = s.DenNgay,
                                KeHoach = s.KeHoach,
                                DaGieoTrong = s.DaGieoTrong,
                                MoTa = s.MoTa,
                                Order = s.Order,
                                IsStatus = s.IsStatus,
                            };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync();

                return new PagedResult<SoLieuSinhTruongVm>()
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

        public async Task<List<SoLieuSinhTruongVm>> GetAll()
        {
            try
            {
                var query = from s in _context.SoLieuSinhTruong
                            where !s.IsDelete && s.IsStatus
                            select new SoLieuSinhTruongVm
                            {
                                Id = s.Id,
                                TuNgay = s.TuNgay,
                                DenNgay = s.DenNgay,
                                KeHoach = s.KeHoach,
                                DaGieoTrong = s.DaGieoTrong,
                                MoTa = s.MoTa,
                                Order = s.Order,
                                IsStatus = s.IsStatus,
                            };

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<SoLieuSinhTruongVm> GetById(int id)
        {
            try
            {
                var query = from s in _context.SoLieuSinhTruong
                            where !s.IsDelete && s.Id == id
                            select new SoLieuSinhTruongVm
                            {
                                Id = s.Id,
                                TuNgay = s.TuNgay,
                                DenNgay = s.DenNgay,
                                KeHoach = s.KeHoach,
                                DaGieoTrong = s.DaGieoTrong,
                                MoTa = s.MoTa,
                                Order = s.Order,
                                IsStatus = s.IsStatus,
                                CayTrongId = s.CayTrongId,
                            };

                return await query.FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Create(SoLieuSinhTruongCreateRequest request)
        {
            try
            {
                _action = $"Thêm số liệu sinh trưởng từ ngày {request.TuNgay:dd/MM/yyyy} đến {request.DenNgay:dd/MM/yyyy}";

                if (await _context.SoLieuSinhTruong.AnyAsync(x =>
                    x.CayTrongId == request.CayTrongId &&
                    x.TuNgay == request.TuNgay &&
                    x.DenNgay == request.DenNgay))
                {
                    return Result<int>.Error(_action, $"Số liệu sinh trưởng cho cây trồng này trong khoảng thời gian này đã tồn tại");
                }

                int total = await _context.SoLieuSinhTruong.CountAsync();

                var obj = new SoLieuSinhTruong()
                {
                    CayTrongId = request.CayTrongId,
                    TuNgay = request.TuNgay,
                    DenNgay = request.DenNgay,
                    KeHoach = request.KeHoach,
                    DaGieoTrong = request.DaGieoTrong,
                    MoTa = request.MoTa,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    OrganId = null,
                    CreateByUserId = null,
                    LastModifiedByUserId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now,
                };
                _context.SoLieuSinhTruong.Add(obj);
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

        public async Task<Result<int>> Update(SoLieuSinhTruongUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật số liệu sinh trưởng với Id: \"{id}\"";

                var obj = await _context.SoLieuSinhTruong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy số liệu sinh trưởng cần sửa");

                _action = $"Cập nhật số liệu sinh trưởng từ ngày {obj.TuNgay:dd/MM/yyyy} đến {obj.DenNgay:dd/MM/yyyy}";

                obj.CayTrongId = request.CayTrongId;
                obj.TuNgay = request.TuNgay;
                obj.DenNgay = request.DenNgay;
                obj.KeHoach = request.KeHoach;
                obj.DaGieoTrong = request.DaGieoTrong;
                obj.OrganId = null;
                obj.MoTa = request.MoTa;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.SoLieuSinhTruong.Update(obj);
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
                _action = $"Xóa số liệu sinh trưởng với Id: \"{id}\"";

                var obj = await _context.SoLieuSinhTruong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy số liệu sinh trưởng cần xóa", id);

                _action = $"Xóa số liệu sinh trưởng từ ngày {obj.TuNgay:dd/MM/yyyy} đến {obj.DenNgay:dd/MM/yyyy}";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SoLieuSinhTruong.Update(obj);

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
                _action = $"Cập nhật vị trí hiển thị số liệu sinh trưởng với Id: \"{id}\"";

                var obj = await _context.SoLieuSinhTruong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy số liệu sinh trưởng cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị số liệu sinh trưởng từ ngày {obj.TuNgay:dd/MM/yyyy} đến {obj.DenNgay:dd/MM/yyyy}";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SoLieuSinhTruong.Update(obj);

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
                _action = $"Cập nhật trạng thái áp dụng số liệu sinh trưởng với Id: \"{id}\"";

                var obj = await _context.SoLieuSinhTruong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy số liệu sinh trưởng cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng số liệu sinh trưởng từ ngày {obj.TuNgay:dd/MM/yyyy} đến {obj.DenNgay:dd/MM/yyyy}";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SoLieuSinhTruong.Update(obj);

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