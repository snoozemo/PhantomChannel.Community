using AutoMapper;
using PhantomChannel.Community.Posts;
using PhantomChannel.Community.Comments;
namespace PhantomChannel.Community;

public class CommunityApplicationAutoMapperProfile : Profile
{
    public CommunityApplicationAutoMapperProfile()
    {

        CreateMap<Post, PostDto>();
        CreateMap<CreateUpdatePostDto, Post>();

        CreateMap<Comment, CommentDto>();
        CreateMap<CreateUpdateCommentDto, Comment>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
