using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using KrkPoll.Data.Dto;
using KrkPoll.Logic.Commands;
using KrkPoll.Logic.Queries;
using KrkPoll.Utils.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KrkPoll.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PollController : ControllerBase
    {
        private readonly IQueryHandler<GetActivePollsQuery, Result<List<PollInfoDto>>> _getPollsHandler;
        private readonly IQueryHandler<GetPollWithDetailsQuery, PollDetailsDto> _getPollDetailsQueryHandler;
        private readonly ICommandHandler<SubmitPollVotesCommand> _submitCommandHandler;


        public PollController(
            IQueryHandler<GetActivePollsQuery, Result<List<PollInfoDto>>> getPollsHandler,
            IQueryHandler<GetPollWithDetailsQuery, PollDetailsDto> getPollDetailsQueryHandler,
            ICommandHandler<SubmitPollVotesCommand> submitCommandHandler)
        {
            _getPollsHandler = getPollsHandler;
            _getPollDetailsQueryHandler = getPollDetailsQueryHandler;
            _submitCommandHandler = submitCommandHandler;
        }

        /// <summary>
        /// Get active polls with no details attached.
        /// </summary>
        /// <param name="lastId">Optional last id of poll. This is used for pagination. </param>
        /// <returns></returns>
        [HttpGet]
        [Route("active-list")]
        public async Task<IActionResult> GetActivePolls([FromQuery] int? lastId)
        {
            var query = new GetActivePollsQuery(lastId);
            var result = await _getPollsHandler.Handle(query);
            
            if (result.IsFailure)
            {
                //log error
                return BadRequest(result.Error);
            }

            if (result.Value.IsNullOrEmpty()) return NoContent();

            return Ok(result.Value);
        }

        /// <summary>
        /// Get specific poll with questions and answers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPoll(int id)
        {
            var query = new GetPollWithDetailsQuery(id);
            var result = await _getPollDetailsQueryHandler.Handle(query);
            if (result == null) return NoContent();
            return Ok(result);
        }

        /// <summary>
        /// submit votes for the poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="votes"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("submit-votes/{id}")]
        public async Task<IActionResult> SubmitVotes(int id, [FromBody] List<VoteDto> votes)
        {
            var command = new SubmitPollVotesCommand(votes, id);
            var result = await _submitCommandHandler.Handle(command);

            if (result.IsFailure)
            {
                //log
                return BadRequest(result.Error);
            }

            return NoContent();
        }
    }
}