using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechLife.CSDLMoiTruong.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableTinhHinhGayHaiCayTrong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TinhHinhGayHaiCayTrong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SinhVatGayHaiId = table.Column<int>(type: "int", nullable: false),
                    DiaBanId = table.Column<int>(type: "int", nullable: false),
                    MucDoNhiem = table.Column<int>(type: "int", nullable: false),
                    DienTichNhiem = table.Column<double>(type: "float", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    OrganId = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinhHinhGayHaiCayTrong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TinhHinhGayHaiCayTrong_DiaBanAnhHuong_DiaBanId",
                        column: x => x.DiaBanId,
                        principalTable: "DiaBanAnhHuong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TinhHinhGayHaiCayTrong_SinhVatGayHai_SinhVatGayHaiId",
                        column: x => x.SinhVatGayHaiId,
                        principalTable: "SinhVatGayHai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TinhHinhGayHaiCayTrong_DiaBanId",
                table: "TinhHinhGayHaiCayTrong",
                column: "DiaBanId");

            migrationBuilder.CreateIndex(
                name: "IX_TinhHinhGayHaiCayTrong_SinhVatGayHaiId",
                table: "TinhHinhGayHaiCayTrong",
                column: "SinhVatGayHaiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TinhHinhGayHaiCayTrong");
        }
    }
}
