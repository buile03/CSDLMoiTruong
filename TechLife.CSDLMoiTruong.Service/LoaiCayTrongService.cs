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
using TechLife.CSDLMoiTruong.Model.LoaiCayTrong;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface ILoaiCayTrongService
    {
        Task<PagedResult<LoaiCayTrongVm>> GetPagings(GetPagingRequest request);

        Task<List<LoaiCayTrongVm>> GetAll();

        Task<LoaiCayTrongVm> GetById(int id);

        Task<Result<int>> Create(LoaiCayTrongCreateRequest request);

        Task<Result<int>> Update(LoaiCayTrongUpdateRequest request);

        Task<Result<int>> Delete(DeleteRequest request);

        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);

        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
    }

    public class LoaiCayTrongService : BaseService, ILoaiCayTrongService
    {
        private readonly AppDbContext _context;
        public LoaiCayTrongService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<LoaiCayTrongVm>> GetPagings(GetPagingRequest request)
        {
            try
            {
                var query = from g in _context.LoaiCayTrong
                            where !g.IsDelete
                            && (string.IsNullOrEmpty(request.Keyword) || (g.Description.Contains(request.Keyword) || g.Name.Contains(request.Keyword)))
                            select new LoaiCayTrongVm()
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                Description = g.Description,
                                Order = g.Order,
                                IsStatus = g.IsStatus,
                            };

                //3. Paging
                int totalRow = await query.CountAsync();

                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                //4. Select and projection
                var pagedResult = new PagedResult<LoaiCayTrongVm>()
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

        public async Task<List<LoaiCayTrongVm>> GetAll()
        {
            try
            {
                var query = from g in _context.LoaiCayTrong
                            where !g.IsDelete && g.IsStatus
                            select new LoaiCayTrongVm
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

        public async Task<LoaiCayTrongVm> GetById(int id)
        {
            try
            {
                var query = from g in _context.LoaiCayTrong
                            where !g.IsDelete && g.Id == id //&& g.IsStatus
                            select new LoaiCayTrongVm
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

        public async Task<Result<int>> Create(LoaiCayTrongCreateRequest request)
        {
            try
            {
                _action = $"Thêm loại cây trồng \"{request.Name}\"";

                if (await _context.LoaiCayTrong.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error(_action, $"Loại cây trồng \"{request.Name}\" đã tồn tại");

                int total = await _context.LoaiCayTrong.CountAsync();

                var obj = new LoaiCayTrong()
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
                _context.LoaiCayTrong.Add(obj);
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

        public async Task<Result<int>> Update(LoaiCayTrongUpdateRequest request)
        {
            try
            {
                try
                {
                    int id = request.Id.DecodeId();

                    _action = $"Cập nhật thông tin loại cây trồng với Id: \"{id}\"";

                    var obj = await _context.LoaiCayTrong.FindAsync(id);

                    if (obj == null)
                        return Result<int>.Error(_action, "Không tìm thấy loại cây trồng cần sửa");

                    _action = $"Cập nhật thông tin loại cây trồng \"{obj.Name}\"";

                    obj.Name = request.Name.TrimSpace();
                    obj.Code = request.Code;
                    obj.Description = request.Description.TrimSpace();
                    obj.LastModifiedByUserId = null;
                    obj.LastModifiedOnDate = DateTime.Now;
                    _context.LoaiCayTrong.Update(obj);
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

                _action = $"Xóa loại cây trồng với Id: \"{id}\"";

                var obj = await _context.LoaiCayTrong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy loại cây trồng cần xóa", id);

                _action = $"Xóa loại cây trồng \"{obj.Name}\"";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.LoaiCayTrong.Update(obj);

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

                _action = $"Cập nhật vị trí hiển thị loại cây trồng với Id: \"{id}\"";

                var obj = await _context.LoaiCayTrong.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy loại cây trồng cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị loại cây trồng \"{obj.Name}\"";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.LoaiCayTrong.Update(obj);

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

                _action = $"Cập nhật trạng thái áp dụng loại cây trồng với Id: \"{id}\"";

                var obj = await _context.LoaiCayTrong.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy loại cây trồng cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng loại cây trồng \"{obj.Name}\"";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.LoaiCayTrong.Update(obj);

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
