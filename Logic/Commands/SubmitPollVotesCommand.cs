using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KrkPoll.Data;
using KrkPoll.Data.Dto;
using KrkPoll.Data.Models;
using KrkPoll.Services;
using KrkPoll.Utils.Result;
using Microsoft.EntityFrameworkCore;

namespace KrkPoll.Logic.Commands
{
    public class SubmitPollVotesCommand : ICommand
    {
        public int PollId { get; }
        public List<VoteDto> Votes { get; }

        public SubmitPollVotesCommand(List<VoteDto> votes, int pollId)
        {
            Votes = votes;
            PollId = pollId;
        }

        public sealed class SubmitPollVotesCommandHandler : ICommandHandler<SubmitPollVotesCommand>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IIdentityProvider _identityProvider;

            public SubmitPollVotesCommandHandler(ApplicationDbContext dbContext, IIdentityProvider identityProvider)
            {
                _dbContext = dbContext;
                _identityProvider = identityProvider;
            }

            public async Task<Result> Handle(SubmitPollVotesCommand command)
            {
                var userId = _identityProvider.GetUserIdFromClaims();

                var poll = await GetPoll(command.PollId, userId);
                if(poll == null) return Result.Fail("Poll doesn't exist");
                if(HasVotedBefore(poll)) return Result.Fail("User has already voted");

                var votes = new List<VotingHistory>();
                foreach (var vote in command.Votes)
                {
                    if (!IsReferenceValid(vote, poll)) return Result.Fail("Reference validation failed");
                    votes.AddRange(vote.AnswerIds.Select(id => CreateNewVote(id, vote, userId)));
                }

                _dbContext.VotingHistory.AddRange(votes);
                await _dbContext.SaveChangesAsync();
                return Result.Ok();
            }

            private VotingHistory CreateNewVote(int id, VoteDto vote, string userId) => 
                new VotingHistory {AnswerId = id, QuestionId = vote.QuestionId, ApplicationUserId = userId};


            private bool HasVotedBefore(Poll poll) => poll.Questions.Select(q => q.Voting).Any(v => v.Count > 0);

            private bool IsReferenceValid(VoteDto vote, Poll poll)
            {
                var question = poll.Questions.FirstOrDefault(q => q.Id == vote.QuestionId);

                if (question == null) return false;
                if (!question.MultipleChoice && vote.AnswerIds.Count > 1) return false;
                if (vote.AnswerIds.Except(question.Answers.Select(a => a.Id)).Any()) return false;

                return true;
            }

            private async Task<Poll> GetPoll(int id, string userId)
            {
                return await _dbContext.Polls
                    .AsNoTracking()
                    .Include(p => p.Questions)
                        .ThenInclude(p => p.Answers)
                    .Include(p => p.Questions)
                        .ThenInclude(q => q.Voting.Where(v => v.ApplicationUserId == userId))
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
        }
    }
}
