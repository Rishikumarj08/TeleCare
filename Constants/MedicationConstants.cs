namespace TeleCare.Constants
{
    /// Centralized API response messages and constants.
    /// This ensures consistent messaging across all endpoints.
  
    public static class ApiConstants
    {
        // Success Messages
        public const string Success = "Operation completed successfully";
        public const string CreatedSuccessfully = "Resource created successfully";
        public const string UpdatedSuccessfully = "Resource updated successfully";
        public const string DeletedSuccessfully = "Resource deleted successfully";

        // Error Messages
        public const string NotFound = "Requested resource was not found";
        public const string InvalidRequest = "Invalid request data. Please check your input and try again";
        public const string InternalServerError = "An internal server error occurred. Please try again later";
        public const string UnauthorizedAccess = "You are not authorized to perform this action";
        public const string ConflictError = "A conflict occurred while processing your request";
        public const string ValidationError = "One or more validation errors occurred";

        // Medication-Specific Messages
        public const string MedicationNotFound = "Medication record not found";
        public const string MedicationCreatedSuccessfully = "Medication created successfully";
        public const string MedicationUpdatedSuccessfully = "Medication updated successfully";
        public const string InvalidMedicationData = "Invalid medication data provided";

        
    }
} 