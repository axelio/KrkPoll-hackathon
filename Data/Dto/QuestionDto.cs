using System.Collections.Generic;

namespace KrkPoll.Data.Dto
{
    public class QuestionViewDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool MultipleChoice { get; set; }
        public List<AnswerViewDto> Answers { get; set; }
    }
}