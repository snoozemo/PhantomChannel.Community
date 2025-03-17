using AutoMapper;
using PhantomChannel.Community.Books;

namespace PhantomChannel.Community;

public class CommunityApplicationAutoMapperProfile : Profile
{
    public CommunityApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
