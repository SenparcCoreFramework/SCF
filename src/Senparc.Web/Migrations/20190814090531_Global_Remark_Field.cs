using Microsoft.EntityFrameworkCore.Migrations;

namespace Senparc.Web.Migrations
{
    public partial class Global_Remark_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "SystemConfigs",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "SystemConfigs",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "SysRoles",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "SysRoles",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "SysRoleAdminUserInfos",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "SysRoleAdminUserInfos",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "SysPermission",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "SysPermission",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "SysMenus",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "SysMenus",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "SysButtons",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "SysButtons",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "PointsLogs",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "PointsLogs",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "FeedBacks",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "FeedBacks",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "AdminUserInfos",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "AdminUserInfos",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "Accounts",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Accounts",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemark",
                table: "AccountPayLogs",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "AccountPayLogs",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "SystemConfigs");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "SystemConfigs");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "SysRoles");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "SysRoles");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "SysRoleAdminUserInfos");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "SysRoleAdminUserInfos");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "SysPermission");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "SysPermission");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "SysMenus");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "SysMenus");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "SysButtons");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "SysButtons");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "PointsLogs");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "PointsLogs");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "AdminUserInfos");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "AdminUserInfos");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AdminRemark",
                table: "AccountPayLogs");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "AccountPayLogs");
        }
    }
}
