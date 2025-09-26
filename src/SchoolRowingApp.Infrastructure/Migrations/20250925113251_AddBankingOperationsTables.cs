using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRowingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBankingOperationsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bob");

            migrationBuilder.EnsureSchema(
                name: "banking");

            migrationBuilder.CreateTable(
                name: "TransactionImports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    ImportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SuccessCount = table.Column<int>(type: "integer", nullable: false),
                    SkippedCount = table.Column<int>(type: "integer", nullable: false),
                    ErrorCount = table.Column<int>(type: "integer", nullable: false),
                    TotalRows = table.Column<int>(type: "integer", nullable: false),
                    FileHash = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionImports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "banking",
                columns: table => new
                {
                    OperationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CardLastDigits = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaymentCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Cashback = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MccCode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BonusAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RoundUpAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OperationAmountWithRoundUp = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => new { x.OperationDate, x.Amount, x.Currency });
                });


            migrationBuilder.CreateTable(
                name: "TransactionImportDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionImportId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    Result = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    RawData = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionImportDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionImportDetails_TransactionImports_TransactionImpo~",
                        column: x => x.TransactionImportId,
                        principalTable: "TransactionImports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTransactionImport",
                columns: table => new
                {
                    TransactionImportsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionsOperationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionsAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TransactionsCurrency = table.Column<string>(type: "character varying(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTransactionImport", x => new { x.TransactionImportsId, x.TransactionsOperationDate, x.TransactionsAmount, x.TransactionsCurrency });
                    table.ForeignKey(
                        name: "FK_TransactionTransactionImport_TransactionImports_Transaction~",
                        column: x => x.TransactionImportsId,
                        principalTable: "TransactionImports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionTransactionImport_Transactions_TransactionsOpera~",
                        columns: x => new { x.TransactionsOperationDate, x.TransactionsAmount, x.TransactionsCurrency },
                        principalSchema: "banking",
                        principalTable: "Transactions",
                        principalColumns: new[] { "OperationDate", "Amount", "Currency" },
                        onDelete: ReferentialAction.Cascade);
                });



            migrationBuilder.CreateIndex(
                name: "IX_TransactionImportDetails_TransactionImportId",
                table: "TransactionImportDetails",
                column: "TransactionImportId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Category",
                schema: "banking",
                table: "Transactions",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IsProcessed",
                schema: "banking",
                table: "Transactions",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentDate",
                schema: "banking",
                table: "Transactions",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Status",
                schema: "banking",
                table: "Transactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTransactionImport_TransactionsOperationDate_Tran~",
                table: "TransactionTransactionImport",
                columns: new[] { "TransactionsOperationDate", "TransactionsAmount", "TransactionsCurrency" });
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
                name: "TransactionImportDetails");

            migrationBuilder.DropTable(
                name: "TransactionTransactionImport");

            migrationBuilder.DropTable(
                name: "MembershipPeriods",
                schema: "bob");

            migrationBuilder.DropTable(
                name: "Athletes");

            migrationBuilder.DropTable(
                name: "Payers");

            migrationBuilder.DropTable(
                name: "TransactionImports");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "banking");
        }
    }
}
