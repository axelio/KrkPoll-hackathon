using FluentValidation;
using KrkPoll.Logic.Commands;

namespace KrkPoll.Logic.Validation.Validators
{
    public class SubmitDiscussionPostCommandValidator : AbstractValidator<SubmitDiscussionPostCommand>
    {
        public SubmitDiscussionPostCommandValidator()
        {
            RuleFor(x => x.Post).MaximumLength(4000);
            RuleFor(x => x.PollId).GreaterThan(0);
        }
    }
}