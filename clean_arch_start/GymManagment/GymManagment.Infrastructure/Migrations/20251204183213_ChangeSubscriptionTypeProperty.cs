using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSubscriptionTypeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubcriptionType",
                table: "Subscriptions",
                newName: "AdminId");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionType",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Subscriptions",
                newName: "SubcriptionType");
        }
    }
}
