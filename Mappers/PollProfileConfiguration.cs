using AutoMapper;
using KrkPoll.Data.Dto;
using KrkPoll.Data.Models;

namespace KrkPoll.Mappers
{
    public class PollProfileConfiguration : Profile
    {
        public PollProfileConfiguration()
        {
            CreateMap<Poll, PollInfoDto>();
            CreateMap<Poll, PollDetailsDto>();

        }
    }
}
