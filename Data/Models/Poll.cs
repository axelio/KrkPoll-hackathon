using System;
using System.Collections.Generic;

namespace KrkPoll.Data.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
        public List<DiscussionPost> DiscussionPosts { get; set; }
        //TODO poll can be created only by officials - role based authorization
        public ApplicationUser Author { get; set; }
        public string ApplicationUserId { get; set; }
    }
}