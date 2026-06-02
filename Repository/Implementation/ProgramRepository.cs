using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.Model;
using TeleCare.DTO;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly AppDbContext _context;

        public ProgramRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProgramModel>> GetAllProgramsAsync(ProgramSearchDTO searchDTO)
        {
            var query = _context.Programs.AsNoTracking().AsQueryable();

            if (searchDTO != null)
            {
                if (searchDTO.ProgramID.HasValue)
                    query = query.Where(p => p.ProgramID == searchDTO.ProgramID.Value);

                if (searchDTO.Status.HasValue)
                    query = query.Where(p => p.Status == searchDTO.Status.Value);

                if (searchDTO.PatientID.HasValue)
                {
                    query = query.Where(p =>
                        _context.CarePlans.Any(cp =>
                            cp.ProgramID == p.ProgramID &&
                            cp.PatientID == searchDTO.PatientID.Value));
                }

                if (!string.IsNullOrWhiteSpace(searchDTO.SearchText))
                {
                    query = query.Where(p =>
                        p.ProgramName.Contains(searchDTO.SearchText));
                }

                int pageNumber = searchDTO.PageNumber <= 0 ? 1 : searchDTO.PageNumber;
                int pageSize = searchDTO.PageSize <= 0 ? 10 : searchDTO.PageSize;

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<ProgramModel?> GetProgramByIdAsync(int programId)
        {
            return await _context.Programs.FindAsync(programId);
        }

        public async Task AddProgramAsync(ProgramModel programModel)
        {
            await _context.Programs.AddAsync(programModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProgramAsync(ProgramModel programModel)
        {
            _context.Programs.Update(programModel);
            await _context.SaveChangesAsync();
        }
    }
}