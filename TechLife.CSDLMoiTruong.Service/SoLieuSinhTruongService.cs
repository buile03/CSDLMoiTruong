using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
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
        Task<Result<int>> ImportExcel(ImportExcelRequest request);
        Task<FileResult> ExportExcel(ExportExcelRequest request);
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
                            join c in _context.LoaiCayTrong on s.CayTrongId equals c.Id into ctGroup
                            from c in ctGroup.DefaultIfEmpty()
                            where !s.IsDelete
                            && (request.CayTrongId == null || request.CayTrongId == 0 || s.CayTrongId == request.CayTrongId)
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
        public async Task<Result<int>> ImportExcel(ImportExcelRequest request)
        {
            try
            {
                _action = "Import danh sách số liệu sinh trưởng từ Excel";

                if (request.File == null || request.File.Length == 0)
                    return Result<int>.Error(_action, "Vui lòng chọn file Excel");

                var listSoLieu = new List<SoLieuSinhTruong>();

                using (var stream = new MemoryStream())
                {
                    await request.File.CopyToAsync(stream);

                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rowCount = worksheet.RowsUsed().Count();

                        if (worksheet.Cell(1, 1).GetString() != "Loại cây trồng" ||
                            worksheet.Cell(1, 2).GetString() != "Từ ngày" ||
                            worksheet.Cell(1, 3).GetString() != "Đến ngày")
                        {
                            return Result<int>.Error(_action, "File Excel không đúng định dạng. Vui lòng tải file mẫu và làm theo hướng dẫn.");
                        }

                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                var tenLoaiCay = worksheet.Cell(row, 1).GetString().Trim();
                                if (string.IsNullOrEmpty(tenLoaiCay))
                                    continue;

                                var loaiCayTrong = await _context.LoaiCayTrong
                                    .FirstOrDefaultAsync(x => x.Name.ToLower() == tenLoaiCay.ToLower());

                                if (loaiCayTrong == null)
                                {
                                    loaiCayTrong = new LoaiCayTrong
                                    {
                                        Name = tenLoaiCay,
                                        Code = GenerateCodeFromName(tenLoaiCay),
                                        Description = $"Tự động tạo khi import số liệu sinh trưởng ngày {DateTime.Now:dd/MM/yyyy}",
                                        IsStatus = true,
                                        IsDelete = false,
                                        CreateOnDate = DateTime.Now,
                                        LastModifiedOnDate = DateTime.Now
                                    };
                                    _context.LoaiCayTrong.Add(loaiCayTrong);
                                    await _context.SaveChangesAsync();
                                }

                                var soLieu = new SoLieuSinhTruong
                                {
                                    CayTrongId = loaiCayTrong.Id,
                                    TuNgay = DateTime.Parse(worksheet.Cell(row, 2).GetString().Trim()),
                                    DenNgay = DateTime.Parse(worksheet.Cell(row, 3).GetString().Trim()),
                                    KeHoach = worksheet.Cell(row, 4).GetValue<double>(),
                                    DaGieoTrong = worksheet.Cell(row, 5).GetValue<double>(),
                                    MoTa = worksheet.Cell(row, 6).GetString().Trim() ?? "",
                                    Order = 1,
                                    IsStatus = true,
                                    IsDelete = false,
                                    CreateOnDate = DateTime.Now,
                                    LastModifiedOnDate = DateTime.Now
                                };

                                listSoLieu.Add(soLieu);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                }

                if (listSoLieu.Count == 0)
                    return Result<int>.Error(_action, "Không có dữ liệu hợp lệ để import");

                await _context.SoLieuSinhTruong.AddRangeAsync(listSoLieu);
                var result = await _context.SaveChangesAsync();

                return Result<int>.Success(_action, result);
            }
            catch (Exception ex)
            {
                return Result<int>.Error(_action, ex.Message);
            }
        }

        private string GenerateCodeFromName(string name)
        {
            return name.ToUpper().Replace(" ", "");
        }
        public async Task<FileResult> ExportExcel(ExportExcelRequest request)
        {
            try
            {
                var query = from s in _context.SoLieuSinhTruong
                            join c in _context.LoaiCayTrong on s.CayTrongId equals c.Id
                            where !s.IsDelete
                            && (request.CayTrongId == null || request.CayTrongId == 0 || s.CayTrongId == request.CayTrongId)
                            && (request.Ids == null || request.Ids.Contains(s.Id))
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

                var data = await query.ToListAsync();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SoLieuSinhTruong");

                    // Add headers
                    worksheet.Cell(1, 1).Value = "Loại cây trồng";
                    worksheet.Cell(1, 2).Value = "Từ ngày";
                    worksheet.Cell(1, 3).Value = "Đến ngày";
                    worksheet.Cell(1, 4).Value = "Kế hoạch";
                    worksheet.Cell(1, 5).Value = "Đã gieo trồng";
                    worksheet.Cell(1, 6).Value = "Mô tả";

                    var headerRange = worksheet.Range(1, 1, 1, 6);
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Font.Bold = true;

                    // Add data
                    for (int i = 0; i < data.Count; i++)
                    {
                        var row = i + 2;
                        worksheet.Cell(row, 1).Value = data[i].TenCayTrong;
                        worksheet.Cell(row, 2).Value = data[i].TuNgay.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 3).Value = data[i].DenNgay.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 4).Value = data[i].KeHoach;
                        worksheet.Cell(row, 5).Value = data[i].DaGieoTrong;
                        worksheet.Cell(row, 6).Value = data[i].MoTa;
                    }

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return new FileContentResult(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            FileDownloadName = $"SoLieuSinhTruong_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}