using System;

namespace KrkPoll.Data.Models
{
    public class VotingHistory
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }
        public int AnswerId { get; set; }
    }
}