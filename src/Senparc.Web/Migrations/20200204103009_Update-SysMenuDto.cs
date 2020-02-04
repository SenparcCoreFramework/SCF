using Microsoft.EntityFrameworkCore.Migrations;

namespace Senparc.Web.Migrations
{
    public partial class UpdateSysMenuDto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceCode",
                table: "SysPermission",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceCode",
                table: "SysPermission");
        }
    }
}
