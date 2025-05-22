using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventApi.Migrations
{
    /// <inheritdoc />
    public partial class Add_ReminderTask_Update_Reminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDate",
                table: "Reminders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OccurrenceCounter",
                table: "Reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceCount",
                table: "Reminders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceInterval",
                table: "Reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceUnit",
                table: "Reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                table: "Reminders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReminderTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReminderID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityLevel = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReminderTasks_Reminders_ReminderID",
                        column: x => x.ReminderID,
                        principalTable: "Reminders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReminderTasks_ReminderID",
                table: "ReminderTasks",
                column: "ReminderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReminderTasks");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "OccurrenceCounter",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "RecurrenceCount",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "RecurrenceInterval",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "RecurrenceUnit",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Reminders");
        }
    }
}
