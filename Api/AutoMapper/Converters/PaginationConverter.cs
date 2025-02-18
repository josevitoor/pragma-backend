using AutoMapper;
using System.Collections.Generic;
using TCE.Base.Repository._BaseRepository.Paging;

namespace Application.AutoMapper.Converters
{
    public class PaginateConverter<TSource, TDest>
    : ITypeConverter<IPaginate<TSource>, IPaginate<TDest>> where TSource : class where TDest : class
    {
        public IPaginate<TDest> Convert(IPaginate<TSource> source, IPaginate<TDest> destination, ResolutionContext context)
        {
            var sourceItems = source.Items;
            destination = Paginate.From(source, (sourceItems) =>
            {
                return context.Mapper.Map<List<TDest>>(sourceItems);
            });
            return destination;
        }
    }
}
