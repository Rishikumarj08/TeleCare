using System.ComponentModel.DataAnnotations;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;
using TeleCare.Enum;

namespace TeleCare.Service.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository repository;

        public PatientService(IPatientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PatientResponseDto> createPatientRecordAsync(PatientCreateDto patientCreateDto)
        {
            var entity = new PatientModel
            {
                UserID = patientCreateDto.UserID,
                MRN = patientCreateDto.MRN,
                Name = patientCreateDto.Name,
                DOB = patientCreateDto.DOB,
                Gender = patientCreateDto.Gender,
                Address = patientCreateDto.Address,
                ContactInfoJSON = patientCreateDto.ContactInfoJSON,
                PrimaryLanguage = patientCreateDto.PrimaryLanguage,
                EmergencyContactJSON = patientCreateDto.EmergencyContactJSON,
                ConsentStatus = patientCreateDto.ConsentStatus,
                EnrolledProgramsJSON = patientCreateDto.EnrolledProgramsJSON,
                Status = PatientStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            Validate(entity);

            var result = await repository.createPatientRecordAsync(entity);
            return MapToDto(result);
        }

        public async Task<PatientResponseDto?> getPatientDetailsByPatientIdAsync(int patientId)
        {
            var entity = await repository.getPatientRecordByPatientIdAsync(patientId);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<PatientResponseDto?> updatePatientDetailsByPatientIdAsync(int patientId, PatientUpdateDto patientUpdateDto)
        {
            var entity = await repository.getPatientRecordByPatientIdAsync(patientId);
            if (entity == null) return null;

            entity.Name = patientUpdateDto.Name;
            entity.Address = patientUpdateDto.Address;
            entity.ContactInfoJSON = patientUpdateDto.ContactInfoJSON;
            entity.EmergencyContactJSON = patientUpdateDto.EmergencyContactJSON;

            Validate(entity);

            var updated = await repository.updatePatientRecordByPatientIdAsync(entity);
            return MapToDto(updated);
        }

        public async Task<List<PatientResponseDto>> getFilteredPatientRecordsAsync(PatientQueryDto patientQueryDto)
        {
            var data = await repository.getFilteredPatientRecordsAsync(patientQueryDto);
            return data.Select(MapToDto).ToList();
        }

        private void Validate(PatientModel entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                throw new ArgumentException(results.First().ErrorMessage);
            }
        }

        private PatientResponseDto MapToDto(PatientModel patientResponseDto) => new PatientResponseDto
        {
            PatientID = patientResponseDto.PatientID,
            UserID = patientResponseDto.UserID,
            MRN = patientResponseDto.MRN,
            Name = patientResponseDto.Name,
            DOB = patientResponseDto.DOB,
            Gender = patientResponseDto.Gender,
            Address = patientResponseDto.Address,
            Status = patientResponseDto.Status,
            CreatedAt = patientResponseDto.CreatedAt,
            ContactInfoJSON = patientResponseDto.ContactInfoJSON,
            PrimaryLanguage = patientResponseDto.PrimaryLanguage,
            ConsentStatus = patientResponseDto.ConsentStatus,
            EmergencyContactJSON = patientResponseDto.EmergencyContactJSON,
            EnrolledProgramsJSON = patientResponseDto.EnrolledProgramsJSON
        };
    }
}