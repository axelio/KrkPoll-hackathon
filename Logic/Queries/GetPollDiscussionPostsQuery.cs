using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KrkPoll.Data;
using KrkPoll.Data.Dto;
using KrkPoll.Data.Models;
using KrkPoll.Extensions;
using KrkPoll.Logic.Validation;
using KrkPoll.Logic.Validation.Validators;
using KrkPoll.Utils.Result;
using Microsoft.EntityFrameworkCore;

namespace KrkPoll.Logic.Queries
{
    public class GetPollDiscussionPostsQuery : IQuery<Result<List<DiscussionPostDto>>>
    {
        public int? LastId { get; }
        public int PollId { get; }

        public GetPollDiscussionPostsQuery(int? lastId, int pollId)
        {
            LastId = lastId;
            PollId = pollId;
        }

        public sealed class GetPollDiscussionPostsQueryHandler : IQueryHandler<GetPollDiscussionPostsQuery, Result<List<DiscussionPostDto>>>
        {
            private readonly IMapper _mapper;
            private readonly IValidationService _validationService;
            private readonly ApplicationDbContext _dbContext;

            public GetPollDiscussionPostsQueryHandler(IMapper mapper, IValidationService validationService, ApplicationDbContext dbContext)
            {
                _mapper = mapper;
                _validationService = validationService;
                _dbContext = dbContext;
            }

            public async Task<Result<List<DiscussionPostDto>>> Handle(GetPollDiscussionPostsQuery query)
            {
                var validationResult = await _validationService.ValidateRulesAsync(new GetDiscussionPostQueryValidator(), query);
                if (validationResult.IsFailure) return Result.Fail<List<DiscussionPostDto>>(validationResult.Error);

                var posts = await GetPosts(query);

                return Result.Ok(_mapper.Map<List<DiscussionPostDto>>(posts));
            }

            private async Task<List<DiscussionPost>> GetPosts(GetPollDiscussionPostsQuery query)
            {
                return await _dbContext.DiscussionPosts
                    .Where(dp => dp.PollId == query.PollId)
                    .AsNoTracking()
                    .TryApplyLastIdCondition(query.LastId)
                    .Take(20)
                    .ToListAsync();
            }
        }
    }
}