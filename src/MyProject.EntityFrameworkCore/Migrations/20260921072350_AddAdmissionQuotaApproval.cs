using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmissionQuotaApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApprovedBy",
                table: "AdmissionQuotas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedTime",
                table: "AdmissionQuotas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "AdmissionQuotas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AdmissionQuotas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "AdmissionQuotas");

            migrationBuilder.DropColumn(
                name: "ApprovedTime",
                table: "AdmissionQuotas");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "AdmissionQuotas");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AdmissionQuotas");
        }
    }
}
