namespace TeleCare.Repository.Interface
{
    using TeleCare.DTO;
    using TeleCare.Model;
 
    public interface IRuleRepository
    {
        Task<List<Rule>> GetAllRulesAsync();
        Task<Rule?> GetRuleByIdAsync(int ruleId);
        Task<List<Rule>> SearchRulesAsync(SearchRuleDto searchDto);
        Task AddRuleAsync(Rule rule);
        Task UpdateRuleAsync(Rule rule);
        Task DeleteRuleAsync(Rule rule);
    }
}
 
 