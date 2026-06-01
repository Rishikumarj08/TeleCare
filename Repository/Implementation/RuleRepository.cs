namespace TeleCare.Repository.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Data;
    using TeleCare.DTO;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
 
    public class RuleRepository : IRuleRepository
    {
        private readonly AppDbContext _context;
 
        public RuleRepository(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task<List<Rule>> GetAllRulesAsync()
        {
            return await _context.Rules.ToListAsync();
        }
 
        public async Task<Rule?> GetRuleByIdAsync(int ruleId)
        {
            return await _context.Rules.FirstOrDefaultAsync(r => r.RuleID == ruleId);
        }
 
        public async Task<List<Rule>> SearchRulesAsync(SearchRuleDto searchDto)
        {
            var query = _context.Rules.AsQueryable();
 
            if (searchDto.RuleID.HasValue)
                query = query.Where(r => r.RuleID == searchDto.RuleID.Value);
 
            if (!string.IsNullOrWhiteSpace(searchDto.Name))
                query = query.Where(r => r.Name.Contains(searchDto.Name));
 
            if (!string.IsNullOrWhiteSpace(searchDto.Status))
                query = query.Where(r => r.Status.ToLower() == searchDto.Status.Trim().ToLower());
 
            if (searchDto.ActiveFrom.HasValue)
                query = query.Where(r => r.ActiveFrom >= searchDto.ActiveFrom.Value);
 
            if (searchDto.ActiveTo.HasValue)
                query = query.Where(r => r.ActiveTo <= searchDto.ActiveTo.Value);
 
            return await query.ToListAsync();
        }
 
        public async Task AddRuleAsync(Rule rule)
        {
            await _context.Rules.AddAsync(rule);
            await _context.SaveChangesAsync();
        }
 
        public async Task UpdateRuleAsync(Rule rule)
        {
            _context.Rules.Update(rule);
            await _context.SaveChangesAsync();
        }
 
        public async Task DeleteRuleAsync(Rule rule)
        {
            _context.Rules.Remove(rule);
            await _context.SaveChangesAsync();
        }
    }
}
 
 