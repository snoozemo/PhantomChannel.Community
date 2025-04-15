
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Posts;

public class PostPagedResultRequestDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; } = "";

}