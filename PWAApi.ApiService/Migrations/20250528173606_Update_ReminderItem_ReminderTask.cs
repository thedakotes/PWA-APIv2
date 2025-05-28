using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventApi.Migrations
{
    /// <inheritdoc />
    public partial class Update_ReminderItem_ReminderTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReminderItems_Reminders_ReminderID",
                table: "ReminderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReminderTasks_Reminders_ReminderID",
                table: "ReminderTasks");

            migrationBuilder.RenameColumn(
                name: "ReminderID",
                table: "ReminderTasks",
                newName: "ReminderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReminderTasks_ReminderID",
                table: "ReminderTasks",
                newName: "IX_ReminderTasks_ReminderId");

            migrationBuilder.RenameColumn(
                name: "ReminderID",
                table: "ReminderItems",
                newName: "ReminderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReminderItems_ReminderID",
                table: "ReminderItems",
                newName: "IX_ReminderItems_ReminderId");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "ReminderTasks",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ReminderTasks",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "PriorityLevel",
                table: "ReminderTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "Reminders",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Reminders",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "ReminderItems",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ReminderItems",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "CalendarEvents",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "CalendarEvents",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderItems_Reminders_ReminderId",
                table: "ReminderItems",
                column: "ReminderId",
                principalTable: "Reminders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderTasks_Reminders_ReminderId",
                table: "ReminderTasks",
                column: "ReminderId",
                principalTable: "Reminders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReminderItems_Reminders_ReminderId",
                table: "ReminderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReminderTasks_Reminders_ReminderId",
                table: "ReminderTasks");

            migrationBuilder.DropColumn(
                name: "PriorityLevel",
                table: "ReminderTasks");

            migrationBuilder.RenameColumn(
                name: "ReminderId",
                table: "ReminderTasks",
                newName: "ReminderID");

            migrationBuilder.RenameIndex(
                name: "IX_ReminderTasks_ReminderId",
                table: "ReminderTasks",
                newName: "IX_ReminderTasks_ReminderID");

            migrationBuilder.RenameColumn(
                name: "ReminderId",
                table: "ReminderItems",
                newName: "ReminderID");

            migrationBuilder.RenameIndex(
                name: "IX_ReminderItems_ReminderId",
                table: "ReminderItems",
                newName: "IX_ReminderItems_ReminderID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "ReminderTasks",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "ReminderTasks",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "Reminders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Reminders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "ReminderItems",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "ReminderItems",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "CalendarEvents",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "CalendarEvents",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderItems_Reminders_ReminderID",
                table: "ReminderItems",
                column: "ReminderID",
                principalTable: "Reminders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderTasks_Reminders_ReminderID",
                table: "ReminderTasks",
                column: "ReminderID",
                principalTable: "Reminders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
