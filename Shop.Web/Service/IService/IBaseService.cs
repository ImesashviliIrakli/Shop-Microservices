using Shop.Web.Models;

namespace Shop.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
