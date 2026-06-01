namespace TeleCare.Service.Implementation

{

    using TeleCare.Constants;

    using TeleCare.DTO;

    using TeleCare.Exceptions;

    using TeleCare.Model;

    using TeleCare.Repository.Interface;

    using TeleCare.Service.Interface;
 
    public class RuleService : IRuleService

    {

        private readonly IRuleRepository _ruleRepository;
 
        public RuleService(IRuleRepository ruleRepository)

        {

            _ruleRepository = ruleRepository;

        }
 
        public async Task<List<RuleResponseDto>> GetAllRulesAsync()

        {

            var rules = await _ruleRepository.GetAllRulesAsync();

            return rules.Select(Map).ToList();

        }
 
        public async Task<RuleResponseDto> GetRuleByIdAsync(int ruleId)

        {

            var rule = await _ruleRepository.GetRuleByIdAsync(ruleId);

            if (rule == null)

                throw new NotFoundException(AppConstants.RuleNotFound);

            return Map(rule);

        }
 
        public async Task<List<RuleResponseDto>> SearchRulesAsync(SearchRuleDto searchDto)

        {

            var rules = await _ruleRepository.SearchRulesAsync(searchDto);

            if (rules == null || rules.Count == 0)

                throw new NotFoundException(AppConstants.NoRulesFound);

            return rules.Select(Map).ToList();

        }
 
        public async Task CreateRuleAsync(RuleCreateDto ruleDto)

        {

            if (string.IsNullOrWhiteSpace(ruleDto.Name))

                throw new BadRequestException(AppConstants.RuleNameRequired);
 
            var rule = new Rule

            {

                Name = ruleDto.Name,

                Description = ruleDto.Description,

                ActiveFrom = ruleDto.ActiveFrom,

                ActiveTo = ruleDto.ActiveTo,

                Status = ruleDto.Status

            };
 
            await _ruleRepository.AddRuleAsync(rule);

        }
 
        public async Task UpdateRuleAsync(int ruleId, RuleCreateDto ruleDto)

        {

            var rule = await _ruleRepository.GetRuleByIdAsync(ruleId);

            if (rule == null)

                throw new NotFoundException(AppConstants.RuleNotFound);
 
            if (string.IsNullOrWhiteSpace(ruleDto.Name))

                throw new BadRequestException(AppConstants.RuleNameRequired);
 
            rule.Name = ruleDto.Name;

            rule.Description = ruleDto.Description;

            rule.ActiveFrom = ruleDto.ActiveFrom;

            rule.ActiveTo = ruleDto.ActiveTo;

            rule.Status = ruleDto.Status;
 
            await _ruleRepository.UpdateRuleAsync(rule);

        }
 
        public async Task DeleteRuleAsync(int ruleId)

        {

            var rule = await _ruleRepository.GetRuleByIdAsync(ruleId);

            if (rule == null)

                throw new NotFoundException(AppConstants.RuleNotFound);

            await _ruleRepository.DeleteRuleAsync(rule);

        }
 
        private static RuleResponseDto Map(Rule rule) => new()

        {

            RuleID = rule.RuleID,

            Name = rule.Name,

            Description = rule.Description,

            ActiveFrom = rule.ActiveFrom,

            ActiveTo = rule.ActiveTo,

            Status = rule.Status

        };

    }

}
 