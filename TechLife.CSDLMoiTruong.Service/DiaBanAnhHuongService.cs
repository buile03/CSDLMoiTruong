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
        Task<PagedResult<DiaBanAnhHuongVm>> GetPagings(DiaBanAnhHuongGetPagingRequest request);
        Task<List<DiaBanAnhHuongVm>> GetAll(int? parentId = null);
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
        private List<DiaBanAnhHuongVm> _lstDiaBan;

        public DiaBanAnhHuongService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<DiaBanAnhHuongVm> GetDiaBan(List<DiaBanAnhHuongVm> data, int? parentId = null, int level = 0)
        {
            var lst = data.Where(v => v.ParentId == parentId);
            foreach (var d in lst)
            {
                string space = "";
                for (int i = 0; i < level; i++)
                {
                    space += "- ";
                }

                _lstDiaBan.Add(new DiaBanAnhHuongVm()
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = space + d.Name,
                    Description = d.Description,
                    ParentName = d.ParentName,
                    ParentId = d.ParentId,
                    Order = d.Order,
                    IsStatus = d.IsStatus,
                });

                if (data.Where(v => v.ParentId == d.Id).Any())
                {
                    int level_next = level + 1;
                    GetDiaBan(data, d.Id, level_next);
                }
            }
            return _lstDiaBan;
        }

        public async Task<PagedResult<DiaBanAnhHuongVm>> GetPagings(DiaBanAnhHuongGetPagingRequest request)
        {
            try
            {
                var query = from d in _context.DiaBanAnhHuong
                            where !d.IsDelete
                            && (request.ParentId == null || d.ParentId == request.ParentId)
                            && (string.IsNullOrWhiteSpace(request.Keyword) ||
                               (d.Description.Contains(request.Keyword) || d.Name.Contains(request.Keyword)))
                            orderby d.Order
                            select new DiaBanAnhHuongVm()
                            {
                                Id = d.Id,
                                Name = d.Name,
                                Code = d.Code,
                                Description = d.Description,
                                ParentId = d.ParentId,
                                ParentName = d.Parent.Name,
                                Order = d.Order,
                                IsStatus = d.IsStatus,
                            };

                var data = await query.ToListAsync();
                _lstDiaBan = new List<DiaBanAnhHuongVm>();
                _lstDiaBan = GetDiaBan(data, request.ParentId, 0);

                int totalRow = await query.CountAsync();

                data = _lstDiaBan.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList();

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

        public async Task<List<DiaBanAnhHuongVm>> GetAll(int? parentId = null)
        {
            try
            {
                var query = from d in _context.DiaBanAnhHuong
                            where !d.IsDelete && d.IsStatus
                            select new DiaBanAnhHuongVm
                            {
                                Id = d.Id,
                                Name = d.Name,
                                Code = d.Code,
                                Description = d.Description,
                                ParentId = d.ParentId,
                                ParentName = d.Parent.Name,
                                Order = d.Order,
                                IsStatus = d.IsStatus,
                            };

                var data = await query.ToListAsync();
                _lstDiaBan = new List<DiaBanAnhHuongVm>();
                _lstDiaBan = GetDiaBan(data, parentId, 0);

                return _lstDiaBan;
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
                var obj = await _context.DiaBanAnhHuong
                    .Include(x => x.Parent)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (obj == null) return null;

                return new DiaBanAnhHuongVm()
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Code = obj.Code,
                    Description = obj.Description,
                    ParentId = obj.ParentId,
                    ParentName = obj.Parent?.Name,
                    Order = obj.Order,
                    IsStatus = obj.IsStatus,
                };
            }
            catch { throw; }
        }

        public async Task<Result<int>> Create(DiaBanAnhHuongCreateRequest request)
        {
            try
            {
                _action = $"Thêm địa bàn \"{request.Name}\"";

                if (await _context.DiaBanAnhHuong.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error(_action, $"Địa bàn \"{request.Name}\" đã tồn tại");

                int total = await _context.DiaBanAnhHuong
                    .Where(x => x.ParentId == request.ParentId && !x.IsDelete)
                    .CountAsync();

                var obj = new DiaBanAnhHuong()
                {
                    Name = request.Name.TrimSpace(),
                    Code = request.Code.TrimSpace(),
                    Description = request.Description?.TrimSpace(),
                    ParentId = request.ParentId,
                    Order = total + 1,
                    IsDelete = false,
                    IsStatus = true,
                    CreateByUserId = null,
                    OrganId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedByUserId = null,
                    LastModifiedOnDate = DateTime.Now,
                };

                await _context.DiaBanAnhHuong.AddAsync(obj);
                var result = await base.SaveChange();
                if (result > 0)
                    return Result<int>.Success(_action, obj.Id);

                return Result<int>.Error(_action, obj.Id);
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
                _action = $"Cập nhật thông tin địa bàn với Id: \"{id}\"";

                var obj = await _context.DiaBanAnhHuong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy địa bàn cần sửa");

                _action = $"Cập nhật thông tin địa bàn \"{obj.Name}\"";

                obj.Name = request.Name.TrimSpace();
                obj.Code = request.Code.TrimSpace();
                obj.Description = request.Description?.TrimSpace();
                obj.ParentId = request.ParentId;
                obj.OrganId = null;
                obj.LastModifiedByUserId = request.UserId;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.DiaBanAnhHuong.Update(obj);

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
                _action = $"Xóa địa bàn với Id: \"{id}\"";

                var obj = await _context.DiaBanAnhHuong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy địa bàn cần xóa", id);

                _action = $"Xóa địa bàn \"{obj.Name}\"";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = request.UserId;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.DiaBanAnhHuong.Update(obj);

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
                _action = $"Cập nhật trạng thái địa bàn với Id: \"{id}\"";

                var obj = await _context.DiaBanAnhHuong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy địa bàn cần cập nhật");

                _action = $"Cập nhật trạng thái địa bàn \"{obj.Name}\"";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = request.UserId;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.DiaBanAnhHuong.Update(obj);

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
                _action = $"Cập nhật vị trí hiển thị địa bàn với Id: \"{id}\"";
                
                var obj = await _context.DiaBanAnhHuong.FindAsync(id);
                if (obj == null) 
                    return Result<int>.Error(_action, "Không tìm thấy địa bàn cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị địa bàn \"{obj.Name}\"";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = request.UserId;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.DiaBanAnhHuong.Update(obj);

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