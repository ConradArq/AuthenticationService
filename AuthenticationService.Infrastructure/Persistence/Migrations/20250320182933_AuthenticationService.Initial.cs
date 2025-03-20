using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuthenticationServiceInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AuthenticationService");

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "AuthenticationService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "AuthenticationService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationMenu",
                schema: "AuthenticationService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupTitle = table.Column<bool>(type: "bit", nullable: true),
                    Badge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BadgeClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ParentApplicationMenuId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationMenu_ApplicationMenu_ParentApplicationMenuId",
                        column: x => x.ParentApplicationMenuId,
                        principalSchema: "AuthenticationService",
                        principalTable: "ApplicationMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationMenu_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "AuthenticationService",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "AuthenticationService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "AuthenticationService",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "AuthenticationService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MicrosoftId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRegistered = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "AuthenticationService",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoleMenu",
                schema: "AuthenticationService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationRoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationMenuId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleMenu_ApplicationMenu_ApplicationMenuId",
                        column: x => x.ApplicationMenuId,
                        principalSchema: "AuthenticationService",
                        principalTable: "ApplicationMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleMenu_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalSchema: "AuthenticationService",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleMenu_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "AuthenticationService",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "AuthenticationService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "AuthenticationService",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "AuthenticationService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "AuthenticationService",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "AuthenticationService",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "AuthenticationService",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "AuthenticationService",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "AuthenticationService",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "AuthenticationService",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "AuthenticationService",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "AuthenticationService",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "AuthenticationService",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "AuthenticationService",
                table: "Status",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "LastModifiedBy", "LastModifiedDate", "Name", "StatusId" },
                values: new object[,]
                {
                    { 1, "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Active", 1 },
                    { 2, "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Inactive", 1 }
                });

            migrationBuilder.InsertData(
                schema: "AuthenticationService",
                table: "ApplicationMenu",
                columns: new[] { "Id", "Badge", "BadgeClass", "Class", "CreatedBy", "CreatedDate", "GroupTitle", "Icon", "IconType", "LastModifiedBy", "LastModifiedDate", "Order", "ParentApplicationMenuId", "Path", "StatusId", "Title" },
                values: new object[,]
                {
                    { 1, null, null, null, "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, null, null, null, 0, null, null, 1, "MENU" },
                    { 2, null, null, "menu-toggle", "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "calendar", "feather", null, null, 0, null, null, 1, "Calendar" }
                });

            migrationBuilder.InsertData(
                schema: "AuthenticationService",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate", "Name", "NormalizedName", "StatusId" },
                values: new object[,]
                {
                    { "1f4b2341-7326-4fca-a906-7c9db3abbd4b", null, "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guest", "GUEST", 1 },
                    { "b37495f4-c5b4-4cfa-9a34-68f28f0fd6a6", null, "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Manager", "MANAGER", 1 },
                    { "d2674562-193a-41e6-9a92-7f7cb04caf90", null, "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Administrator", "ADMINISTRATOR", 1 }
                });

            migrationBuilder.InsertData(
                schema: "AuthenticationService",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Email", "EmailConfirmed", "FirstName", "GoogleId", "IsRegistered", "LastModifiedBy", "LastModifiedDate", "LastName", "LockoutEnabled", "LockoutEnd", "MicrosoftId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StatusId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", 0, "4bf551a5-746e-4b56-98e2-dd02474ccf46", "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "conra.arq@gmail.com", true, "Conrado", null, true, null, null, "Arquer", false, null, null, "CONRA.ARQ@GMAIL.COM", "CONRAARQ", "AQAAAAIAAYagAAAAEKJ/JVSYfMAJx974FD4O2vDFc3lCX0C5lB1Si4dH3w3SS7vSo8IpCELj6hqHZprTRA==", "555-555", true, "4566d67a-bd12-417e-b8c0-7b7bd12ae40e", 1, false, "conraarq" });

            migrationBuilder.InsertData(
                schema: "AuthenticationService",
                table: "ApplicationRoleMenu",
                columns: new[] { "Id", "ApplicationMenuId", "ApplicationRoleId", "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate", "StatusId" },
                values: new object[] { 1, 1, "d2674562-193a-41e6-9a92-7f7cb04caf90", "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1 });

            migrationBuilder.InsertData(
                schema: "AuthenticationService",
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId", "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate", "StatusId" },
                values: new object[] { "d2674562-193a-41e6-9a92-7f7cb04caf90", "a18be9c0-aa65-4af8-bd17-00bd9344e575", "a18be9c0-aa65-4af8-bd17-00bd9344e575", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationMenu_ParentApplicationMenuId",
                schema: "AuthenticationService",
                table: "ApplicationMenu",
                column: "ParentApplicationMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationMenu_StatusId",
                schema: "AuthenticationService",
                table: "ApplicationMenu",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleMenu_ApplicationMenuId",
                schema: "AuthenticationService",
                table: "ApplicationRoleMenu",
                column: "ApplicationMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleMenu_ApplicationRoleId",
                schema: "AuthenticationService",
                table: "ApplicationRoleMenu",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleMenu_StatusId",
                schema: "AuthenticationService",
                table: "ApplicationRoleMenu",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "AuthenticationService",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_StatusId",
                schema: "AuthenticationService",
                table: "AspNetRoles",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "AuthenticationService",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "AuthenticationService",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "AuthenticationService",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "AuthenticationService",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_StatusId",
                schema: "AuthenticationService",
                table: "AspNetUserRoles",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "AuthenticationService",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StatusId",
                schema: "AuthenticationService",
                table: "AspNetUsers",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "AuthenticationService",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                schema: "AuthenticationService",
                table: "RefreshToken",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRoleMenu",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "ApplicationMenu",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "AuthenticationService");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "AuthenticationService");
        }
    }
}
