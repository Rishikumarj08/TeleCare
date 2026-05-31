namespace TeleCare.Constants
{
    public static class ApplicationMessages
    {
        public const string InvalidPatientReference = "Invalid patient reference number provided.";

        public const string AppointmentMustBeFuture = "Appointment must be scheduled for a future date and time.";

        public const string AppointmentTypeRequired = "Appointment type is required.";

        public const string AppointmentModeRequired = "Appointment mode is required.";
        public const string InvalidAppointmentId = "Invalid appointment identifier provided.";
        public const string AppointmentNotFound = "Requested appointment record was not found.";
        public const string AppointmentUpdateFutureOnly = "Updated appointment time must be in the future.";
        public const string CompletedAppointmentCannotBeModified = "Completed appointments cannot be modified.";

        public const string VisitNoteNotesRequired = "Visit note description is required.";
        public const string VisitNoteDiagnosisRequired = "Diagnosis information is required.";
        public const string VisitNoteOrdersRequired = "Orders information is required.";
        public const string InvalidVisitNoteId = "Invalid visit note identifier provided.";
        public const string VisitNoteNotFound = "Requested visit note record was not found.";
        public const string CompletedVisitNoteCannotBeModified = "Completed visit notes cannot be modified.";

        public const string AlertTypeRequired = "Alert type is required.";
        public const string AlertMessageRequired = "Alert message is required.";

        
        public const string InvalidAlertId = "Invalid alert identifier provided.";
        public const string AlertNotFound = "Requested alert record was not found.";

        public const string RequestBodyNull = "Request body cannot be null.";





    }
}