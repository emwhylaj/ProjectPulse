using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPulseAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixGlobalQueryFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProjectMembers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "JoinedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(7547), new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(7550) });

            migrationBuilder.UpdateData(
                table: "ProjectMembers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "JoinedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(7558), new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(7557), new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(7560) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(7362), new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(7364) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8099), new DateTime(2025, 8, 8, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8078), new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8101) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 5, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8112), new DateTime(2025, 8, 16, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8108), new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8113) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 6, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8121), new DateTime(2025, 8, 18, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8118), new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(8123) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 15, 21, 30, 996, DateTimeKind.Utc).AddTicks(4428), "$2a$12$7kTU7K.DpnbPRU6IIfyYUOdTj3tyHT1sqojdpJw2fRrlEP6c0ToMG", new DateTime(2025, 8, 13, 15, 21, 30, 996, DateTimeKind.Utc).AddTicks(4438) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 15, 21, 31, 901, DateTimeKind.Utc).AddTicks(7474), "$2a$12$2kV6p/FsQCD.8Kb/Flo8U.t2jPn6bARWvElxXi4k9M4hY1W0m.IkO", new DateTime(2025, 8, 13, 15, 21, 31, 901, DateTimeKind.Utc).AddTicks(7487) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(4466), "$2a$12$6swoz/kXiRKF/bCCdw0B3ux29v3SXpddELyMh.s.ZChC64Q5sK0p2", new DateTime(2025, 8, 13, 15, 21, 32, 720, DateTimeKind.Utc).AddTicks(4478) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProjectMembers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "JoinedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6523), new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6522), new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6524) });

            migrationBuilder.UpdateData(
                table: "ProjectMembers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "JoinedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6529), new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6528), new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6529) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6463), new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6464) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6621), new DateTime(2025, 8, 8, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6606), new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6622) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 5, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6629), new DateTime(2025, 8, 16, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6626), new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6630) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 6, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6635), new DateTime(2025, 8, 18, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6633), new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(6636) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 13, 2, 41, 161, DateTimeKind.Utc).AddTicks(1066), "$2a$12$sgl3.yB/rRdoTpTVzfha9.9cTM2YGAebZFzl0GUNuj4h46RCKjWCS", new DateTime(2025, 8, 13, 13, 2, 41, 161, DateTimeKind.Utc).AddTicks(1074) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 13, 2, 41, 588, DateTimeKind.Utc).AddTicks(8723), "$2a$12$B0LiTzQv/CIVIblJGNfLPulu0.mHYdGXXYiX0KdBHyg7Vc4Vs15ka", new DateTime(2025, 8, 13, 13, 2, 41, 588, DateTimeKind.Utc).AddTicks(8737) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(4361), "$2a$12$W8UZLCk5kph6aYgrsJOK9eiCdPMshSrjQdxeWSocXqXH7ANtKEFwK", new DateTime(2025, 8, 13, 13, 2, 42, 42, DateTimeKind.Utc).AddTicks(4372) });
        }
    }
}
