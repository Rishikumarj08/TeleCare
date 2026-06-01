using System.ComponentModel.DataAnnotations;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class AdherenceService : IAdherenceService
    {
        private readonly IAdherenceRepository repository;

        public AdherenceService(IAdherenceRepository repository)
        {
            this.repository = repository;
        }

        public async Task<AdherenceResponseDto> createAdherenceRecordAsync(AdherenceCreateDto adherenceCreateDto)
        {
            var entity = new AdherenceRecordModel
            {
                MedID = adherenceCreateDto.MedID,
                PatientID = adherenceCreateDto.PatientID,
                TakenAt = adherenceCreateDto.TakenAt,
                Source = adherenceCreateDto.Source,
                Notes = adherenceCreateDto.Notes,
                Status = adherenceCreateDto.Status
            };

            Validate(entity);

            var result = await repository.createAdherenceRecordAsync(entity);
            return MapToDto(result);
        }

        public async Task<AdherenceResponseDto?> getAdherenceDetailsByAdhIDAsync(int adhId)
        {
            var entity = await repository.getAdherenceRecordByAdhIDAsync(adhId);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<AdherenceResponseDto?> updateAdherenceDetailsByAdhIDAsync(int adhId, AdherenceUpdateDto adherenceUpdateDto)
        {
            var entity = await repository.getAdherenceRecordByAdhIDAsync(adhId);
            if (entity == null) return null;

            entity.TakenAt = adherenceUpdateDto.TakenAt;
            entity.Notes = adherenceUpdateDto.Notes;
            entity.Status = adherenceUpdateDto.Status;

            Validate(entity);

            var updated = await repository.updateAdherenceRecordByAdhIDAsync(entity);
            return MapToDto(updated);
        }

        public async Task<List<AdherenceResponseDto>> getFilteredAdherenceRecordsAsync(AdherenceQueryDto adherenceQueryDto)
        {
            var data = await repository.getFilteredAdherenceRecordsAsync(adherenceQueryDto);
            return data.Select(MapToDto).ToList();
        }

        private void Validate(AdherenceRecordModel entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                throw new ArgumentException(results.First().ErrorMessage);
            }
        }

        private AdherenceResponseDto MapToDto(AdherenceRecordModel adherenceRecordModel) => new AdherenceResponseDto
        {
            AdhID = adherenceRecordModel.AdhID,
            MedID = adherenceRecordModel.MedID,
            PatientID = adherenceRecordModel.PatientID,
            TakenAt = adherenceRecordModel.TakenAt,
            Source = adherenceRecordModel.Source,
            Notes = adherenceRecordModel.Notes,
            Status = adherenceRecordModel.Status
        };
    }
}