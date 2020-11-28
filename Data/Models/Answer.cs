using System;
using System.Collections.Generic;

namespace KrkPoll.Data.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public List<VotingHistory> Voting { get; set; }
    }
}