using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQuickFinal.Migrations
{
    /// <inheritdoc />
    public partial class IdentityDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    MediId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediPrice = table.Column<int>(type: "int", nullable: false),
                    MediCategory = table.Column<int>(type: "int", nullable: false),
                    MediType = table.Column<int>(type: "int", nullable: false),
                    MediDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.MediId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medicines");
        }
    }
}
