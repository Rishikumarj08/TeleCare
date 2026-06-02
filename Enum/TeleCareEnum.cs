namespace TeleCare.Enum
{
    public enum RoleEnum
    {
        Patient = 1,
        Clinician = 2,
        CareCoordinator = 3,
        DeviceTechnician = 4,
        Administrator = 5,
        Auditor = 6
    }

    public enum RuleStatus
    {
        Active,
        Inactive,
        Draft
    }

    public enum ClaimStatus
    {
        Pending,
        Submitted,
        Approved,
        Rejected,
        Paid
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentMethod
    {
        UPI,
        BankTransfer,
        CreditCard,
        DebitCard,
        Cash,
        Cheque
    }

    public enum ChargeStatus
    {
        Pending,
        Billed,
        Paid,
        Waived,
        Overdue
    }

    public enum NotificationStatus
    {
        Unread,
        Read
    }

    public enum NotificationCategory
    {
        Alert,
        Reminder,
        Update,
        System,
        Billing
    }

    public enum AuditAction
    {
        CREATE,
        UPDATE,
        DELETE,
        VIEW
    }

    public enum AuditResourceType
    {
        User,
        Rule,
        Claim,
        Payment,
        Charge,
        Notification,
        Patient,
        Enrollment,
        Device,
        Telemetry,
        Appointment,
        VisitNote,
        Alert,
        CarePlan,
        Medication,
        AdherenceRecord,
        Program
    }

    public enum KpiPerformanceIndicator
    {
        Exceeded,
        OnTrack,
        BelowTarget
    }
}
