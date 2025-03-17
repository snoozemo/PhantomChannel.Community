using AutoMapper;
using PhantomChannel.Community.Books;

namespace PhantomChannel.Community.Blazor.Client;

public class CommunityBlazorAutoMapperProfile : Profile
{
    public CommunityBlazorAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();

        //Define your AutoMapper configuration here for the Blazor project.
    }
}