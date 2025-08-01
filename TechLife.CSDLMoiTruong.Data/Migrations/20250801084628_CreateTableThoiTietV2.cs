using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechLife.CSDLMoiTruong.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableThoiTietV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiongCayTrongs_LoaiCayTrongs_LoaiCayTrongId",
                table: "GiongCayTrongs");

            migrationBuilder.DropForeignKey(
                name: "FK_SinhVatGayHais_LoaiCayTrongs_LoaiCayTrongId",
                table: "SinhVatGayHais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SinhVatGayHais",
                table: "SinhVatGayHais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiCayTrongs",
                table: "LoaiCayTrongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiongCayTrongs",
                table: "GiongCayTrongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiaBanAnhHuongs",
                table: "DiaBanAnhHuongs");

            migrationBuilder.RenameTable(
                name: "SinhVatGayHais",
                newName: "SinhVatGayHai");

            migrationBuilder.RenameTable(
                name: "LoaiCayTrongs",
                newName: "LoaiCayTrong");

            migrationBuilder.RenameTable(
                name: "GiongCayTrongs",
                newName: "GiongCayTrong");

            migrationBuilder.RenameTable(
                name: "DiaBanAnhHuongs",
                newName: "DiaBanAnhHuong");

            migrationBuilder.RenameIndex(
                name: "IX_SinhVatGayHais_LoaiCayTrongId",
                table: "SinhVatGayHai",
                newName: "IX_SinhVatGayHai_LoaiCayTrongId");

            migrationBuilder.RenameIndex(
                name: "IX_GiongCayTrongs_LoaiCayTrongId",
                table: "GiongCayTrong",
                newName: "IX_GiongCayTrong_LoaiCayTrongId");

            migrationBuilder.AlterColumn<int>(
                name: "OrganId",
                table: "SinhVatGayHai",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OrganId",
                table: "LoaiCayTrong",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OrganId",
                table: "GiongCayTrong",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OrganId",
                table: "DiaBanAnhHuong",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SinhVatGayHai",
                table: "SinhVatGayHai",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiCayTrong",
                table: "LoaiCayTrong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiongCayTrong",
                table: "GiongCayTrong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiaBanAnhHuong",
                table: "DiaBanAnhHuong",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ThoiTiet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NhietDoCaoNhat = table.Column<double>(type: "float", nullable: false),
                    NhietDoThapNhat = table.Column<double>(type: "float", nullable: false),
                    DoAmTB = table.Column<double>(type: "float", nullable: false),
                    NgayMua = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ThoiTiet", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GiongCayTrong_LoaiCayTrong_LoaiCayTrongId",
                table: "GiongCayTrong",
                column: "LoaiCayTrongId",
                principalTable: "LoaiCayTrong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SinhVatGayHai_LoaiCayTrong_LoaiCayTrongId",
                table: "SinhVatGayHai",
                column: "LoaiCayTrongId",
                principalTable: "LoaiCayTrong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiongCayTrong_LoaiCayTrong_LoaiCayTrongId",
                table: "GiongCayTrong");

            migrationBuilder.DropForeignKey(
                name: "FK_SinhVatGayHai_LoaiCayTrong_LoaiCayTrongId",
                table: "SinhVatGayHai");

            migrationBuilder.DropTable(
                name: "ThoiTiet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SinhVatGayHai",
                table: "SinhVatGayHai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiCayTrong",
                table: "LoaiCayTrong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiongCayTrong",
                table: "GiongCayTrong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiaBanAnhHuong",
                table: "DiaBanAnhHuong");

            migrationBuilder.RenameTable(
                name: "SinhVatGayHai",
                newName: "SinhVatGayHais");

            migrationBuilder.RenameTable(
                name: "LoaiCayTrong",
                newName: "LoaiCayTrongs");

            migrationBuilder.RenameTable(
                name: "GiongCayTrong",
                newName: "GiongCayTrongs");

            migrationBuilder.RenameTable(
                name: "DiaBanAnhHuong",
                newName: "DiaBanAnhHuongs");

            migrationBuilder.RenameIndex(
                name: "IX_SinhVatGayHai_LoaiCayTrongId",
                table: "SinhVatGayHais",
                newName: "IX_SinhVatGayHais_LoaiCayTrongId");

            migrationBuilder.RenameIndex(
                name: "IX_GiongCayTrong_LoaiCayTrongId",
                table: "GiongCayTrongs",
                newName: "IX_GiongCayTrongs_LoaiCayTrongId");

            migrationBuilder.AlterColumn<int>(
                name: "OrganId",
                table: "SinhVatGayHais",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrganId",
                table: "LoaiCayTrongs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrganId",
                table: "GiongCayTrongs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrganId",
                table: "DiaBanAnhHuongs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SinhVatGayHais",
                table: "SinhVatGayHais",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiCayTrongs",
                table: "LoaiCayTrongs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiongCayTrongs",
                table: "GiongCayTrongs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiaBanAnhHuongs",
                table: "DiaBanAnhHuongs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GiongCayTrongs_LoaiCayTrongs_LoaiCayTrongId",
                table: "GiongCayTrongs",
                column: "LoaiCayTrongId",
                principalTable: "LoaiCayTrongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SinhVatGayHais_LoaiCayTrongs_LoaiCayTrongId",
                table: "SinhVatGayHais",
                column: "LoaiCayTrongId",
                principalTable: "LoaiCayTrongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
