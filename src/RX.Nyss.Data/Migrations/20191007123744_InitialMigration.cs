﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RX.Nyss.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertRule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountThreshold = table.Column<int>(nullable: false),
                    HoursThreshold = table.Column<int>(nullable: true),
                    MetersThreshold = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertRule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false),
                    LanguageCode = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false),
                    LanguageCode = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlertRecipient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlertRuleId = table.Column<int>(nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertRecipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertRecipient_AlertRule_AlertRuleId",
                        column: x => x.AlertRuleId,
                        principalTable: "AlertRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthRisk",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    HealthRiskType = table.Column<string>(maxLength: 20, nullable: false),
                    HealthRiskCode = table.Column<int>(nullable: false),
                    AlertRuleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthRisk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthRisk_AlertRule_AlertRuleId",
                        column: x => x.AlertRuleId,
                        principalTable: "AlertRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Localization",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    ApplicationLanguageId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localization", x => new { x.ApplicationLanguageId, x.Key });
                    table.ForeignKey(
                        name: "FK_Localization_ApplicationLanguage_ApplicationLanguageId",
                        column: x => x.ApplicationLanguageId,
                        principalTable: "ApplicationLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalizedTemplate",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    ApplicationLanguageId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizedTemplate", x => new { x.ApplicationLanguageId, x.Key });
                    table.ForeignKey(
                        name: "FK_LocalizedTemplate_ApplicationLanguage_ApplicationLanguageId",
                        column: x => x.ApplicationLanguageId,
                        principalTable: "ApplicationLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NationalSocieties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    IsArchived = table.Column<bool>(nullable: false),
                    RegionCustomName = table.Column<string>(maxLength: 100, nullable: true),
                    DistrictCustomName = table.Column<string>(maxLength: 100, nullable: true),
                    VillageCustomName = table.Column<string>(maxLength: 100, nullable: true),
                    ZoneCustomName = table.Column<string>(maxLength: 100, nullable: true),
                    ContentLanguageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NationalSocieties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NationalSocieties_ContentLanguages_ContentLanguageId",
                        column: x => x.ContentLanguageId,
                        principalTable: "ContentLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TimeZone = table.Column<string>(maxLength: 50, nullable: true),
                    State = table.Column<string>(maxLength: 50, nullable: false),
                    ContentLanguageId = table.Column<int>(nullable: false),
                    NationalSocietyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_ContentLanguages_ContentLanguageId",
                        column: x => x.ContentLanguageId,
                        principalTable: "ContentLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_NationalSocieties_NationalSocietyId",
                        column: x => x.NationalSocietyId,
                        principalTable: "NationalSocieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    NationalSocietyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_NationalSocieties_NationalSocietyId",
                        column: x => x.NationalSocietyId,
                        principalTable: "NationalSocieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_District_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Village",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    DistrictId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Village", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Village_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    VillageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zone_Village_VillageId",
                        column: x => x.VillageId,
                        principalTable: "Village",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Role = table.Column<string>(maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: false),
                    AdditionalPhoneNumber = table.Column<string>(maxLength: 20, nullable: true),
                    Organization = table.Column<string>(maxLength: 100, nullable: true),
                    IsFirstLogin = table.Column<bool>(nullable: false),
                    ApplicationLanguageId = table.Column<int>(nullable: true),
                    IsDataOwner = table.Column<bool>(nullable: true),
                    HasConsented = table.Column<bool>(nullable: true),
                    ConsentedDateTime = table.Column<DateTime>(nullable: true),
                    NationalSocietyId = table.Column<int>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    SupervisorUser_NationalSocietyId = table.Column<int>(nullable: true),
                    VillageId = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: true),
                    DataManagerUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_NationalSocieties_NationalSocietyId",
                        column: x => x.NationalSocietyId,
                        principalTable: "NationalSocieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_DataManagerUserId",
                        column: x => x.DataManagerUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_NationalSocieties_SupervisorUser_NationalSocietyId",
                        column: x => x.SupervisorUser_NationalSocietyId,
                        principalTable: "NationalSocieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Village_VillageId",
                        column: x => x.VillageId,
                        principalTable: "Village",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_ApplicationLanguage_ApplicationLanguageId",
                        column: x => x.ApplicationLanguageId,
                        principalTable: "ApplicationLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertRecipient_AlertRuleId",
                table: "AlertRecipient",
                column: "AlertRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLanguage_DisplayName",
                table: "ApplicationLanguage",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentLanguages_DisplayName",
                table: "ContentLanguages",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_District_RegionId",
                table: "District",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRisk_AlertRuleId",
                table: "HealthRisk",
                column: "AlertRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_NationalSocieties_ContentLanguageId",
                table: "NationalSocieties",
                column: "ContentLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NationalSocieties_Name",
                table: "NationalSocieties",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ContentLanguageId",
                table: "Project",
                column: "ContentLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_NationalSocietyId",
                table: "Project",
                column: "NationalSocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_NationalSocietyId",
                table: "Region",
                column: "NationalSocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_NationalSocietyId",
                table: "User",
                column: "NationalSocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DataManagerUserId",
                table: "User",
                column: "DataManagerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SupervisorUser_NationalSocietyId",
                table: "User",
                column: "SupervisorUser_NationalSocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_VillageId",
                table: "User",
                column: "VillageId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ZoneId",
                table: "User",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ApplicationLanguageId",
                table: "User",
                column: "ApplicationLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Village_DistrictId",
                table: "Village",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_VillageId",
                table: "Zone",
                column: "VillageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertRecipient");

            migrationBuilder.DropTable(
                name: "HealthRisk");

            migrationBuilder.DropTable(
                name: "Localization");

            migrationBuilder.DropTable(
                name: "LocalizedTemplate");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AlertRule");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "ApplicationLanguage");

            migrationBuilder.DropTable(
                name: "Village");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "NationalSocieties");

            migrationBuilder.DropTable(
                name: "ContentLanguages");
        }
    }
}
