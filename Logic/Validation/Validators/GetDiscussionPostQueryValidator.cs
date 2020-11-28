using FluentValidation;
using KrkPoll.Logic.Queries;

namespace KrkPoll.Logic.Validation.Validators
{
    public class GetDiscussionPostQueryValidator : AbstractValidator<GetPollDiscussionPostsQuery>
    {
        public GetDiscussionPostQueryValidator()
        {
            RuleFor(x => x.LastId).GreaterThan(0).When(x => x.LastId.HasValue);
            RuleFor(x => x.PollId).GreaterThan(0);

        }
    }
}