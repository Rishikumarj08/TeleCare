namespace TeleCare.Service.Interface
{
    using TeleCare.DTO;
 
    public interface IRuleService
    {
        Task<List<RuleResponseDto>> GetAllRulesAsync();
        Task<RuleResponseDto> GetRuleByIdAsync(int ruleId);
        Task<List<RuleResponseDto>> SearchRulesAsync(SearchRuleDto searchDto);
        Task CreateRuleAsync(RuleCreateDto ruleDto);
        Task UpdateRuleAsync(int ruleId, RuleCreateDto ruleDto);
        Task DeleteRuleAsync(int ruleId);
    }
}
 
 