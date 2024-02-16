using Marten;
using PricingService.Domain;
using System.Threading.Tasks;

namespace PricingService.DataAccess.Marten;

public class MartenTariffRepository : ITariffRepository
{
    private readonly IDocumentSession session;

    public MartenTariffRepository(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<Tariff> Add(Tariff tariff)
    {
        session.Insert(tariff);
        return tariff;
    }


    public async Task<bool> Exists(string code)
    {
        return await session.Query<Tariff>().AnyAsync(t => t.Code == code);
    }


    public async Task<Tariff> WithCode(string code)
    {
        return await session.Query<Tariff>()
            .FirstOrDefaultAsync(t => t.Code.Equals(code.ToUpper()));
    }

    public async Task<Tariff> Update(Tariff tariff)
    {
        session.Update(tariff);
        return tariff;
    }

    public async Task Delete(Tariff tariff)
    {
        session?.Delete(tariff);
    }

    public Task<Tariff> this[string code] => WithCode(code);
}