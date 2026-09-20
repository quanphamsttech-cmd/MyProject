using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Migrations
{
    public partial class FixAdmissionPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AdmissionPeriods",
                table: "AdmissionPeriods");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AdmissionPeriods",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdmissionPeriods",
                table: "AdmissionPeriods",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AdmissionPeriods",
                table: "AdmissionPeriods");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AdmissionPeriods",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdmissionPeriods",
                table: "AdmissionPeriods",
                column: "Id");
        }
    }
}