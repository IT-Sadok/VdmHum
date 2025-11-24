using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTokenHashMaxLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "token_hash",
                table: "refresh_tokens",
                type: "character varying(44)",
                maxLength: 44,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(88)",
                oldMaxLength: 88);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "token_hash",
                table: "refresh_tokens",
                type: "character varying(88)",
                maxLength: 88,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(44)",
                oldMaxLength: 44);
        }
    }
}
