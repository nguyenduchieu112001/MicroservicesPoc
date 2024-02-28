using Marten;
using PaymentService.Domain;
using System;
using System.Threading.Tasks;

namespace PaymentService.DataAccess.Marten;

public class MartenDataStore : IDataStore
{
    private readonly IDocumentSession session;

    public MartenDataStore(IDocumentSession documentSession, IPolicyAccountRepository policyAccountRepository)
    {
        this.session = documentSession;
        PolicyAccounts = policyAccountRepository;
    }

    public IPolicyAccountRepository PolicyAccounts { get; }

    public async Task CommitChanges()
    {
        try
        {
            await session.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Dispose()
    {
    }
}