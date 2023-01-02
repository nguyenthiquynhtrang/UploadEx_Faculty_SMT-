using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenThiQuynhTrangBTH2.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablePosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PositionID",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    PositionID = table.Column<string>(type: "TEXT", nullable: false),
                    PositionName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.PositionID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionID",
                table: "Employees",
                column: "PositionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Position_PositionID",
                table: "Employees",
                column: "PositionID",
                principalTable: "Position",
                principalColumn: "PositionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Position_PositionID",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PositionID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PositionID",
                table: "Employees");
        }
    }
}
