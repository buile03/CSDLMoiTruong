using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        Task<Result<int>> ImportExcel(ImportExcelRequest request);
        Task<FileResult> ExportExcel(ExportExcelRequest request);

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

                if ((request.DenNgay - request.TuNgay).TotalDays != 6)
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

                if (request.DenNgay <= request.TuNgay)
                {
                    return Result<int>.Error(_action, "Ngày kết thúc phải lớn hơn ngày bắt đầu");
                }

                if ((request.DenNgay - request.TuNgay).TotalDays != 6)
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
        public async Task<Result<int>> ImportExcel(ImportExcelRequest request)
        {
            try
            {
                _action = "Import danh sách tình hình gây hại cây trồng từ Excel";

                if (request.File == null || request.File.Length == 0)
                    return Result<int>.Error(_action, "Vui lòng chọn file Excel");

                var listTinhHinh = new List<TinhHinhGayHaiCayTrong>();

                using var stream = new MemoryStream();
                await request.File.CopyToAsync(stream);

                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rowCount = worksheet.RowsUsed().Count();

                if (worksheet.Cell(1, 1).GetString() != "Loại cây trồng" ||
                    worksheet.Cell(1, 2).GetString() != "Sinh vật gây hại" ||
                    worksheet.Cell(1, 3).GetString() != "Địa bàn" ||
                    worksheet.Cell(1, 4).GetString() != "Từ ngày" ||
                    worksheet.Cell(1, 5).GetString() != "Đến ngày")
                {
                    return Result<int>.Error(_action, "File Excel không đúng định dạng.");
                }

                int currentLoaiCayCount = await _context.LoaiCayTrong.CountAsync();
                int currentSinhVatCount = await _context.SinhVatGayHai.CountAsync();
                int currentDiaBanCount = await _context.DiaBanAnhHuong.CountAsync();

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var tenLoaiCay = worksheet.Cell(row, 1).GetString().Trim();
                        var tenSinhVat = worksheet.Cell(row, 2).GetString().Trim();
                        var tenDiaBan = worksheet.Cell(row, 3).GetString().Trim();

                        if (string.IsNullOrEmpty(tenLoaiCay) || string.IsNullOrEmpty(tenSinhVat) || string.IsNullOrEmpty(tenDiaBan))
                            continue;

                        var loaiCay = await _context.LoaiCayTrong.FirstOrDefaultAsync(x => x.Name.ToLower() == tenLoaiCay.ToLower());
                        if (loaiCay == null)
                        {
                            currentLoaiCayCount++;
                            loaiCay = new LoaiCayTrong
                            {
                                Name = tenLoaiCay,
                                Code = "",
                                Description = $"Tự động tạo khi import {DateTime.Now:dd/MM/yyyy}",
                                Order = currentLoaiCayCount,
                                IsStatus = true,
                                IsDelete = false,
                                CreateOnDate = DateTime.Now,
                                LastModifiedOnDate = DateTime.Now
                            };
                            _context.LoaiCayTrong.Add(loaiCay);
                            await _context.SaveChangesAsync();
                        }

                        var sinhVat = await _context.SinhVatGayHai.FirstOrDefaultAsync(x => x.Name.ToLower() == tenSinhVat.ToLower());
                        if (sinhVat == null)
                        {
                            currentSinhVatCount++;
                            sinhVat = new SinhVatGayHai
                            {
                                Name = tenSinhVat,
                                Code = "",
                                LoaiCayTrongId = loaiCay.Id,
                                Description = $"Tự động tạo khi import {DateTime.Now:dd/MM/yyyy}",
                                Order = currentSinhVatCount,
                                IsStatus = true,
                                IsDelete = false,
                                CreateOnDate = DateTime.Now,
                                LastModifiedOnDate = DateTime.Now
                            };
                            _context.SinhVatGayHai.Add(sinhVat);
                            await _context.SaveChangesAsync();
                        }

                        var diaBan = await _context.DiaBanAnhHuong.FirstOrDefaultAsync(x => x.Name.ToLower() == tenDiaBan.ToLower());
                        if (diaBan == null)
                        {
                            currentDiaBanCount++;
                            diaBan = new DiaBanAnhHuong
                            {
                                Name = tenDiaBan,
                                Code = "",
                                Description = $"Tự động tạo khi import {DateTime.Now:dd/MM/yyyy}",
                                Order = currentDiaBanCount,
                                IsStatus = true,
                                IsDelete = false,
                                CreateOnDate = DateTime.Now,
                                LastModifiedOnDate = DateTime.Now
                            };
                            _context.DiaBanAnhHuong.Add(diaBan);
                            await _context.SaveChangesAsync();
                        }

                        var tuNgay = DateTime.Parse(worksheet.Cell(row, 4).GetString().Trim());
                        var denNgay = DateTime.Parse(worksheet.Cell(row, 5).GetString().Trim());

                        if ((denNgay - tuNgay).TotalDays != 6)
                            return Result<int>.Error(_action, $"Khoảng cách ngày không đúng 1 tuần tại dòng {row}");

                        var mucDo = (MucDoNhiem)worksheet.Cell(row, 6).GetValue<int>();
                        var dienTichStr = worksheet.Cell(row, 7).GetString().Trim().Replace(",", ".");
                        var dienTich = double.Parse(dienTichStr, CultureInfo.InvariantCulture);

                        var tinhHinh = new TinhHinhGayHaiCayTrong
                        {
                            SinhVatGayHaiId = sinhVat.Id,
                            DiaBanId = diaBan.Id,
                            TuNgay = tuNgay,
                            DenNgay = denNgay,
                            MucDoNhiem = mucDo,
                            DienTichNhiem = dienTich,
                            IsStatus = true,
                            IsDelete = false,
                            CreateOnDate = DateTime.Now,
                            LastModifiedOnDate = DateTime.Now
                        };

                        listTinhHinh.Add(tinhHinh);
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (listTinhHinh.Count == 0)
                    return Result<int>.Error(_action, "Không có dữ liệu hợp lệ để import");

                await _context.TinhHinhGayHaiCayTrong.AddRangeAsync(listTinhHinh);
                var result = await _context.SaveChangesAsync();

                return Result<int>.Success(_action, result);
            }
            catch (Exception ex)
            {
                return Result<int>.Error(_action, ex.Message);
            }
        }


        public async Task<FileResult> ExportExcel(ExportExcelRequest request)
        {
            try
            {
                var query = from t in _context.TinhHinhGayHaiCayTrong
                            join sv in _context.SinhVatGayHai on t.SinhVatGayHaiId equals sv.Id
                            join lct in _context.LoaiCayTrong on sv.LoaiCayTrongId equals lct.Id
                            join db in _context.DiaBanAnhHuong on t.DiaBanId equals db.Id
                            where !t.IsDelete
                            && (request.SinhVatGayHaiId == null || t.SinhVatGayHaiId == request.SinhVatGayHaiId)
                            && (request.DiaBanAnhHuongId == null || t.DiaBanId == request.DiaBanAnhHuongId)
                            && (request.MucDoNhiem == null || (int)t.MucDoNhiem == request.MucDoNhiem)
                            && (string.IsNullOrEmpty(request.Keyword) ||
                               (sv.Name.Contains(request.Keyword) || db.Name.Contains(request.Keyword)))
                            orderby t.TuNgay descending
                            select new
                            {
                                LoaiCayTrong = lct.Name,
                                SinhVatGayHai = sv.Name,
                                DiaBan = db.Name,
                                t.TuNgay,
                                t.DenNgay,
                                t.MucDoNhiem,
                                t.DienTichNhiem
                            };

                var data = await query.ToListAsync();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("TinhHinhGayHaiCayTrong");

                    // Add headers
                    worksheet.Cell(1, 1).Value = "Loại cây trồng";
                    worksheet.Cell(1, 2).Value = "Sinh vật gây hại";
                    worksheet.Cell(1, 3).Value = "Địa bàn";
                    worksheet.Cell(1, 4).Value = "Từ ngày";
                    worksheet.Cell(1, 5).Value = "Đến ngày";
                    worksheet.Cell(1, 6).Value = "Mức độ nhiễm";
                    worksheet.Cell(1, 7).Value = "Diện tích nhiễm (ha)";

                    // Style header
                    var headerRange = worksheet.Range(1, 1, 1, 7);
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Font.Bold = true;

                    // Add data
                    for (int i = 0; i < data.Count; i++)
                    {
                        var row = i + 2;
                        worksheet.Cell(row, 1).Value = data[i].LoaiCayTrong;
                        worksheet.Cell(row, 2).Value = data[i].SinhVatGayHai;
                        worksheet.Cell(row, 3).Value = data[i].DiaBan;
                        worksheet.Cell(row, 4).Value = data[i].TuNgay.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 5).Value = data[i].DenNgay.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 6).Value = data[i].MucDoNhiem.ToString();
                        worksheet.Cell(row, 7).Value = data[i].DienTichNhiem;
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return new FileContentResult(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            FileDownloadName = $"TinhHinhGayHaiCayTrong_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
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