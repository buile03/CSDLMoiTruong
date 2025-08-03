using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Data.EF;
using TechLife.CSDLMoiTruong.Data.Entities;
using TechLife.CSDLMoiTruong.Model.SanPhamCongBo;

namespace TechLife.CSDLMoiTruong.Service
{
    public interface ISanPhamCongBoService
    {
        Task<PagedResult<SanPhamCongBoVm>> GetPagings(SanPhamCongBoGetPagingRequest request);
        Task<List<SanPhamCongBoVm>> GetAll();
        Task<SanPhamCongBoVm> GetById(int id);
        Task<Result<int>> Create(SanPhamCongBoCreateRequest request);
        Task<Result<int>> Update(SanPhamCongBoUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateStatusRequest request);
        Task<Result<int>> UpdateOrder(UpdateOrderRequest request);
        Task<Result<int>> ImportExcel(ImportExcelRequest request);
        Task<FileResult> ExportExcel(ExportExcelRequest request);
    }

    public class SanPhamCongBoService : BaseService, ISanPhamCongBoService
    {
        private readonly AppDbContext _context;
        public SanPhamCongBoService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<SanPhamCongBoVm>> GetPagings(SanPhamCongBoGetPagingRequest request)
        {
            try
            {
                var query = from sp in _context.SanPhamCongBo
                            join dv in _context.DonViCongBo on sp.DonViCongBoId equals dv.Id
                            where !sp.IsDelete
                            && (request.DonViCongBoId == null || sp.DonViCongBoId == request.DonViCongBoId)
                            && (string.IsNullOrEmpty(request.Keyword) ||
                               (sp.Description.Contains(request.Keyword) ||
                                sp.Name.Contains(request.Keyword) ||
                                sp.Code.Contains(request.Keyword) ||
                                sp.SoCongBo.Contains(request.Keyword)))
                            select new SanPhamCongBoVm()
                            {
                                Id = sp.Id,
                                Name = sp.Name,
                                Code = sp.Code,
                                Description = sp.Description,
                                DonViCongBoId = sp.DonViCongBoId,
                                DonViCongBoName = dv.Name,
                                SoCongBo = sp.SoCongBo,
                                NgayCongBo = sp.NgayCongBo,
                                Order = sp.Order,
                                IsStatus = sp.IsStatus,
                            };

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync();

                return new PagedResult<SanPhamCongBoVm>()
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

        public async Task<List<SanPhamCongBoVm>> GetAll()
        {
            try
            {
                var query = from sp in _context.SanPhamCongBo
                            where !sp.IsDelete && sp.IsStatus
                            select new SanPhamCongBoVm
                            {
                                Id = sp.Id,
                                Name = sp.Name,
                                Code = sp.Code,
                                Description = sp.Description,
                                DonViCongBoId = sp.DonViCongBoId,
                                SoCongBo = sp.SoCongBo,
                                NgayCongBo = sp.NgayCongBo,
                                Order = sp.Order,
                                IsStatus = sp.IsStatus,
                            };

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<SanPhamCongBoVm> GetById(int id)
        {
            try
            {
                var query = from sp in _context.SanPhamCongBo
                            join dv in _context.DonViCongBo on sp.DonViCongBoId equals dv.Id
                            where !sp.IsDelete && sp.Id == id
                            select new SanPhamCongBoVm
                            {
                                Id = sp.Id,
                                Name = sp.Name,
                                Code = sp.Code,
                                Description = sp.Description,
                                DonViCongBoId = sp.DonViCongBoId,
                                DonViCongBoName = dv.Name,
                                SoCongBo = sp.SoCongBo,
                                NgayCongBo = sp.NgayCongBo,
                                Order = sp.Order,
                                IsStatus = sp.IsStatus,
                            };

                return await query.FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Create(SanPhamCongBoCreateRequest request)
        {
            try
            {
                _action = $"Thêm sản phẩm công bố \"{request.Name}\"";

                if (await _context.SanPhamCongBo.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error(_action, $"Sản phẩm \"{request.Name}\" đã tồn tại");

                int total = await _context.SanPhamCongBo.CountAsync();

                var obj = new SanPhamCongBo()
                {
                    Name = request.Name,
                    Code = request.Code,
                    DonViCongBoId = request.DonViCongBoId,
                    SoCongBo = request.SoCongBo,
                    NgayCongBo = request.NgayCongBo,
                    Description = request.Description,
                    Order = total + 1,
                    IsStatus = true,
                    IsDelete = false,
                    CreateByUserId = null,
                    LastModifiedByUserId = null,
                    OrganId = null,
                    CreateOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now,
                };
                _context.SanPhamCongBo.Add(obj);
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

        public async Task<Result<int>> Update(SanPhamCongBoUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                _action = $"Cập nhật thông tin sản phẩm công bố với Id: \"{id}\"";

                var obj = await _context.SanPhamCongBo.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy sản phẩm cần sửa");

                _action = $"Cập nhật thông tin sản phẩm \"{obj.Name}\"";

                obj.Name = request.Name.TrimSpace();
                obj.Code = request.Code;
                obj.DonViCongBoId = request.DonViCongBoId;
                obj.SoCongBo = request.SoCongBo;
                obj.NgayCongBo = request.NgayCongBo;
                obj.OrganId = null;
                obj.Description = request.Description.TrimSpace();
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;

                _context.SanPhamCongBo.Update(obj);
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
                _action = $"Xóa sản phẩm công bố với Id: \"{id}\"";

                var obj = await _context.SanPhamCongBo.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy sản phẩm cần xóa", id);

                _action = $"Xóa sản phẩm \"{obj.Name}\"";

                obj.IsDelete = true;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SanPhamCongBo.Update(obj);

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
                _action = $"Cập nhật vị trí hiển thị sản phẩm công bố với Id: \"{id}\"";

                var obj = await _context.SanPhamCongBo.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy sản phẩm cần cập nhật");

                _action = $"Cập nhật vị trí hiển thị sản phẩm \"{obj.Name}\"";

                obj.Order = request.Value;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SanPhamCongBo.Update(obj);

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
                _action = $"Cập nhật trạng thái áp dụng sản phẩm công bố với Id: \"{id}\"";

                var obj = await _context.SanPhamCongBo.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error(_action, "Không tìm thấy sản phẩm cần cập nhật");

                _action = $"Cập nhật trạng thái áp dụng sản phẩm \"{obj.Name}\"";

                obj.IsStatus = !obj.IsStatus;
                obj.LastModifiedByUserId = null;
                obj.LastModifiedOnDate = DateTime.Now;
                _context.SanPhamCongBo.Update(obj);

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
                _action = "Import danh sách sản phẩm từ Excel";

                if (request.File == null || request.File.Length == 0)
                    return Result<int>.Error(_action, "Vui lòng chọn file Excel");

                var listSanPham = new List<SanPhamCongBo>();

                using (var stream = new MemoryStream())
                {
                    await request.File.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // Bỏ qua header
                        {
                            try
                            {
                                var sanPham = new SanPhamCongBo
                                {
                                    Name = worksheet.Cells[row, 1].Value?.ToString().Trim(),
                                    Code = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                                    DonViCongBoId = request.DonViCongBoId,
                                    SoCongBo = worksheet.Cells[row, 3].Value?.ToString().Trim(),
                                    NgayCongBo = DateTime.Parse(worksheet.Cells[row, 4].Value?.ToString()),
                                    Description = worksheet.Cells[row, 5].Value?.ToString().Trim(),
                                    Order = 1,
                                    IsStatus = true,
                                    IsDelete = false,
                                    CreateOnDate = DateTime.Now,
                                    LastModifiedOnDate = DateTime.Now
                                };

                                listSanPham.Add(sanPham);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                }

                if (listSanPham.Count == 0)
                    return Result<int>.Error(_action, "Không có dữ liệu hợp lệ để import");

                await _context.SanPhamCongBo.AddRangeAsync(listSanPham);
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
                var query = from sp in _context.SanPhamCongBo
                            join dv in _context.DonViCongBo on sp.DonViCongBoId equals dv.Id
                            where !sp.IsDelete
                            && (request.DonViCongBoId == null || sp.DonViCongBoId == request.DonViCongBoId)
                            && (request.Ids == null || request.Ids.Contains(sp.Id))
                            && (string.IsNullOrEmpty(request.Keyword) ||
                               (sp.Description.Contains(request.Keyword) ||
                                sp.Name.Contains(request.Keyword) ||
                                sp.Code.Contains(request.Keyword) ||
                                sp.SoCongBo.Contains(request.Keyword)))
                            select new SanPhamCongBoVm()
                            {
                                Id = sp.Id,
                                Name = sp.Name,
                                Code = sp.Code,
                                Description = sp.Description,
                                DonViCongBoId = sp.DonViCongBoId,
                                DonViCongBoName = dv.Name,
                                SoCongBo = sp.SoCongBo,
                                NgayCongBo = sp.NgayCongBo,
                                Order = sp.Order,
                                IsStatus = sp.IsStatus,
                            };

                var data = await query.ToListAsync();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SanPhamCongBo");

                    worksheet.Cell(1, 1).Value = "Tên sản phẩm";
                    worksheet.Cell(1, 2).Value = "Mã sản phẩm";
                    worksheet.Cell(1, 3).Value = "Đơn vị công bố";
                    worksheet.Cell(1, 4).Value = "Số công bố";
                    worksheet.Cell(1, 5).Value = "Ngày công bố";
                    worksheet.Cell(1, 6).Value = "Mô tả";

                    var headerRange = worksheet.Range(1, 1, 1, 11);
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Font.Bold = true;

                    for (int i = 0; i < data.Count; i++)
                    {
                        var row = i + 2;
                        worksheet.Cell(row, 1).Value = data[i].Name;
                        worksheet.Cell(row, 2).Value = data[i].Code;
                        worksheet.Cell(row, 3).Value = data[i].DonViCongBoName;
                        worksheet.Cell(row, 4).Value = data[i].SoCongBo;
                        worksheet.Cell(row, 5).Value = data[i].NgayCongBo.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 6).Value = data[i].Description;
                    }

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return new FileContentResult(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            FileDownloadName = $"SanPhamCongBo_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
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