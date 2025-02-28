using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingManager.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "billing",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    invoice_number = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billing", x => x.id);
                    table.ForeignKey(
                        name: "FK_billing_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "billing_lines",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric", nullable: false),
                    sub_total = table.Column<decimal>(type: "numeric", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    billing_id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billing_lines", x => x.id);
                    table.ForeignKey(
                        name: "FK_billing_lines_billing_billing_id",
                        column: x => x.billing_id,
                        principalTable: "billing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_billing_lines_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_billing_customer_id",
                table: "billing",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_billing_lines_billing_id",
                table: "billing_lines",
                column: "billing_id");

            migrationBuilder.CreateIndex(
                name: "IX_billing_lines_product_id",
                table: "billing_lines",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billing_lines");

            migrationBuilder.DropTable(
                name: "billing");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "customer");
        }
    }
}
