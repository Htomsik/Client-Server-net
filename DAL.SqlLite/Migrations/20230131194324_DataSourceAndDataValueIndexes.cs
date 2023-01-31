using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.SqlLite.Migrations
{
    /// <inheritdoc />
    public partial class DataSourceAndDataValueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Values_Time",
                table: "Values",
                column: "Time");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_Name",
                table: "Sources",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Values_Time",
                table: "Values");

            migrationBuilder.DropIndex(
                name: "IX_Sources_Name",
                table: "Sources");
        }
    }
}
