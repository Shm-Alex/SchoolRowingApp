using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRowingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bob");

            migrationBuilder.CreateTable(
                name: "Athletes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Athletes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipPeriods",
                schema: "bob",
                columns: table => new
                {
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    BaseFee = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPeriods", x => new { x.Year, x.Month });
                    table.CheckConstraint("CK_BaseFee", @"""BaseFee"" >= 0");
                    table.CheckConstraint("CK_Month", @"""Month"" >= 1 AND ""Month"" <= 12");
                    table.CheckConstraint("CK_Year", @"""Year"" >= 2020 AND ""Year"" <= 2100");
                });

            migrationBuilder.CreateTable(
                name: "Payers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AthleteMemberships",
                schema: "bob",
                columns: table => new
                {
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipPeriodMonth = table.Column<int>(type: "integer", nullable: false),
                    MembershipPeriodYear = table.Column<int>(type: "integer", nullable: false),
                    ParticipationCoefficient = table.Column<decimal>(type: "numeric(3,1)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteMemberships", x => new { x.AthleteId, x.MembershipPeriodYear, x.MembershipPeriodMonth });
                    table.ForeignKey(
                        name: "FK_AthleteMemberships_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthleteMemberships_MembershipPeriods_MembershipPeriodYear_M~",
                        columns: x => new { x.MembershipPeriodYear, x.MembershipPeriodMonth },
                        principalSchema: "bob",
                        principalTable: "MembershipPeriods",
                        principalColumns: new[] { "Year", "Month" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AthletePayers",
                columns: table => new
                {
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    PayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PayerType = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthletePayers", x => new { x.AthleteId, x.PayerId });
                    table.ForeignKey(
                        name: "FK_AthletePayers_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthletePayers_Payers_PayerId",
                        column: x => x.PayerId,
                        principalTable: "Payers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AthleteMemberships_MembershipPeriodYear_MembershipPeriodMon~",
                schema: "bob",
                table: "AthleteMemberships",
                columns: new[] { "MembershipPeriodYear", "MembershipPeriodMonth" });

            migrationBuilder.CreateIndex(
                name: "IX_AthletePayers_PayerId",
                table: "AthletePayers",
                column: "PayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AthleteMemberships",
                schema: "bob");

            migrationBuilder.DropTable(
                name: "AthletePayers");

            migrationBuilder.DropTable(
                name: "MembershipPeriods",
                schema: "bob");

            migrationBuilder.DropTable(
                name: "Athletes");

            migrationBuilder.DropTable(
                name: "Payers");
        }
    }
}
