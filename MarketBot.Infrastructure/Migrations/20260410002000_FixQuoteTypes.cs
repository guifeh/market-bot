using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixQuoteTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Quotes"" 
                ALTER COLUMN ""ChangePct"" TYPE numeric(10,4) 
                USING ""ChangePct""::numeric;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChangePct",
                table: "Quotes",
                type: "text",
                precision: 10,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,4)",
                oldPrecision: 10,
                oldScale: 4,
                oldNullable: true);
        }
    }
}
