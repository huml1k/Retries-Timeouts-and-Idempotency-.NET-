using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIGateway.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_httpDataEntities_idempotencyKeyEntities_IdempotencyKeyId",
                table: "httpDataEntities");

            migrationBuilder.DropIndex(
                name: "IX_httpDataEntities_IdempotencyKeyId",
                table: "httpDataEntities");

            migrationBuilder.AlterColumn<string>(
                name: "IdempotencyKey",
                table: "idempotencyKeyEntities",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "idempotencyKeyEntities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseBody",
                table: "httpDataEntities",
                type: "varchar(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RequestPath",
                table: "httpDataEntities",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RequestMethod",
                table: "httpDataEntities",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestDate",
                table: "httpDataEntities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "RequestBody",
                table: "httpDataEntities",
                type: "varchar(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "userEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FinancialProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "financialProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnpaidCredit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreditDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financialProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_financialProfiles_userEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "userEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdempotencyKey_Key",
                table: "idempotencyKeyEntities",
                column: "IdempotencyKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_idempotencyKeyEntities_HttpExchanceDataID",
                table: "idempotencyKeyEntities",
                column: "HttpExchanceDataID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_financialProfiles_UserId",
                table: "financialProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_idempotencyKeyEntities_httpDataEntities_HttpExchanceDataID",
                table: "idempotencyKeyEntities",
                column: "HttpExchanceDataID",
                principalTable: "httpDataEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_idempotencyKeyEntities_httpDataEntities_HttpExchanceDataID",
                table: "idempotencyKeyEntities");

            migrationBuilder.DropTable(
                name: "financialProfiles");

            migrationBuilder.DropTable(
                name: "userEntities");

            migrationBuilder.DropIndex(
                name: "IX_IdempotencyKey_Key",
                table: "idempotencyKeyEntities");

            migrationBuilder.DropIndex(
                name: "IX_idempotencyKeyEntities_HttpExchanceDataID",
                table: "idempotencyKeyEntities");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdempotencyKey",
                table: "idempotencyKeyEntities",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "idempotencyKeyEntities",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseBody",
                table: "httpDataEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)");

            migrationBuilder.AlterColumn<string>(
                name: "RequestPath",
                table: "httpDataEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "RequestMethod",
                table: "httpDataEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestDate",
                table: "httpDataEntities",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "RequestBody",
                table: "httpDataEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)");

            migrationBuilder.CreateIndex(
                name: "IX_httpDataEntities_IdempotencyKeyId",
                table: "httpDataEntities",
                column: "IdempotencyKeyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_httpDataEntities_idempotencyKeyEntities_IdempotencyKeyId",
                table: "httpDataEntities",
                column: "IdempotencyKeyId",
                principalTable: "idempotencyKeyEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
