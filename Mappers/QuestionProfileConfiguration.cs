using AutoMapper;
using KrkPoll.Data.Dto;
using KrkPoll.Data.Models;

namespace KrkPoll.Mappers
{
    public class QuestionProfileConfiguration : Profile
    {
        public QuestionProfileConfiguration()
        {
            CreateMap<Question, QuestionViewDto>();
        }
    }
}