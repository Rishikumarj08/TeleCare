using System.ComponentModel.DataAnnotations;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class VisitNoteService : IVisitNoteService
    {
        private readonly IVisitNoteRepository repository;

        public VisitNoteService(IVisitNoteRepository repository)
        {
            this.repository = repository;
        }

        public async Task<VisitNoteDto?> createVisitNoteRecordAsync(VisitNoteDto dto)
        {
            var entity = new VisitNote
            {
                PatientReferenceNumber = dto.PatientReferenceNumber,
                Notes = dto.Notes,
                Diagnosis = dto.Diagnosis,
                Orders = dto.Orders,
                AttachmentName = dto.AttachmentName,
                VisitNoteStatus = dto.VisitNoteStatus,
                CreatedOn = DateTime.Now
            };

            Validate(entity);

            var result = await repository.createVisitNoteRecordAsync(entity);

            if (result != null)
            {
                dto.VisitNoteId = result.Id;
            }
            return dto;
        }

        public async Task<List<VisitNoteDto>> getAllVisitNoteRecordsAsync()
        {
            var data = await repository.getAllVisitNoteRecordsAsync();

            return data.Select(x => new VisitNoteDto
            {
                VisitNoteId = x.Id,
                PatientReferenceNumber = x.PatientReferenceNumber,
                Notes = x.Notes,
                Diagnosis = x.Diagnosis,
                Orders = x.Orders,
                AttachmentName = x.AttachmentName,
                VisitNoteStatus = x.VisitNoteStatus
            }).ToList();
        }

        public async Task<VisitNoteDto?> getVisitNoteDetailsByVisitNoteIdAsync(int id)
        {
            var entity = await repository.getVisitNoteRecordByVisitNoteIdAsync(id);

            return entity == null ? null : new VisitNoteDto
            {
                VisitNoteId = entity.Id,
                PatientReferenceNumber = entity.PatientReferenceNumber,
                Notes = entity.Notes,
                Diagnosis = entity.Diagnosis,
                Orders = entity.Orders,
                AttachmentName = entity.AttachmentName,
                VisitNoteStatus = entity.VisitNoteStatus
            };
        }

        public async Task<VisitNoteDto?> updateVisitNoteDetailsByVisitNoteIdAsync(int id, VisitNoteDto dto)
        {
            var entity = await repository.getVisitNoteRecordByVisitNoteIdAsync(id);

            if (entity == null) return null;

            entity = ApplyUpdate(entity, dto);

            var updated = await repository.updateVisitNoteRecordByVisitNoteIdAsync(entity);
            return updated == null ? null : Map(updated);
        }

        public async Task<List<VisitNoteDto>> getFilteredVisitNoteRecordsAsync(VisitNoteQueryDto query)
        {
            var data = await repository.getFilteredVisitNoteRecordsAsync(query);

            return data.Select(x => new VisitNoteDto
            {
                VisitNoteId = x.Id,
                PatientReferenceNumber = x.PatientReferenceNumber,
                Notes = x.Notes,
                Diagnosis = x.Diagnosis,
                Orders = x.Orders,
                AttachmentName = x.AttachmentName,
                VisitNoteStatus = x.VisitNoteStatus
            }).ToList();
        }

        private VisitNote ApplyUpdate(VisitNote entity, VisitNoteDto dto)
        {
            entity.Notes = dto.Notes;
            entity.Diagnosis = dto.Diagnosis;
            entity.Orders = dto.Orders;
            entity.AttachmentName = dto.AttachmentName;
            entity.VisitNoteStatus = dto.VisitNoteStatus;

            Validate(entity);

            return entity;
        }

        private VisitNoteDto Map(VisitNote entity)
        {
            return new VisitNoteDto
            {
                VisitNoteId = entity.Id,
                PatientReferenceNumber = entity.PatientReferenceNumber,
                Notes = entity.Notes,
                Diagnosis = entity.Diagnosis,
                Orders = entity.Orders,
                AttachmentName = entity.AttachmentName,
                VisitNoteStatus = entity.VisitNoteStatus
            };
        }

        private void Validate(VisitNote entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                throw new Exception(results.First().ErrorMessage);
            }
        }
    }
}