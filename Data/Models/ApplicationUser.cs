using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace KrkPoll.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Poll> Polls { get; set; }
        public List<DiscussionPost> DiscussionPosts { get; set; }
        public List<VotingHistory> Voting { get; set; }
    }
}
