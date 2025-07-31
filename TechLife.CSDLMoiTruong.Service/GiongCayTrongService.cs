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
using TechLife.CSDLMoiTruong.Model.GiongCayTrong;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface IGiongCayTrongService
    {
        Task<PagedResult<GiongCayTrongVm>> GetPagings(GiongCayTrongGetPagingRequest request);
        Task<List<GiongCayTrongVm>> GetAll();
        Task<GiongCayTrongVm> GetById(int id);
        Task<Result<int>> Create(GiongCayTrongCreateRequest request);
        Task<Result<int>> Update(GiongCayTrongUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
    }

    public class GiongCayTrongService : BaseService, IGiongCayTrongService
    {
        private readonly AppDbContext _context;
        private readonly ILoaiCayTrongService _loaiCayTrongService;

        public GiongCayTrongService(AppDbContext context, ILoaiCayTrongService loaiCayTrongService) : base(context)
        {
            _context = context;
            _loaiCayTrongService = loaiCayTrongService;
        }

        public async Task<PagedResult<GiongCayTrongVm>> GetPagings(GiongCayTrongGetPagingRequest request)
        {
            try
            {
                var query = from g in _context.GiongCayTrongs
                            join l in _context.LoaiCayTrongs on g.LoaiCayTrongId equals l.Id into lg
                            from l in lg.DefaultIfEmpty()
                            where !g.IsDelete
                            && (request.LoaiCayTrongId == null || g.LoaiCayTrongId == request.LoaiCayTrongId)
                            && (string.IsNullOrEmpty(request.Keyword) ||
                               (g.Name.Contains(request.Keyword) || g.Description.Contains(request.Keyword)))
                            orderby g.Order
                            select new GiongCayTrongVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                Description = g.Description,
                                Order = g.Order,
                                IsStatus = g.IsStatus,
                                LoaiCayTrongId = g.LoaiCayTrongId,
                                LoaiCayTrongName = l != null ? l.Name : ""
                            };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync();

                return new PagedResult<GiongCayTrongVm>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GiongCayTrongVm>> GetAll()
        {
            try
            {
                return await (from g in _context.GiongCayTrongs
                              where !g.IsDelete && g.IsStatus
                              orderby g.Order
                              select new GiongCayTrongVm
                              {
                                  Id = g.Id,
                                  Name = g.Name,
                                  Code = g.Code,
                                  Description = g.Description,
                                  Order = g.Order,
                                  IsStatus = g.IsStatus,
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GiongCayTrongVm> GetById(int id)
        {
            try
            {
                return await (from g in _context.GiongCayTrongs
                              where !g.IsDelete && g.Id == id
                              select new GiongCayTrongVm
                              {
                                  Id = g.Id,
                                  Name = g.Name,
                                  Code = g.Code,
                                  Description = g.Description,
                                  Order = g.Order,
                                  IsStatus = g.IsStatus,
                                  LoaiCayTrongId = g.LoaiCayTrongId,
                              }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<int>> Create(GiongCayTrongCreateRequest request)
        {
            try
            {
                _action = $"Thêm giống cây trồng \"{request.Name}\"";

                if (await _context.GiongCayTrongs.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error(_action, $"Giống cây trồng \"{request.Name}\" đã tồn tại");

                int total = await _context.GiongCayTrongs.CountAsync();

                var obj = new GiongCayTrong
                {
                    Name = request.Name.Trim(),
                    Code = request.Code,
                    Description = request.Description,
                    LoaiCayTrongId = request.LoaiCayTrongId,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    CreateByUserId = null,
                    LastModifiedByUserId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                };

                _context.GiongCayTrongs.Add(obj);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return Result<int>.Success(_action, obj.Id);

                return Result<int>.Error(_action);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<int>> Update(GiongCayTrongUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật giống cây trồng ID: \"{id}\"";

                var entity = await _context.GiongCayTrongs.FindAsync(id);
                if (entity == null)
                    return Result<int>.Error(_action, "Không tìm thấy giống cây trồng");

                _action = $"Cập nhật giống cây trồng \"{entity.Name}\"";

                entity.Name = request.Name.Trim();
                entity.Code = request.Code;
                entity.Description = request.Description;
                entity.LoaiCayTrongId = request.LoaiCayTrongId;
                entity.LastModifiedByUserId = null;
                entity.LastModifiedOnDate = DateTime.Now;

                _context.GiongCayTrongs.Update(entity);
                var result = await base.SaveChange();

                return result > 0
                    ? Result<int>.Success(_action, id)
                    : Result<int>.Error(_action, id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<int>> Delete(DeleteRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Xóa giống cây trồng ID: \"{id}\"";

                var entity = await _context.GiongCayTrongs.FindAsync(id);
                if (entity == null)
                    return Result<int>.Error(_action, "Không tìm thấy giống cây trồng", id);

                _action = $"Xóa giống cây trồng \"{entity.Name}\"";

                entity.IsDelete = true;
                entity.LastModifiedByUserId = null;
                entity.LastModifiedOnDate = DateTime.Now;

                _context.GiongCayTrongs.Update(entity);
                var result = await base.SaveChange();

                return result > 0
                    ? Result<int>.Success(_action, id)
                    : Result<int>.Error(_action, id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<int>> UpdateStatus(UpdateStatusRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật trạng thái giống cây trồng ID: \"{id}\"";

                var entity = await _context.GiongCayTrongs.FindAsync(id);
                if (entity == null)
                    return Result<int>.Error(_action, "Không tìm thấy giống cây trồng");

                _action = $"Cập nhật trạng thái \"{entity.Name}\"";

                entity.IsStatus = !entity.IsStatus;
                entity.LastModifiedByUserId = null;
                entity.LastModifiedOnDate = DateTime.Now;

                _context.GiongCayTrongs.Update(entity);
                var result = await base.SaveChange();

                return result > 0
                    ? Result<int>.Success(_action, id)
                    : Result<int>.Error(_action, id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<int>> UpdateOrder(UpdateOrderRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật thứ tự giống cây trồng ID: \"{id}\"";

                var entity = await _context.GiongCayTrongs.FindAsync(id);
                if (entity == null)
                    return Result<int>.Error(_action, "Không tìm thấy giống cây trồng");

                _action = $"Cập nhật thứ tự \"{entity.Name}\"";

                entity.Order = request.Value;
                entity.LastModifiedByUserId = null;
                entity.LastModifiedOnDate = DateTime.Now;

                _context.GiongCayTrongs.Update(entity);
                var result = await base.SaveChange();

                return result > 0
                    ? Result<int>.Success(_action, id)
                    : Result<int>.Error(_action, id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
