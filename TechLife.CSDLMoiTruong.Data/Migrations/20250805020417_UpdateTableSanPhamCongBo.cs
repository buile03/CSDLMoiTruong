using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechLife.CSDLMoiTruong.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableSanPhamCongBo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamCongBo_DonViCongBo_DonViCongBoId",
                table: "SanPhamCongBo");

            migrationBuilder.AlterColumn<int>(
                name: "DonViCongBoId",
                table: "SanPhamCongBo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamCongBo_DonViCongBo_DonViCongBoId",
                table: "SanPhamCongBo",
                column: "DonViCongBoId",
                principalTable: "DonViCongBo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamCongBo_DonViCongBo_DonViCongBoId",
                table: "SanPhamCongBo");

            migrationBuilder.AlterColumn<int>(
                name: "DonViCongBoId",
                table: "SanPhamCongBo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamCongBo_DonViCongBo_DonViCongBoId",
                table: "SanPhamCongBo",
                column: "DonViCongBoId",
                principalTable: "DonViCongBo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
