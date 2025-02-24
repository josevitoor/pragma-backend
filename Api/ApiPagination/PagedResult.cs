using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;
using TCE.Base.Repository._BaseRepository.Paging;

namespace Application.ApiPagination;

public class PagedResult<T> : IActionResult
{
    private readonly IPaginate<T> _page;

    public PagedResult(IPaginate<T> page)
    {
        _page = page;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var serverSide = context.HttpContext.Request.Query["serverSide"];

        ObjectResult result;
        if (!StringValues.IsNullOrEmpty(serverSide) && serverSide.First() == "true")
        {
            result = new ObjectResult(new
            {
                data = _page.Items,
                start = _page.Offset,
                lenght = _page.Size,
                recordsFiltered = _page.Items?.Count,
                recordsTotal = _page.Count
            });
        }
        else
        {
            result = new ObjectResult(_page.Items);
        }

        await result.ExecuteResultAsync(context);
    }
}
