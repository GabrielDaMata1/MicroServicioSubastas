using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialSubasta_Subasta_SubastaId",
                table: "HistorialSubasta");

            migrationBuilder.DropIndex(
                name: "IX_HistorialSubasta_SubastaId",
                table: "HistorialSubasta");

            migrationBuilder.DropColumn(
                name: "SubastaId",
                table: "HistorialSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialSubasta_IdSubasta",
                table: "HistorialSubasta",
                column: "IdSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialSubasta_Subasta_IdSubasta",
                table: "HistorialSubasta",
                column: "IdSubasta",
                principalTable: "Subasta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialSubasta_Subasta_IdSubasta",
                table: "HistorialSubasta");

            migrationBuilder.DropIndex(
                name: "IX_HistorialSubasta_IdSubasta",
                table: "HistorialSubasta");

            migrationBuilder.AddColumn<Guid>(
                name: "SubastaId",
                table: "HistorialSubasta",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HistorialSubasta_SubastaId",
                table: "HistorialSubasta",
                column: "SubastaId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialSubasta_Subasta_SubastaId",
                table: "HistorialSubasta",
                column: "SubastaId",
                principalTable: "Subasta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
