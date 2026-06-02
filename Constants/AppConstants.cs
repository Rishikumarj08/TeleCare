namespace TeleCare.Constants
{
    public static class AppConstants
    {
        // General
        public const string RecordCreated = "Record created successfully.";
        public const string RecordUpdated = "Record updated successfully.";
        public const string RecordDeleted = "Record deleted successfully.";
        public const string RecordNotFound = "Record not found.";
        public const string InvalidRole = "Invalid role selected.";
        public const string NoUsersFound = "No users found.";
        public const string InvalidCredentials = "Invalid credentials provided.";
        public const string ClinicianPinRequired = "Clinician accounts require PIN verification.";
        public const string PatientForgotPasswordNotSupported = "Forgot password is supported only for patient accounts.";
        public const string PasswordResetTokenInvalid = "Password reset token is invalid or expired.";
        public const string EmailAlreadyRegistered = "An account already exists with this email.";
        public const string RegistrationRestricted = "Registration is permitted only for patient or administrator roles.";

 
        // Rule
        public const string NoRulesFound = "No rules found.";
        public const string RuleNotFound = "Rule not found.";
        public const string RuleNameRequired = "Rule name is required.";
 
        // Payer
        public const string NoPayersFound = "No payers found.";
        public const string PayerNotFound = "Payer not found.";
 
        // Claim
        public const string NoClaimsFound = "No claims found.";
        public const string ClaimNotFound = "Claim not found.";
        public const string PatientNotFound = "Patient not found.";

 
        // Payment
        public const string NoPaymentsFound = "No payments found.";
        public const string PaymentNotFound = "Payment not found.";
        public const string ClaimNotFoundForPayment = "The referenced claim does not exist.";
 
        // Charge
        public const string NoChargesFound = "No charges found.";
        public const string ChargeNotFound = "Charge not found.";
 
        // Notification
        public const string NoNotificationsFound = "No notifications found.";
        public const string NotificationNotFound = "Notification not found.";
        public const string RecipientUserNotFound = "The recipient user does not exist.";

        // Audit Log
        public const string NoAuditLogsFound = "No audit logs found.";
        public const string AuditLogNotFound = "Audit log not found.";

        // Patient Visits
        public const string NoVisitNotesFound = "No patient visits found.";

        // KPI
        public const string NoKpisFound = "No KPIs found.";
        public const string KpiNotFound = "KPI not found.";
        public const string KpiNameRequired = "KPI name is required.";
        public const string KpiReportingPeriodRequired = "Reporting period is required.";
        public const string KpiTargetValueInvalid = "Target value must be between 0 and 100.";
    }
}
