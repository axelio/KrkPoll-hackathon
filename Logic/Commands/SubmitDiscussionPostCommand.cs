using System.Threading.Tasks;
using KrkPoll.Data;
using KrkPoll.Data.Models;
using KrkPoll.Logic.Validation;
using KrkPoll.Logic.Validation.Validators;
using KrkPoll.Services;
using KrkPoll.Utils.Result;

namespace KrkPoll.Logic.Commands
{
    public class SubmitDiscussionPostCommand : ICommand
    {
        public string Post { get; }
        public int PollId { get; }

        public SubmitDiscussionPostCommand(int pollId, string post)
        {
            PollId = pollId;
            Post = post;
        }

        public sealed class SubmitDiscussionPostCommandHandler : ICommandHandler<SubmitDiscussionPostCommand>
        {
            private readonly IIdentityProvider _identityProvider;
            private readonly IValidationService _validationService;
            private readonly ApplicationDbContext _dbContext;

            public SubmitDiscussionPostCommandHandler(IIdentityProvider identityProvider, IValidationService validationService, ApplicationDbContext dbContext)
            {
                _identityProvider = identityProvider;
                _validationService = validationService;
                _dbContext = dbContext;
            }

            public async Task<Result> Handle(SubmitDiscussionPostCommand command)
            {
                var validationResult = await _validationService.ValidateRulesAsync(new SubmitDiscussionPostCommandValidator(), command);
                if (validationResult.IsFailure) return Result.Fail(validationResult.Error);

                var userId = _identityProvider.GetUserIdFromClaims();
                var post = new DiscussionPost
                {
                    PollId = command.PollId,
                    ApplicationUserId = userId,
                    Post = command.Post
                };

                _dbContext.DiscussionPosts.Add(post);
                await _dbContext.SaveChangesAsync();

                return Result.Ok();

            }
        }
    }
}