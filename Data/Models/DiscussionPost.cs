using System;

namespace KrkPoll.Data.Models
{
    public class DiscussionPost
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUser Author { get; set; }
        public string ApplicationUserId { get; set; }
        public Poll Poll { get; set; }
        public int PollId { get; set; }
        public string Post { get; set; }
    }
}