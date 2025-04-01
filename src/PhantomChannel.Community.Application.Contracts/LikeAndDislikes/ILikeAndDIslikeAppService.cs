using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PhantomChannel.Community.LikeAndDislikes;

public interface ILikeAndDislikeAppService :
 IDeleteAppService<Guid>
//TODO 缺失增加
{
}