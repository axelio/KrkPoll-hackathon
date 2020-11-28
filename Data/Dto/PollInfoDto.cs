using System;
using System.Collections.Generic;

namespace KrkPoll.Data.Dto
{
    public class PollInfoDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class PollDetailsDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<QuestionViewDto> Questions { get; set; }
    }
}
