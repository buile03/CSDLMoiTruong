using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Common.Enums;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Data.EF;
using TechLife.CSDLMoiTruong.Data.Entities;
using TechLife.CSDLMoiTruong.Model.TinhHinhGayHaiCayTrong;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface ITinhHinhGayHaiCayTrongService
    {
        Task<PagedResult<TinhHinhGayHaiCayTrongVm>> GetPagings(TinhHinhGayHaiCayTrongGetPaging request);
        Task<List<TinhHinhGayHaiCayTrongVm>> GetAll();
        Task<TinhHinhGayHaiCayTrongVm> GetById(int id);
        Task<Result<int>> Create(TinhHinhGayHaiCayTrongCreateRequest request);
        Task<Result<int>> Update(TinhHinhGayHaiCayTrongUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);

        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);

    }

    public class TinhHinhGayHaiCayTrongService : BaseService, ITinhHinhGayHaiCayTrongService
    {
        private readonly AppDbContext _context;

        public TinhHinhGayHaiCayTrongService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<TinhHinhGayHaiCayTrongVm>> GetPagings(TinhHinhGayHaiCayTrongGetPaging request)
        {
            try
            {
                var query = from t in _context.TinhHinhGayHaiCayTrong
                            where !t.IsDelete
                            && (string.IsNullOrEmpty(request.Keyword) || (t.SinhVatGayHai.Name.Contains(request.Keyword) || t.DiaBan.Name.Contains(request.Keyword)))
                            && (request.SinhVatGayHaiId == null || request.SinhVatGayHaiId == 0 || t.SinhVatGayHaiId == request.SinhVatGayHaiId)
                            && (request.DiaBanAnhHuongId == null || request.DiaBanAnhHuongId == 0 || t.DiaBanId == request.DiaBanAnhHuongId)
                            && (request.MucDoNhiem == null || request.MucDoNhiem == 0 || (int)t.MucDoNhiem == request.MucDoNhiem)
                            orderby t.TuNgay descending
                            select new TinhHinhGayHaiCayTrongVm()
                            {
                                Id = t.Id,
                                TuNgay = t.TuNgay,
                                DenNgay = t.DenNgay,
                                SinhVatGayHaiId = t.SinhVatGayHaiId,
                                SinhVatGayHaiName = t.SinhVatGayHai.Name,
                                DiaBanId = t.DiaBanId,
                                DiaBanName = t.DiaBan.Name,
                                MucDoNhiem = EnumHelper.GetEnumValue<MucDoNhiem>((int)t.MucDoNhiem),
                                DienTichNhiem = t.DienTichNhiem
                            };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync();

                return new PagedResult<TinhHinhGayHaiCayTrongVm>()
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

        public async Task<List<TinhHinhGayHaiCayTrongVm>> GetAll()
        {
            try
            {
                var query = from t in _context.TinhHinhGayHaiCayTrong
                            where !t.IsDelete
                            orderby t.TuNgay descending
                            select new TinhHinhGayHaiCayTrongVm
                            {
                                Id = t.Id,
                                TuNgay = t.TuNgay,
                                DenNgay = t.DenNgay,
                                SinhVatGayHaiId = t.SinhVatGayHaiId,
                                SinhVatGayHaiName = t.SinhVatGayHai.Name,
                                DiaBanId = t.DiaBanId,
                                DiaBanName = t.DiaBan.Name,
                                DienTichNhiem = t.DienTichNhiem
                            };

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<TinhHinhGayHaiCayTrongVm> GetById(int id)
        {
            try
            {
                var obj = await _context.TinhHinhGayHaiCayTrong
                    .Include(x => x.SinhVatGayHai)
                    .Include(x => x.DiaBan)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (obj == null) return null;

                return new TinhHinhGayHaiCayTrongVm()
                {
                    Id = obj.Id,
                    TuNgay = obj.TuNgay,
                    DenNgay = obj.DenNgay,
                    SinhVatGayHaiId = obj.SinhVatGayHaiId,
                    SinhVatGayHaiName = obj.SinhVatGayHai?.Name,
                    DiaBanId = obj.DiaBanId,
                    DiaBanName = obj.DiaBan?.Name,
                    MucDoNhiem = EnumHelper.GetEnumValue<MucDoNhiem>((int)obj.MucDoNhiem),
                    DienTichNhiem = obj.DienTichNhiem
                };
            }
            catch { throw; }
        }

        public async Task<Result<int>> Create(TinhHinhGayHaiCayTrongCreateRequest request)
        {
            try
            {
                _action = $"Thêm tình hình gây hại cây trồng";

                if (request.DenNgay <= request.TuNgay)
                {
                    return Result<int>.Error(_action, "Ngày kết thúc phải lớn hơn ngày bắt đầu");
                }

                if ((request.DenNgay - request.TuNgay).TotalDays != 7)
                {
                    return Result<int>.Error(_action, "Khoảng cách giữa từ ngày và đến ngày phải là 1 tuần");
                }

                var obj = new TinhHinhGayHaiCayTrong()
                {
                    TuNgay = request.TuNgay,
                    DenNgay = request.DenNgay,
                    SinhVatGayHaiId = request.SinhVatGayHaiId,
                    DiaBanId = request.DiaBanId,
                    MucDoNhiem = EnumHelper.GetEnumValue<MucDoNhiem>((int)request.MucDoNhiem),
                    DienTichNhiem = request.DienTichNhiem,
                    IsDelete = false,
                    CreateByUserId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedByUserId = null,
                    OrganId = null,
                    LastModifiedOnDate = DateTime.Now,
                };

                await _context.TinhHinhGayHaiCayTrong.AddAsync(obj);
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
        
        public async Task<Result<int>> Update(TinhHinhGayHaiCayTrongUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật tình hình gây hại cây trồng với Id: \"{id}\"";

                var obj = await _context.TinhHinhGayHaiCayTrong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy bản ghi cần sửa");

                // Kiểm tra khoảng thời gian
                if (request.DenNgay <= request.TuNgay)
                {
                    return Result<int>.Error(_action, "Ngày kết thúc phải lớn hơn ngày bắt đầu");
                }

                // Kiểm tra khoảng cách 1 tuần
                if ((request.DenNgay - request.TuNgay).TotalDays != 7)
                {
                    return Result<int>.Error(_action, "Khoảng cách giữa từ ngày và đến ngày phải là 1 tuần");
                }

                obj.TuNgay = request.TuNgay;
                obj.DenNgay = request.DenNgay;
                obj.SinhVatGayHaiId = request.SinhVatGayHaiId;
                obj.DiaBanId = request.DiaBanId;
                obj.MucDoNhiem = EnumHelper.GetEnumValue<MucDoNhiem>((int)request.MucDoNhiem);
                obj.DienTichNhiem = request.DienTichNhiem;
                obj.LastModifiedByUserId = null;
                obj.OrganId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.TinhHinhGayHaiCayTrong.Update(obj);
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
                _action = $"Xóa tình hình gây hại cây trồng với Id: \"{id}\"";

                var obj = await _context.TinhHinhGayHaiCayTrong.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy bản ghi cần xóa", id);

                obj.IsDelete = true;
                obj.LastModifiedByUserId = request.UserId;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.TinhHinhGayHaiCayTrong.Update(obj);

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

                _action = $"Cập nhật vị trí hiển thị tình hình gây hại cây trồng với Id: \"{id}\"";

                var obj = await _context.TinhHinhGayHaiCayTrong.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy thông tin cần cập nhật");


                obj.Order = request.Value;
                obj.LastModifiedByUserId = request.UserId;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.TinhHinhGayHaiCayTrong.Update(obj);

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

                _action = $"Cập nhật trạng thái áp dụng thông tin tình hình gây hại cây trồng với Id: \"{id}\"";

                var obj = await _context.TinhHinhGayHaiCayTrong.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy thông tin cần cập nhật");


                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = request.UserId;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.TinhHinhGayHaiCayTrong.Update(obj);

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