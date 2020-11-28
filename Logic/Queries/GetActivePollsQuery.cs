using System;
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
    public class GetActivePollsQuery : IQuery<Result<List<PollInfoDto>>>
    {
        public int? LastId { get; }

        public GetActivePollsQuery(int? lastId)
        {
            LastId = lastId;
        }

        public sealed class GetActivePollsQueryHandler :IQueryHandler<GetActivePollsQuery, Result<List<PollInfoDto>>>
        {
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _dbContext;
            private readonly IValidationService _validationService;

            public GetActivePollsQueryHandler(IMapper mapper, ApplicationDbContext dbContext, IValidationService validationService)
            {
                _mapper = mapper;
                _dbContext = dbContext;
                _validationService = validationService;
            }

            public async Task<Result<List<PollInfoDto>>> Handle(GetActivePollsQuery query)
            {
                var validationResult = await _validationService.ValidateRulesAsync(new GetPollsQueryValidator(), query);
                if (validationResult.IsFailure) return Result.Fail<List<PollInfoDto>>(validationResult.Error);

                var polls = await GetPolls(query);

                return Result.Ok(_mapper.Map<List<PollInfoDto>>(polls));
            }

            private async Task<List<Poll>> GetPolls(GetActivePollsQuery query)
            {
                return await _dbContext.Polls
                    .AsNoTracking()
                    .Where(p => p.ExpirationDate > DateTime.UtcNow)
                    .TryApplyLastIdCondition(query.LastId)
                    .Take(20)
                    .ToListAsync();
            }
        }
    }
}
