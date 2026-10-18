using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentLoanSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Members");
        }
    }
}
