using Marten;
using PaymentService.Domain;
using System;
using System.Threading.Tasks;

namespace PaymentService.DataAccess.Marten;

public class MartenDataStore : IDataStore
{
    private readonly IDocumentSession session;

    public MartenDataStore(IDocumentStore documentStore)
    {
        session = documentStore.LightweightSession();
        PolicyAccounts = new MartenPolicyAccountRepository(session);
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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) session.Dispose();
    }
}