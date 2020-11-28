using FluentValidation;
using KrkPoll.Logic.Queries;

namespace KrkPoll.Logic.Validation.Validators
{
    public class GetPollsQueryValidator : AbstractValidator<GetActivePollsQuery>
    {
        public GetPollsQueryValidator()
        {
            RuleFor(x => x.LastId).GreaterThan(0).When(x => x.LastId.HasValue);
        }
    }
}
