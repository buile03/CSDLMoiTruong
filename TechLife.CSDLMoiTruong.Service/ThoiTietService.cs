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
using TechLife.CSDLMoiTruong.Model.ThoiTiet;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface IThoiTietService
    {
        Task<PagedResult<ThoiTietVm>> GetPagings(ThoiTietGetPagingRequest request);
        Task<List<ThoiTietVm>> GetAll();
        Task<ThoiTietVm> GetById(int id);
        Task<Result<int>> Create(ThoiTietCreateRequest request);
        Task<Result<int>> Update(ThoiTietUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
    }

    public class ThoiTietService : BaseService, IThoiTietService
    {
        private readonly AppDbContext _context;
        public ThoiTietService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<ThoiTietVm>> GetPagings(ThoiTietGetPagingRequest request)
        {
            try
            {
                var query = from g in _context.ThoiTiet
                            where !g.IsDelete
                            select new ThoiTietVm()
                            {
                                Id = g.Id,
                                TuNgay = g.TuNgay,
                                DenNgay = g.DenNgay,
                                NhietDoCaoNhat = g.NhietDoCaoNhat,
                                NhietDoThapNhat = g.NhietDoThapNhat,
                                DoAmTB = g.DoAmTB,
                                NgayMua = g.NgayMua,
                                Order = g.Order,
                                IsStatus = g.IsStatus,
                            };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                return new PagedResult<ThoiTietVm>()
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

        public async Task<List<ThoiTietVm>> GetAll()
        {
            try
            {
                var query = from g in _context.ThoiTiet
                            where !g.IsDelete && g.IsStatus
                            select new ThoiTietVm
                            {
                                Id = g.Id,
                                TuNgay = g.TuNgay,
                                DenNgay = g.DenNgay,
                                NhietDoCaoNhat = g.NhietDoCaoNhat,
                                NhietDoThapNhat = g.NhietDoThapNhat,
                                DoAmTB = g.DoAmTB,
                                NgayMua = g.NgayMua,
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

        public async Task<ThoiTietVm> GetById(int id)
        {
            try
            {
                var query = from g in _context.ThoiTiet
                            where !g.IsDelete && g.Id == id
                            select new ThoiTietVm
                            {
                                Id = g.Id,
                                TuNgay = g.TuNgay,
                                DenNgay = g.DenNgay,
                                NhietDoCaoNhat = g.NhietDoCaoNhat,
                                NhietDoThapNhat = g.NhietDoThapNhat,
                                DoAmTB = g.DoAmTB,
                                NgayMua = g.NgayMua,
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

        public async Task<Result<int>> Create(ThoiTietCreateRequest request)
        {
            try
            {
                _action = $"Thêm thông tin thời tiết từ ngày {request.TuNgay:dd/MM/yyyy} đến {request.DenNgay:dd/MM/yyyy}";

                if (await _context.ThoiTiet.AnyAsync(x => x.TuNgay == request.TuNgay && x.DenNgay == request.DenNgay))
                    return Result<int>.Error(_action, $"Thông tin thời tiết trong khoảng thời gian này đã tồn tại");

                int total = await _context.ThoiTiet.CountAsync();

                var obj = new ThoiTiet()
                {
                    TuNgay = request.TuNgay,
                    DenNgay = request.DenNgay,
                    NhietDoCaoNhat = request.NhietDoCaoNhat,
                    NhietDoThapNhat = request.NhietDoThapNhat,
                    DoAmTB = request.DoAmTB,
                    NgayMua = request.NgayMua,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    CreateByUserId = null,
                    OrganId = null,
                    LastModifiedByUserId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now,
                };
                _context.ThoiTiet.Add(obj);
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

        public async Task<Result<int>> Update(ThoiTietUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật thông tin thời tiết với Id: \"{id}\"";

                var obj = await _context.ThoiTiet.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy thông tin thời tiết cần sửa");

                _action = $"Cập nhật thông tin thời tiết từ ngày {obj.TuNgay:dd/MM/yyyy} đến {obj.DenNgay:dd/MM/yyyy}";

                obj.TuNgay = request.TuNgay;
                obj.DenNgay = request.DenNgay;
                obj.NhietDoCaoNhat = request.NhietDoCaoNhat;
                obj.NhietDoThapNhat = request.NhietDoThapNhat;
                obj.DoAmTB = request.DoAmTB;
                obj.NgayMua = request.NgayMua;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.ThoiTiet.Update(obj);
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
                _action = $"Xóa thông tin thời tiết với Id: \"{id}\"";

                var obj = await _context.ThoiTiet.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy thông tin thời tiết cần xóa", id);

                _action = $"Xóa thông tin thời tiết từ ngày {obj.TuNgay:dd/MM/yyyy} đến {obj.DenNgay:dd/MM/yyyy}";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.ThoiTiet.Update(obj);

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
                _action = $"Cập nhật vị trí hiển thị thông tin thời tiết với Id: \"{id}\"";

                var obj = await _context.ThoiTiet.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy thông tin thời tiết cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị thông tin thời tiết từ ngày {obj.TuNgay:dd/MM/yyyy} đến {obj.DenNgay:dd/MM/yyyy}";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.ThoiTiet.Update(obj);

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
                _action = $"Cập nhật trạng thái áp dụng thông tin thời tiết với Id: \"{id}\"";

                var obj = await _context.ThoiTiet.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy thông tin thời tiết cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng thông tin thời tiết từ ngày {obj.TuNgay:dd/MM/yyyy} đến {obj.DenNgay:dd/MM/yyyy}";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.ThoiTiet.Update(obj);

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