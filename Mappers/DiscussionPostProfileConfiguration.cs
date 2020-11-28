using AutoMapper;
using KrkPoll.Data.Dto;
using KrkPoll.Data.Models;

namespace KrkPoll.Mappers
{
    public class DiscussionPostProfileConfiguration : Profile
    {
        public DiscussionPostProfileConfiguration()
        {
            CreateMap<DiscussionPost, DiscussionPostDto>()
                .ForMember(p => p.UserName, opts => opts.MapFrom(dest => dest.Author.UserName));
        }
    }
}