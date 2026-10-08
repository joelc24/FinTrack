using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseSubCategoryTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseSubCategoryTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseSubCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseSubCategoryTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseSubCategoryTags_ExpenseSubCategories_ExpenseSubCategoryId",
                        column: x => x.ExpenseSubCategoryId,
                        principalTable: "ExpenseSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseSubCategoryTags_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseSubCategoryTags_ExpenseId_ExpenseSubCategoryId",
                table: "ExpenseSubCategoryTags",
                columns: new[] { "ExpenseId", "ExpenseSubCategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseSubCategoryTags_ExpenseSubCategoryId",
                table: "ExpenseSubCategoryTags",
                column: "ExpenseSubCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseSubCategoryTags");
        }
    }
}
