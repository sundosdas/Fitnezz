using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SUBSCRIPTION_WORKOUTS",
                schema: "C##SUNDOS",
                columns: table => new
                {
                    SUBSCRIPTION_ID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    WORKOUT_ID = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C_SUBSCRIPTION_WORKOUT_PK", x => new { x.SUBSCRIPTION_ID, x.WORKOUT_ID });
                    table.ForeignKey(
                        name: "FK_SUBSCRIPTION_WORKOUT_SUBSCRIPTION",
                        column: x => x.SUBSCRIPTION_ID,
                        principalSchema: "C##SUNDOS",
                        principalTable: "SUBSCRIPTIONS",
                        principalColumn: "SUBSCRIPTION_ID");
                    table.ForeignKey(
                        name: "FK_SUBSCRIPTION_WORKOUT_WORKOUT",
                        column: x => x.WORKOUT_ID,
                        principalSchema: "C##SUNDOS",
                        principalTable: "WORKOUT_PLANS",
                        principalColumn: "WORKOUT_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SUBSCRIPTION_WORKOUTS_WORKOUT_ID",
                schema: "C##SUNDOS",
                table: "SUBSCRIPTION_WORKOUTS",
                column: "WORKOUT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SUBSCRIPTION_WORKOUTS",
                schema: "C##SUNDOS");
        }
    }
}
