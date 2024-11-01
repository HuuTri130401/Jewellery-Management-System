using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace THT.JMS.Persistence.Migrations
{
    public partial class Table_Users_and_Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdentityCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdentityCardDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdentityCardAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: true),
                    PurchaseRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolesUsers",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUsers", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RolesUsers_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesUsers_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Created", "CreatedBy", "Deleted", "IsActive", "RoleName", "Updated", "UpdatedBy" },
                values: new object[] { new Guid("3ffa96e3-836a-49ea-bf0f-826fada4153c"), null, null, false, true, "Admin", null, null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Created", "CreatedBy", "Deleted", "IsActive", "RoleName", "Updated", "UpdatedBy" },
                values: new object[] { new Guid("490ecc09-2b02-488c-bdbb-c2994bcda6fb"), null, null, false, true, "User", null, null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Birthday", "Code", "Created", "CreatedBy", "Deleted", "Email", "FirstName", "FullName", "Gender", "IdentityCard", "IdentityCardAddress", "IdentityCardDate", "IsActive", "IsAdmin", "LastName", "Password", "Phone", "PurchaseRevenue", "RefreshToken", "RefreshTokenExpiryTime", "Status", "Updated", "UpdatedBy", "UserName" },
                values: new object[] { new Guid("74cab1a6-0eee-4920-ae09-4300a1af4cd1"), "Binh Phuoc", null, "AD-01", new DateTime(2024, 10, 9, 17, 36, 44, 213, DateTimeKind.Utc).AddTicks(313), new Guid("00000000-0000-0000-0000-000000000000"), false, "admin@gmail.com", "Tri", "Tran Huu Tri", 2, "12345", "Binh Phuoc", new DateTime(2015, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, "Tran", "FF9A32AB4A6E687FF64C2A139A9D04BD3AD58F10", "0333444555", 0m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_RolesUsers_UsersId",
                table: "RolesUsers",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesUsers");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
