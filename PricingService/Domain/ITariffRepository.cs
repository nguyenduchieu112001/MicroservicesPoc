using System.Threading.Tasks;

namespace PricingService.Domain;

public interface ITariffRepository
{
    Task<Tariff> this[string code] { get; }
    Task<Tariff> WithCode(string code);

    Task<Tariff> Add(Tariff tariff);

    Task<Tariff> Update(Tariff tariff);

    Task Delete(Tariff tariff);

    Task<bool> Exists(string code);
}