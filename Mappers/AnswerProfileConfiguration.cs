using AutoMapper;
using KrkPoll.Data.Dto;
using KrkPoll.Data.Models;

namespace KrkPoll.Mappers
{
    public class AnswerProfileConfiguration : Profile
    {
        public AnswerProfileConfiguration()
        {
            CreateMap<Answer, AnswerViewDto>();
        }
    }
}