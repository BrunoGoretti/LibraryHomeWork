using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookrental.Migrations
{
    public partial class Initialaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerModel",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerModel", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "BookModel",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentedDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerModelCustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookModel", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_BookModel_CustomerModel_CustomerModelCustomerId",
                        column: x => x.CustomerModelCustomerId,
                        principalTable: "CustomerModel",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookModel_CustomerModelCustomerId",
                table: "BookModel",
                column: "CustomerModelCustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookModel");

            migrationBuilder.DropTable(
                name: "CustomerModel");
        }
    }
}
