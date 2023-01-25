using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookrental.Migrations
{
    public partial class Initialaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookModel",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookModel", x => x.BookId);
                });

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
                name: "BookModelCustomerModel",
                columns: table => new
                {
                    RentedBooksBookId = table.Column<int>(type: "int", nullable: false),
                    RentedCustomerCustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookModelCustomerModel", x => new { x.RentedBooksBookId, x.RentedCustomerCustomerId });
                    table.ForeignKey(
                        name: "FK_BookModelCustomerModel_BookModel_RentedBooksBookId",
                        column: x => x.RentedBooksBookId,
                        principalTable: "BookModel",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookModelCustomerModel_CustomerModel_RentedCustomerCustomerId",
                        column: x => x.RentedCustomerCustomerId,
                        principalTable: "CustomerModel",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookModelCustomerModel_RentedCustomerCustomerId",
                table: "BookModelCustomerModel",
                column: "RentedCustomerCustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookModelCustomerModel");

            migrationBuilder.DropTable(
                name: "BookModel");

            migrationBuilder.DropTable(
                name: "CustomerModel");
        }
    }
}
