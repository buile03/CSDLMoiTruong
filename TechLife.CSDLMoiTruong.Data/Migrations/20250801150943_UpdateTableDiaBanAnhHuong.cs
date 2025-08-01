using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechLife.CSDLMoiTruong.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableDiaBanAnhHuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "DiaBanAnhHuong",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanAnhHuong_ParentId",
                table: "DiaBanAnhHuong",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiaBanAnhHuong_DiaBanAnhHuong_ParentId",
                table: "DiaBanAnhHuong",
                column: "ParentId",
                principalTable: "DiaBanAnhHuong",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiaBanAnhHuong_DiaBanAnhHuong_ParentId",
                table: "DiaBanAnhHuong");

            migrationBuilder.DropIndex(
                name: "IX_DiaBanAnhHuong_ParentId",
                table: "DiaBanAnhHuong");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "DiaBanAnhHuong");
        }
    }
}
