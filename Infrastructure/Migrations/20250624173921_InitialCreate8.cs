using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subasta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    idProducto = table.Column<Guid>(type: "uuid", nullable: false),
                    fechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    incrementoMinimo = table.Column<decimal>(type: "numeric", nullable: false),
                    precioReserva = table.Column<decimal>(type: "numeric", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subasta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistorialSubasta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSubasta = table.Column<Guid>(type: "uuid", nullable: false),
                    SubastaId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    MontoFinal = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialSubasta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialSubasta_Subasta_SubastaId",
                        column: x => x.SubastaId,
                        principalTable: "Subasta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialSubasta_SubastaId",
                table: "HistorialSubasta",
                column: "SubastaId");

            migrationBuilder.CreateIndex(
                name: "IX_Subasta_Id",
                table: "Subasta",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialSubasta");

            migrationBuilder.DropTable(
                name: "Subasta");
        }
    }
}
