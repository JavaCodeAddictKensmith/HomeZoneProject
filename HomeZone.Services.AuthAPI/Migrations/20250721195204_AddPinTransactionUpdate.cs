using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeZone.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPinTransactionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionPinHash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionPinHash",
                table: "AspNetUsers");
        }
    }
}
