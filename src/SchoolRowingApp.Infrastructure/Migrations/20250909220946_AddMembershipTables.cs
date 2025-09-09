using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRowingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipTables : Migration
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    BaseFee = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPeriods", x => x.Id);
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
                    MembershipPeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipationCoefficient = table.Column<decimal>(type: "numeric(3,1)", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteMemberships", x => new { x.AthleteId, x.MembershipPeriodId });
                    table.ForeignKey(
                        name: "FK_AthleteMemberships_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthleteMemberships_MembershipPeriods_MembershipPeriodId",
                        column: x => x.MembershipPeriodId,
                        principalSchema: "bob",
                        principalTable: "MembershipPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AthletePayers",
                columns: table => new
                {
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    PayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PayerType = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "IX_AthleteMemberships_MembershipPeriodId",
                schema: "bob",
                table: "AthleteMemberships",
                column: "MembershipPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_AthletePayers_PayerId",
                table: "AthletePayers",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPeriods_Year_Month",
                schema: "bob",
                table: "MembershipPeriods",
                columns: new[] { "Year", "Month" },
                unique: true);
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
