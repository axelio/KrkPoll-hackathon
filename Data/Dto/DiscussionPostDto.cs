using System;

namespace KrkPoll.Data.Dto
{
    public class DiscussionPostDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string Post { get; set; }
    }
}