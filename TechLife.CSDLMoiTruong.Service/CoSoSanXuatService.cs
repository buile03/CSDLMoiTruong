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
using TechLife.CSDLMoiTruong.Model.CoSoSanXuat;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface ICoSoSanXuatService
    {
        Task<PagedResult<CoSoSanXuatVm>> GetPagings(CoSoSanXuatGetPagingRequest request);
        Task<List<CoSoSanXuatVm>> GetAll();
        Task<CoSoSanXuatVm> GetById(int id);
        Task<Result<int>> Create(CoSoSanXuatCreateRequest request);
        Task<Result<int>> Update(CoSoSanXuatUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
        Task<Result<int>> ImportExcel(ImportExcelRequest request);
        Task<FileResult> ExportExcel(ExportExcelRequest request);
    }

    public class CoSoSanXuatService : BaseService, ICoSoSanXuatService
    {
        private readonly AppDbContext _context;
        public CoSoSanXuatService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<CoSoSanXuatVm>> GetPagings(CoSoSanXuatGetPagingRequest request)
        {
            try
            {
                var query = from g in _context.CoSoSanXuat
                            where !g.IsDelete
                            && (string.IsNullOrEmpty(request.Keyword) || g.DiaChi.Contains(request.Keyword) || g.DienThoai.Contains(request.Keyword))
                           select new CoSoSanXuatVm()
                           {
                               Id = g.Id,
                               Name = g.Name,
                               Code = g.Code,
                               DiaChi = g.DiaChi,
                               DienThoai = g.DienThoai,
                               Email = g.Email,
                               ChuCoSo = g.ChuCoSo,
                               MaSoThue = g.MaSoThue,
                               GhiChu = g.GhiChu,
                               Order = g.Order,
                               IsStatus = g.IsStatus,
                           };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                var pagedResult = new PagedResult<CoSoSanXuatVm>()
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

        public async Task<List<CoSoSanXuatVm>> GetAll()
        {
            try
            {
                var query = from g in _context.CoSoSanXuat
                            where !g.IsDelete && g.IsStatus
                            select new CoSoSanXuatVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                DiaChi = g.DiaChi,
                                DienThoai = g.DienThoai,
                                Email = g.Email,
                                ChuCoSo = g.ChuCoSo,
                                MaSoThue = g.MaSoThue,
                                GhiChu = g.GhiChu,
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

        public async Task<CoSoSanXuatVm> GetById(int id)
        {
            try
            {
                var query = from g in _context.CoSoSanXuat
                            where !g.IsDelete && g.Id == id
                            select new CoSoSanXuatVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Code = g.Code,
                                DiaChi = g.DiaChi,
                                DienThoai = g.DienThoai,
                                Email = g.Email,
                                ChuCoSo = g.ChuCoSo,
                                MaSoThue = g.MaSoThue,
                                GhiChu = g.GhiChu,
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

        public async Task<Result<int>> Create(CoSoSanXuatCreateRequest request)
        {
            try
            {
                _action = $"Thêm cơ sở sản xuất \"{request.Name}\"";

                if (await _context.CoSoSanXuat.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error(_action, $"Cơ sở sản xuất \"{request.Name}\" đã tồn tại");

                int total = await _context.CoSoSanXuat.CountAsync();

                var obj = new CoSoSanXuat()
                {
                    Name = request.Name,
                    DiaChi = request.DiaChi,
                    Code = request.Code,
                    DienThoai = request.DienThoai,
                    Email = request.Email,
                    ChuCoSo = request.ChuCoSo,
                    MaSoThue = request.MaSoThue,
                    GhiChu = request.GhiChu,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    CreateByUserId = null,
                    LastModifiedByUserId = null,
                    OrganId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now,
                };
                _context.CoSoSanXuat.Add(obj);
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

        public async Task<Result<int>> Update(CoSoSanXuatUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();

                _action = $"Cập nhật thông tin cơ sở sản xuất với Id: \"{id}\"";

                var obj = await _context.CoSoSanXuat.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy cơ sở sản xuất cần sửa");

                _action = $"Cập nhật thông tin cơ sở sản xuất \"{obj.Name}\"";

                obj.Name = request.Name.TrimSpace();
                obj.Code = request.Code;
                obj.DiaChi = request.DiaChi;
                obj.DienThoai = request.DienThoai;
                obj.Email = request.Email;
                obj.ChuCoSo = request.ChuCoSo;
                obj.MaSoThue = request.MaSoThue;
                obj.GhiChu = request.GhiChu;
                obj.LastModifiedByUserId = null;
                obj.OrganId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.CoSoSanXuat.Update(obj);
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

                _action = $"Xóa cơ sở sản xuất với Id: \"{id}\"";

                var obj = await _context.CoSoSanXuat.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy cơ sở sản xuất cần xóa", id);

                _action = $"Xóa cơ sở sản xuất \"{obj.Name}\"";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.CoSoSanXuat.Update(obj);

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

                _action = $"Cập nhật vị trí hiển thị cơ sở sản xuất với Id: \"{id}\"";

                var obj = await _context.CoSoSanXuat.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy cơ sở sản xuất cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị cơ sở sản xuất \"{obj.Name}\"";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.CoSoSanXuat.Update(obj);

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

                _action = $"Cập nhật trạng thái áp dụng cơ sở sản xuất với Id: \"{id}\"";

                var obj = await _context.CoSoSanXuat.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy cơ sở sản xuất cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng cơ sở sản xuất \"{obj.Name}\"";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.CoSoSanXuat.Update(obj);

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
                _action = "Import danh sách cơ sở sản xuất từ Excel";

                if (request.File == null || request.File.Length == 0)
                    return Result<int>.Error(_action, "Vui lòng chọn file Excel");

                var listCoSo = new List<CoSoSanXuat>();

                using (var stream = new MemoryStream())
                {
                    await request.File.CopyToAsync(stream);

                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rowCount = worksheet.RowsUsed().Count();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                int currentCoSoCount = await _context.CoSoSanXuat.CountAsync();
                                var coSo = new CoSoSanXuat
                                {
                                    Name = worksheet.Cell(row, 1).GetString().Trim(),
                                    Code = worksheet.Cell(row, 2).GetString().Trim() ?? "",
                                    DiaChi = worksheet.Cell(row, 3).GetString().Trim() ?? "",
                                    DienThoai = worksheet.Cell(row, 4).GetString().Trim() ?? "",
                                    Email = worksheet.Cell(row, 5).GetString().Trim() ?? "",
                                    ChuCoSo = worksheet.Cell(row, 6).GetString().Trim() ?? "",
                                    MaSoThue = worksheet.Cell(row, 7).GetString().Trim() ?? "",
                                    GhiChu = worksheet.Cell(row, 8).GetString().Trim() ?? "",
                                    Order = currentCoSoCount + 1,
                                    IsStatus = true,
                                    IsDelete = false,
                                    CreateOnDate = DateTime.Now,
                                    LastModifiedOnDate = DateTime.Now
                                };

                                listCoSo.Add(coSo);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                }

                if (listCoSo.Count == 0)
                    return Result<int>.Error(_action, "Không có dữ liệu hợp lệ để import");

                await _context.CoSoSanXuat.AddRangeAsync(listCoSo);
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
                var query = from cs in _context.CoSoSanXuat
                            where !cs.IsDelete
                            && (string.IsNullOrEmpty(request.Keyword) ||
                               (cs.DiaChi.Contains(request.Keyword) ||
                                cs.Name.Contains(request.Keyword) ||
                                cs.Code.Contains(request.Keyword) ||
                                cs.DienThoai.Contains(request.Keyword)))
                            select new CoSoSanXuatVm()
                    {
                        Id = cs.Id,
                        Name = cs.Name,
                        Code = cs.Code,
                        DiaChi = cs.DiaChi,
                        DienThoai = cs.DienThoai,
                        Email = cs.Email,
                        ChuCoSo = cs.ChuCoSo,
                        MaSoThue = cs.MaSoThue,
                        GhiChu = cs.GhiChu,
                        Order = cs.Order,
                        IsStatus = cs.IsStatus,
                    };

                var data = await query.ToListAsync();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("CoSoSanXuat");

                    // Header
                    worksheet.Cell(1, 1).Value = "Tên cơ sở";
                    worksheet.Cell(1, 2).Value = "Mã cơ sở";
                    worksheet.Cell(1, 3).Value = "Địa chỉ";
                    worksheet.Cell(1, 4).Value = "Điện thoại";
                    worksheet.Cell(1, 5).Value = "Email";
                    worksheet.Cell(1, 6).Value = "Chủ cơ sở";
                    worksheet.Cell(1, 7).Value = "Mã số thuế";
                    worksheet.Cell(1, 8).Value = "Ghi chú";

                    var headerRange = worksheet.Range(1, 1, 1, 8);
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Font.Bold = true;

                    // Data
                    for (int i = 0; i < data.Count; i++)
                    {
                        var row = i + 2;
                        worksheet.Cell(row, 1).Value = data[i].Name;
                        worksheet.Cell(row, 2).Value = data[i].Code;
                        worksheet.Cell(row, 3).Value = data[i].DiaChi;
                        worksheet.Cell(row, 4).Value = data[i].DienThoai;
                        worksheet.Cell(row, 5).Value = data[i].Email;
                        worksheet.Cell(row, 6).Value = data[i].ChuCoSo;
                        worksheet.Cell(row, 7).Value = data[i].MaSoThue;
                        worksheet.Cell(row, 8).Value = data[i].GhiChu;
                    }

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return new FileContentResult(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            FileDownloadName = $"CoSoSanXuat_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
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