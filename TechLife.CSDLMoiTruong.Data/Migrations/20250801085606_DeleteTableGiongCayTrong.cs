using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechLife.CSDLMoiTruong.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTableGiongCayTrong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiongCayTrong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiongCayTrong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiCayTrongId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    OrganId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiongCayTrong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiongCayTrong_LoaiCayTrong_LoaiCayTrongId",
                        column: x => x.LoaiCayTrongId,
                        principalTable: "LoaiCayTrong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiongCayTrong_LoaiCayTrongId",
                table: "GiongCayTrong",
                column: "LoaiCayTrongId");
        }
    }
}
