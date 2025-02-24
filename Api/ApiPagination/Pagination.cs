using Microsoft.AspNetCore.Mvc;
using TCE.Base.Repository._BaseRepository.Paging;

namespace Application.ApiPagination;
public static class Pagination
{
    public static IActionResult Page<T>(IPaginate<T> page)
    {
        return new PagedResult<T>(page);
    }
}