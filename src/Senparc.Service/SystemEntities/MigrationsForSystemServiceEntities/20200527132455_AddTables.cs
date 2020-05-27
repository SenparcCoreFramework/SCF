using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Senparc.Service.SystemEntities.MigrationsForSystemServiceEntities
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flag = table.Column<bool>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    AdminRemark = table.Column<string>(maxLength: 300, nullable: true),
                    Remark = table.Column<string>(maxLength: 300, nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    RealName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    PhoneChecked = table.Column<bool>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailChecked = table.Column<bool>(nullable: true),
                    PicUrl = table.Column<string>(nullable: true),
                    HeadImgUrl = table.Column<string>(nullable: true),
                    Package = table.Column<decimal>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    LockMoney = table.Column<decimal>(nullable: false),
                    Sex = table.Column<byte>(nullable: false),
                    QQ = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    ThisLoginTime = table.Column<DateTime>(nullable: false),
                    ThisLoginIp = table.Column<string>(nullable: true),
                    LastLoginTime = table.Column<DateTime>(nullable: false),
                    LastLoginIP = table.Column<string>(nullable: true),
                    Points = table.Column<decimal>(nullable: false),
                    LastWeixinSignInTime = table.Column<DateTime>(nullable: true),
                    WeixinSignTimes = table.Column<int>(nullable: false),
                    WeixinUnionId = table.Column<string>(nullable: true),
                    WeixinOpenId = table.Column<string>(nullable: true),
                    Locked = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminUserInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flag = table.Column<bool>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    AdminRemark = table.Column<string>(maxLength: 300, nullable: true),
                    Remark = table.Column<string>(maxLength: 300, nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    RealName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    ThisLoginTime = table.Column<DateTime>(nullable: false),
                    ThisLoginIp = table.Column<string>(nullable: true),
                    LastLoginTime = table.Column<DateTime>(nullable: false),
                    LastLoginIp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUserInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountPayLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flag = table.Column<bool>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    AdminRemark = table.Column<string>(maxLength: 300, nullable: true),
                    Remark = table.Column<string>(maxLength: 300, nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    OrderNumber = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    PayMoney = table.Column<decimal>(nullable: false),
                    UsedPoints = table.Column<decimal>(nullable: true),
                    CompleteTime = table.Column<DateTime>(nullable: false),
                    AddIp = table.Column<string>(nullable: true),
                    GetPoints = table.Column<decimal>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<byte>(nullable: true),
                    TradeNumber = table.Column<string>(nullable: true),
                    PrepayId = table.Column<string>(nullable: true),
                    PayType = table.Column<int>(nullable: false),
                    OrderType = table.Column<int>(nullable: false),
                    PayParam = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Fee = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPayLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountPayLog_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedBacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flag = table.Column<bool>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    AdminRemark = table.Column<string>(maxLength: 300, nullable: true),
                    Remark = table.Column<string>(maxLength: 300, nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedBacks_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointsLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flag = table.Column<bool>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    AdminRemark = table.Column<string>(maxLength: 300, nullable: true),
                    Remark = table.Column<string>(maxLength: 300, nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    AccountPayLogId = table.Column<int>(nullable: true),
                    Points = table.Column<decimal>(nullable: false),
                    BeforePoints = table.Column<decimal>(nullable: false),
                    AfterPoints = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointsLog_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointsLog_AccountPayLog_AccountPayLogId",
                        column: x => x.AccountPayLogId,
                        principalTable: "AccountPayLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPayLog_AccountId",
                table: "AccountPayLog",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_AccountId",
                table: "FeedBacks",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PointsLog_AccountId",
                table: "PointsLog",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PointsLog_AccountPayLogId",
                table: "PointsLog",
                column: "AccountPayLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUserInfos");

            migrationBuilder.DropTable(
                name: "FeedBacks");

            migrationBuilder.DropTable(
                name: "PointsLog");

            migrationBuilder.DropTable(
                name: "AccountPayLog");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
