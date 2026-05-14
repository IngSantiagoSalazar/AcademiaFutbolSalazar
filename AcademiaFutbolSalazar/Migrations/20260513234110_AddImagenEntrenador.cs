using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademiaFutbolSalazar.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenEntrenador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Estudiantes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Entrenadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Entrenadores");
        }
    }
}
