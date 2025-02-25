using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;

namespace Services;
public class InformationService : BaseService<Information>, IInformationService
{
    public InformationService(IUnitOfWork uow) : base(uow)
    {
    }
}
