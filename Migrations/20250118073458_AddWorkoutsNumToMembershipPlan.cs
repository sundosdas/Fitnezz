using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutsNumToMembershipPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutsNum",
                schema: "C##SUNDOS",
                table: "MEMBERSHIP_PLANS",
                type: "NUMBER(10)",
                nullable: true,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkoutsNum",
                schema: "C##SUNDOS",
                table: "MEMBERSHIP_PLANS");
        }

    }
}
