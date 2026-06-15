using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeleCare.Migrations
{
    /// <inheritdoc />
    public partial class FullRebuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Users_PatientID",
                table: "Charges");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Payers_PayerID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Users_PatientID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Claims_ClaimID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "RoleID1",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientUserID",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayerID1",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientUserID",
                table: "Charges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PerformedByUserID",
                table: "AuditLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitNotes_AppID",
                table: "VisitNotes",
                column: "AppID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitNotes_ClinicianID",
                table: "VisitNotes",
                column: "ClinicianID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitNotes_PatientID",
                table: "VisitNotes",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID1",
                table: "Users",
                column: "RoleID1");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryPoints_DeviceID",
                table: "TelemetryPoints",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryPoints_PatientID",
                table: "TelemetryPoints",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserID",
                table: "Patients",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PatientId",
                table: "Medications",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PrescribedBy",
                table: "Medications",
                column: "PrescribedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_EnrolledBy",
                table: "Enrollments",
                column: "EnrolledBy");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_PatientID",
                table: "Enrollments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ProgramID",
                table: "Enrollments",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AssignedToPatientID",
                table: "Devices",
                column: "AssignedToPatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PatientUserID",
                table: "Claims",
                column: "PatientUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PayerID1",
                table: "Claims",
                column: "PayerID1");

            migrationBuilder.CreateIndex(
                name: "IX_Charges_PatientUserID",
                table: "Charges",
                column: "PatientUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CarePlans_PatientID",
                table: "CarePlans",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_PerformedByUserID",
                table: "AuditLogs",
                column: "PerformedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicianID",
                table: "Appointments",
                column: "ClinicianID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientID",
                table: "Appointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_PatientID",
                table: "Alerts",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_RuleID",
                table: "Alerts",
                column: "RuleID");

            migrationBuilder.CreateIndex(
                name: "IX_AdherenceRecords_MedID",
                table: "AdherenceRecords",
                column: "MedID");

            migrationBuilder.CreateIndex(
                name: "IX_AdherenceRecords_PatientID",
                table: "AdherenceRecords",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdherenceRecords_Medications_MedID",
                table: "AdherenceRecords",
                column: "MedID",
                principalTable: "Medications",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdherenceRecords_Patients_PatientID",
                table: "AdherenceRecords",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Patients_PatientID",
                table: "Alerts",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Rules_RuleID",
                table: "Alerts",
                column: "RuleID",
                principalTable: "Rules",
                principalColumn: "RuleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientID",
                table: "Appointments",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_ClinicianID",
                table: "Appointments",
                column: "ClinicianID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_PerformedByUserID",
                table: "AuditLogs",
                column: "PerformedByUserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarePlans_Patients_PatientID",
                table: "CarePlans",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Patients_PatientID",
                table: "Charges",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Users_PatientUserID",
                table: "Charges",
                column: "PatientUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Patients_PatientID",
                table: "Claims",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Payers_PayerID",
                table: "Claims",
                column: "PayerID",
                principalTable: "Payers",
                principalColumn: "PayerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Payers_PayerID1",
                table: "Claims",
                column: "PayerID1",
                principalTable: "Payers",
                principalColumn: "PayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Users_PatientUserID",
                table: "Claims",
                column: "PatientUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Patients_AssignedToPatientID",
                table: "Devices",
                column: "AssignedToPatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Patients_PatientID",
                table: "Enrollments",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Programs_ProgramID",
                table: "Enrollments",
                column: "ProgramID",
                principalTable: "Programs",
                principalColumn: "ProgramID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Users_EnrolledBy",
                table: "Enrollments",
                column: "EnrolledBy",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Patients_PatientId",
                table: "Medications",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Users_PrescribedBy",
                table: "Medications",
                column: "PrescribedBy",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Users_UserID",
                table: "Patients",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Claims_ClaimID",
                table: "Payments",
                column: "ClaimID",
                principalTable: "Claims",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TelemetryPoints_Devices_DeviceID",
                table: "TelemetryPoints",
                column: "DeviceID",
                principalTable: "Devices",
                principalColumn: "DeviceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TelemetryPoints_Patients_PatientID",
                table: "TelemetryPoints",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID1",
                table: "Users",
                column: "RoleID1",
                principalTable: "Roles",
                principalColumn: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitNotes_Appointments_AppID",
                table: "VisitNotes",
                column: "AppID",
                principalTable: "Appointments",
                principalColumn: "AppID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitNotes_Patients_PatientID",
                table: "VisitNotes",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitNotes_Users_ClinicianID",
                table: "VisitNotes",
                column: "ClinicianID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdherenceRecords_Medications_MedID",
                table: "AdherenceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AdherenceRecords_Patients_PatientID",
                table: "AdherenceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Patients_PatientID",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Rules_RuleID",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_ClinicianID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_PerformedByUserID",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CarePlans_Patients_PatientID",
                table: "CarePlans");

            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Patients_PatientID",
                table: "Charges");

            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Users_PatientUserID",
                table: "Charges");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Patients_PatientID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Payers_PayerID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Payers_PayerID1",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Users_PatientUserID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Patients_AssignedToPatientID",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Patients_PatientID",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Programs_ProgramID",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Users_EnrolledBy",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Patients_PatientId",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Users_PrescribedBy",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Users_UserID",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Claims_ClaimID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_TelemetryPoints_Devices_DeviceID",
                table: "TelemetryPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_TelemetryPoints_Patients_PatientID",
                table: "TelemetryPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID1",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitNotes_Appointments_AppID",
                table: "VisitNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitNotes_Patients_PatientID",
                table: "VisitNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitNotes_Users_ClinicianID",
                table: "VisitNotes");

            migrationBuilder.DropIndex(
                name: "IX_VisitNotes_AppID",
                table: "VisitNotes");

            migrationBuilder.DropIndex(
                name: "IX_VisitNotes_ClinicianID",
                table: "VisitNotes");

            migrationBuilder.DropIndex(
                name: "IX_VisitNotes_PatientID",
                table: "VisitNotes");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TelemetryPoints_DeviceID",
                table: "TelemetryPoints");

            migrationBuilder.DropIndex(
                name: "IX_TelemetryPoints_PatientID",
                table: "TelemetryPoints");

            migrationBuilder.DropIndex(
                name: "IX_Patients_UserID",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Medications_PatientId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_PrescribedBy",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_EnrolledBy",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_PatientID",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_ProgramID",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Devices_AssignedToPatientID",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Claims_PatientUserID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_PayerID1",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Charges_PatientUserID",
                table: "Charges");

            migrationBuilder.DropIndex(
                name: "IX_CarePlans_PatientID",
                table: "CarePlans");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_PerformedByUserID",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ClinicianID",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientID",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_PatientID",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_RuleID",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_AdherenceRecords_MedID",
                table: "AdherenceRecords");

            migrationBuilder.DropIndex(
                name: "IX_AdherenceRecords_PatientID",
                table: "AdherenceRecords");

            migrationBuilder.DropColumn(
                name: "RoleID1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PatientUserID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "PayerID1",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "PatientUserID",
                table: "Charges");

            migrationBuilder.DropColumn(
                name: "PerformedByUserID",
                table: "AuditLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Users_PatientID",
                table: "Charges",
                column: "PatientID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Payers_PayerID",
                table: "Claims",
                column: "PayerID",
                principalTable: "Payers",
                principalColumn: "PayerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Users_PatientID",
                table: "Claims",
                column: "PatientID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Claims_ClaimID",
                table: "Payments",
                column: "ClaimID",
                principalTable: "Claims",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
