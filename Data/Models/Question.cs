using System;
using System.Collections.Generic;

namespace KrkPoll.Data.Models
{
    public class Question
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public bool MultipleChoice { get; set; }
        public Poll Poll { get; set; }
        public int PollId { get; set; }
        public List<Answer> Answers { get; set; }
        public List<VotingHistory> Voting { get; set; }
    }
}