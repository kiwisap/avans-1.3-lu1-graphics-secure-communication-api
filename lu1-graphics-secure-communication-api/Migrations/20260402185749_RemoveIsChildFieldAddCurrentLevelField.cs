using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lu1_graphics_secure_communication_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsChildFieldAddCurrentLevelField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChild",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CurrentLevel",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLevel",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsChild",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
