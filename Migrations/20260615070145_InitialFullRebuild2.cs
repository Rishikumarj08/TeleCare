using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeleCare.Migrations
{
    /// <inheritdoc />
    public partial class InitialFullRebuild2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentName",
                table: "VisitNotes");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "VisitNotes");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VisitNotes");

            migrationBuilder.RenameColumn(
                name: "VisitNoteStatus",
                table: "VisitNotes",
                newName: "PatientID");

            migrationBuilder.RenameColumn(
                name: "PatientReferenceNumber",
                table: "VisitNotes",
                newName: "ClinicianID");

            migrationBuilder.RenameColumn(
                name: "Orders",
                table: "VisitNotes",
                newName: "NoteText");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "VisitNotes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "VisitNotes",
                newName: "NoteID");

            migrationBuilder.RenameColumn(
                name: "PatientReferenceNumber",
                table: "Appointments",
                newName: "PatientID");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Appointments",
                newName: "ScheduledAt");

            migrationBuilder.RenameColumn(
                name: "AppointmentType",
                table: "Appointments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "AppointmentStatus",
                table: "Appointments",
                newName: "DurationMinutes");

            migrationBuilder.RenameColumn(
                name: "AppointmentMode",
                table: "Appointments",
                newName: "Mode");

            migrationBuilder.RenameColumn(
                name: "AppointmentDateTime",
                table: "Appointments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Appointments",
                newName: "AppID");

            migrationBuilder.AddColumn<int>(
                name: "AppID",
                table: "VisitNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentsURIJSON",
                table: "VisitNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiagnosesJSON",
                table: "VisitNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrdersJSON",
                table: "VisitNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClinicianID",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LocationURI",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppID",
                table: "VisitNotes");

            migrationBuilder.DropColumn(
                name: "AttachmentsURIJSON",
                table: "VisitNotes");

            migrationBuilder.DropColumn(
                name: "DiagnosesJSON",
                table: "VisitNotes");

            migrationBuilder.DropColumn(
                name: "OrdersJSON",
                table: "VisitNotes");

            migrationBuilder.DropColumn(
                name: "ClinicianID",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "LocationURI",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientID",
                table: "VisitNotes",
                newName: "VisitNoteStatus");

            migrationBuilder.RenameColumn(
                name: "NoteText",
                table: "VisitNotes",
                newName: "Orders");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "VisitNotes",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ClinicianID",
                table: "VisitNotes",
                newName: "PatientReferenceNumber");

            migrationBuilder.RenameColumn(
                name: "NoteID",
                table: "VisitNotes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Appointments",
                newName: "AppointmentType");

            migrationBuilder.RenameColumn(
                name: "ScheduledAt",
                table: "Appointments",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "PatientID",
                table: "Appointments",
                newName: "PatientReferenceNumber");

            migrationBuilder.RenameColumn(
                name: "Mode",
                table: "Appointments",
                newName: "AppointmentMode");

            migrationBuilder.RenameColumn(
                name: "DurationMinutes",
                table: "Appointments",
                newName: "AppointmentStatus");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Appointments",
                newName: "AppointmentDateTime");

            migrationBuilder.RenameColumn(
                name: "AppID",
                table: "Appointments",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                table: "VisitNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "VisitNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VisitNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
