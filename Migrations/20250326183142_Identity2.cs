using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQuickFinal.Migrations
{
    /// <inheritdoc />
    public partial class Identity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Memberships",
                newName: "MembershipPrice");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Memberships",
                newName: "MembershipName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Memberships",
                newName: "MembershipDescription");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Memberships",
                newName: "MembershipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MembershipPrice",
                table: "Memberships",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "MembershipName",
                table: "Memberships",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "MembershipDescription",
                table: "Memberships",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "MembershipId",
                table: "Memberships",
                newName: "Id");
        }
    }
}
