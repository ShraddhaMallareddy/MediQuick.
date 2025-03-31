using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQuickFinal.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "MediBackground",
                table: "Medicines",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "MediBanner",
                table: "Medicines",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "MediLogo",
                table: "Medicines",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediBackground",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "MediBanner",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "MediLogo",
                table: "Medicines");
        }
    }
}
