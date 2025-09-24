using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolRowingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseCompositeKeyForMembershipPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Устанавливаем расширение для генерации UUID
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");

            // 1. Добавляем временные столбцы для Month и Year в AthleteMemberships
            migrationBuilder.AddColumn<int>(
                name: "MembershipPeriodMonth",
                table: "AthleteMemberships",
                schema: "bob",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MembershipPeriodYear",
                table: "AthleteMemberships",
                schema: "bob",
                nullable: false,
                defaultValue: 0);

            // 2. Заполняем новые столбцы данными из MembershipPeriod
            // Важно: используем правильный синтаксис для PostgreSQL
            migrationBuilder.Sql(@"
        UPDATE ""bob"".""AthleteMemberships"" AS am
        SET ""MembershipPeriodMonth"" = mp.""Month"",
            ""MembershipPeriodYear"" = mp.""Year""
        FROM ""bob"".""MembershipPeriods"" AS mp
        WHERE am.""MembershipPeriodId"" = mp.""Id""
    ");

            // 3. Удаляем внешний ключ, ссылающийся на MembershipPeriods
            migrationBuilder.DropForeignKey(
                name: "FK_AthleteMemberships_MembershipPeriods_MembershipPeriodId",
                table: "AthleteMemberships",
                schema: "bob");

            // 4. Удаляем старый первичный ключ из MembershipPeriods
            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipPeriods",
                table: "MembershipPeriods",
                schema: "bob");

            // 5. Удаляем столбец Id из MembershipPeriods
            migrationBuilder.DropColumn(
                name: "Id",
                table: "MembershipPeriods",
                schema: "bob");

            // 6. Добавляем составной первичный ключ в MembershipPeriods
            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipPeriods",
                table: "MembershipPeriods",
                schema: "bob",
                columns: new[] { "Year", "Month" });

            // 7. Добавляем check-констрейнты для валидации данных
            migrationBuilder.AddCheckConstraint(
                name: "CK_Month",
                table: "MembershipPeriods",
                schema: "bob",
                sql: "\"Month\" >= 1 AND \"Month\" <= 12");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Year",
                table: "MembershipPeriods",
                schema: "bob",
                sql: "Year >= 2020 AND Year <= 2100");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BaseFee",
                table: "MembershipPeriods",
                schema: "bob",
                sql: "BaseFee >= 0");

            // 8. Создаем новый составной внешний ключ
            migrationBuilder.AddForeignKey(
                name: "FK_AthleteMemberships_MembershipPeriods_Month_Year",
                table: "AthleteMemberships",
                schema: "bob",
                columns: ["MembershipPeriodYear", "MembershipPeriodMonth"],
                principalTable: "MembershipPeriods",
                principalColumns: new[] { "Year", "Month" },
                onDelete: ReferentialAction.Cascade);

            // 9. Удаляем старый столбец MembershipPeriodId
            migrationBuilder.DropColumn(
                name: "MembershipPeriodId",
                table: "AthleteMemberships",
                schema: "bob");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Добавляем обратно столбец MembershipPeriodId в AthleteMemberships
            migrationBuilder.AddColumn<Guid>(
                name: "MembershipPeriodId",
                table: "AthleteMemberships",
                schema: "bob",
                nullable: false,
                defaultValue: Guid.Empty);

            // 2. Удаляем составной внешний ключ
            migrationBuilder.DropForeignKey(
                name: "FK_AthleteMemberships_MembershipPeriods_Month_Year",
                table: "AthleteMemberships",
                schema: "bob");

            // 3. Удаляем check-констрейнты
            migrationBuilder.DropCheckConstraint(
                name: "CK_Month",
                table: "MembershipPeriods",
                schema: "bob");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Year",
                table: "MembershipPeriods",
                schema: "bob");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BaseFee",
                table: "MembershipPeriods",
                schema: "bob");

            // 4. Удаляем составной первичный ключ из MembershipPeriods
            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipPeriods",
                table: "MembershipPeriods",
                schema: "bob");

            // 5. Добавляем временный столбец Id в MembershipPeriods
            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "MembershipPeriods",
                schema: "bob",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            // 6. Заполняем временный Id (генерируем новые GUID для существующих записей)
            // Это нужно, так как у нас не было Id ранее
            migrationBuilder.Sql(@"
        UPDATE ""bob"".""MembershipPeriods""
        SET ""TempId"" = uuid_generate_v4()
    ");

            // 7. Добавляем временный столбец в AthleteMemberships для связи
            migrationBuilder.AddColumn<Guid>(
                name: "TempMembershipPeriodId",
                table: "AthleteMemberships",
                schema: "bob",
                nullable: true);

            // 8. Заполняем временный столбец
            migrationBuilder.Sql(@"
        UPDATE ""bob"".""AthleteMemberships"" AS am
        SET ""TempMembershipPeriodId"" = mp.""TempId""
        FROM ""bob"".""MembershipPeriods"" AS mp
        WHERE am.""MembershipPeriodYear"" = mp.""Year"" 
          AND am.""MembershipPeriodMonth"" = mp.""Month""
    ");

            // 9. Переносим данные из временного столбца в окончательный
            migrationBuilder.Sql(@"
        UPDATE ""bob"".""AthleteMemberships""
        SET ""MembershipPeriodId"" = ""TempMembershipPeriodId""
    ");

            // 10. Удаляем временные столбцы из AthleteMemberships
            migrationBuilder.DropColumn(
                name: "TempMembershipPeriodId",
                table: "AthleteMemberships",
                schema: "bob");

            migrationBuilder.DropColumn(
                name: "MembershipPeriodMonth",
                table: "AthleteMemberships",
                schema: "bob");

            migrationBuilder.DropColumn(
                name: "MembershipPeriodYear",
                table: "AthleteMemberships",
                schema: "bob");

            // 11. Переименовываем временный Id в окончательный
            migrationBuilder.RenameColumn(
                name: "TempId",
                table: "MembershipPeriods",
                schema: "bob",
                newName: "Id");

            // 12. Создаем новый первичный ключ
            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipPeriods",
                table: "MembershipPeriods",
                schema: "bob",
                column: "Id");

            // 13. Создаем внешний ключ на новый Id
            migrationBuilder.AddForeignKey(
                name: "FK_AthleteMemberships_MembershipPeriods_MembershipPeriodId",
                table: "AthleteMemberships",
                schema: "bob",
                column: "MembershipPeriodId",
                principalTable: "MembershipPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
