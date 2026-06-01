using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class CarePlanRepository : ICarePlanRepository
    {
        private readonly AppDbContext _context;

        public CarePlanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarePlan>> GetAllCarePlansAsync(CarePlanSearchDTO searchDTO)
        {
            var query = _context.CarePlans.AsQueryable();

            if (searchDTO != null)
            {
                if (searchDTO.PatientID.HasValue)
                    query = query.Where(cp => cp.PatientID == searchDTO.PatientID.Value);

                if (searchDTO.ProgramID.HasValue)
                    query = query.Where(cp => cp.ProgramID == searchDTO.ProgramID.Value);

                if (searchDTO.Status.HasValue)
                    query = query.Where(cp => cp.Status == searchDTO.Status.Value);

                if (!string.IsNullOrEmpty(searchDTO.SearchText))
                    query = query.Where(cp => cp.PlanName.Contains(searchDTO.SearchText));

                int pageNumber = searchDTO.PageNumber <= 0 ? 1 : searchDTO.PageNumber;
                int pageSize = searchDTO.PageSize <= 0 ? 10 : searchDTO.PageSize;
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<CarePlan?> GetCarePlanByIdAsync(int id)
        {
            return await _context.CarePlans.FindAsync(id);
        }

        public async Task AddCarePlanAsync(CarePlan carePlan)
        {
            await _context.CarePlans.AddAsync(carePlan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarePlanAsync(CarePlan carePlan)
        {
            _context.CarePlans.Update(carePlan);
            await _context.SaveChangesAsync();
        }
    }
}