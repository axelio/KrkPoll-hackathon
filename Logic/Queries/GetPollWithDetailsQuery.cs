using System.Threading.Tasks;
using AutoMapper;
using KrkPoll.Data;
using KrkPoll.Data.Dto;
using KrkPoll.Data.Models;
using KrkPoll.Utils.Result;
using Microsoft.EntityFrameworkCore;

namespace KrkPoll.Logic.Queries
{
    public class GetPollWithDetailsQuery : IQuery<PollDetailsDto>
    {
        public int Id { get; }

        public GetPollWithDetailsQuery(int id)
        {
            Id = id;
        }

        public class GetPollWithDetailsQueryHandler : IQueryHandler<GetPollWithDetailsQuery, PollDetailsDto>
        {
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _dbContext;

            public GetPollWithDetailsQueryHandler(IMapper mapper, ApplicationDbContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            public async Task<PollDetailsDto> Handle(GetPollWithDetailsQuery query)
            {
                var poll = await GetPollWithDetails(query.Id);
                
                return poll == null ? null : _mapper.Map<PollDetailsDto>(poll);
            }

            private async Task<Poll> GetPollWithDetails(int pollId)
            {
                return await _dbContext.Polls
                    .AsNoTracking()
                    .Include(p => p.Questions).ThenInclude(q => q.Answers)
                    .FirstOrDefaultAsync(p => p.Id == pollId);
            }
        }
    }
}