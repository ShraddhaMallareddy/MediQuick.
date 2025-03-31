using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQuickFinal.Migrations
{
    /// <inheritdoc />
    public partial class IdentityDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MediManagerId",
                table: "Medicines",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_MediManagerId",
                table: "Medicines",
                column: "MediManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_AspNetUsers_MediManagerId",
                table: "Medicines",
                column: "MediManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_AspNetUsers_MediManagerId",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_MediManagerId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "MediManagerId",
                table: "Medicines");
        }
    }
}
