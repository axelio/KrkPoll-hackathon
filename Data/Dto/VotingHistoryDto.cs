using System.Collections.Generic;

namespace KrkPoll.Data.Dto
{
    public class VoteDto
    {
        public List<int> AnswerIds { get; set; }
        public int QuestionId { get; set; }
    }
}