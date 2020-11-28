using System.Collections.Generic;
using System.Threading.Tasks;
using KrkPoll.Data.Dto;
using KrkPoll.Extensions;
using KrkPoll.Logic.Commands;
using KrkPoll.Logic.Queries;
using KrkPoll.Utils.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KrkPoll.Controllers
{
    [Authorize]
    [ApiController]
    [Route("controller")]
    public class DiscussionController : ControllerBase
    {
        private readonly ICommandHandler<SubmitDiscussionPostCommand> _submitPostHandler;
        private readonly IQueryHandler<GetPollDiscussionPostsQuery, Result<List<DiscussionPostDto>>> _discussionQueryHandler;

        public DiscussionController(
            ICommandHandler<SubmitDiscussionPostCommand> submitPostHandler,
            IQueryHandler<GetPollDiscussionPostsQuery, Result<List<DiscussionPostDto>>> discussionQueryHandler)
        {
            _submitPostHandler = submitPostHandler;
            _discussionQueryHandler = discussionQueryHandler;
        }

        /// <summary>
        /// add post to the discussion
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("submit/{pollId}")]
        public async Task<IActionResult> SubmitPost(int pollId, [FromBody] string post)
        {
            var command = new SubmitDiscussionPostCommand(pollId, post);
            var result = await _submitPostHandler.Handle(command);
            if (result.IsFailure)
            {
                //log
                return BadRequest(result.Error);
            }

            return Ok();
        }

        /// <summary>
        /// Get posts for specific poll
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pollId}")]
        public async Task<IActionResult> GetPosts(int pollId, [FromQuery] int? lastId)
        {
            var query = new GetPollDiscussionPostsQuery(lastId, pollId);
            var result = await _discussionQueryHandler.Handle(query);

            if (result.IsFailure)
            {
                //log error
                return BadRequest(result.Error);
            }

            if (result.Value.IsNullOrEmpty()) return NoContent();

            return Ok(result.Value);
        }
    }
}