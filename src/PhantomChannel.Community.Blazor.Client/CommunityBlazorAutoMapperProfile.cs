using AutoMapper;
using PhantomChannel.Community.Posts;

namespace PhantomChannel.Community.Blazor.Client;

public class CommunityBlazorAutoMapperProfile : Profile
{
    public CommunityBlazorAutoMapperProfile()
    {
        CreateMap<PostDto, CreateUpdatePostDto>();
        //Define your AutoMapper configuration here for the Blazor project.
    }
}