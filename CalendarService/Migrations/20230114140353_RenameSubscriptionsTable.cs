using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarService.Migrations
{
    public partial class RenameSubscriptionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarUserSubscription_AppUsers_UserId",
                table: "CalendarUserSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarUserSubscription_Calendars_CalendarId",
                table: "CalendarUserSubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarUserSubscription",
                table: "CalendarUserSubscription");

            migrationBuilder.RenameTable(
                name: "CalendarUserSubscription",
                newName: "Subscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarUserSubscription_CalendarId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_CalendarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                columns: new[] { "UserId", "CalendarId" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELV758ntHPfIeMqPhp1t1XHgnKBNw1Er9Uz1U0npZCe84AgDCbXDbRBspiuKO+LLOQ==");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "bdb0ce75-e54a-4c3d-9460-3eaa637899eb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "415ac970-fc1a-4b5c-8553-be07b6e35c60");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AppUsers_UserId",
                table: "Subscriptions",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Calendars_CalendarId",
                table: "Subscriptions",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AppUsers_UserId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Calendars_CalendarId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "CalendarUserSubscription");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_CalendarId",
                table: "CalendarUserSubscription",
                newName: "IX_CalendarUserSubscription_CalendarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarUserSubscription",
                table: "CalendarUserSubscription",
                columns: new[] { "UserId", "CalendarId" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEOniW11BAt9rfl3ZeDYANb8TDOF+49XSYdel+QDDO+qepSyjJflaTQLwfBbxtcO5zg==");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "80763544-9597-437c-bc5a-60b7e96edd5e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1cb1d5c4-f541-4272-9072-da11f82e33b1");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarUserSubscription_AppUsers_UserId",
                table: "CalendarUserSubscription",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarUserSubscription_Calendars_CalendarId",
                table: "CalendarUserSubscription",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
