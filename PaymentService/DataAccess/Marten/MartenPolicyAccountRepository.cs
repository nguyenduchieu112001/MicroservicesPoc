using Marten;
using PaymentService.Domain;
using System;
using System.Threading.Tasks;

namespace PaymentService.DataAccess.Marten;

public class MartenPolicyAccountRepository : IPolicyAccountRepository
{
    private readonly IDocumentSession documentSession;

    public MartenPolicyAccountRepository(IDocumentSession documentSession)
    {
        this.documentSession = documentSession;
    }

    public void Add(PolicyAccount policyAccount)
    {
        try
        {
            documentSession.Insert(policyAccount);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Update(PolicyAccount policyAccount)
    {
        //documentSession.Update(policyAccount);
        try
        {
            documentSession.Update(policyAccount);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public async Task<PolicyAccount> FindByNumber(string policyNumber)
    {
        return await documentSession
            .Query<PolicyAccount>()
            .FirstOrDefaultAsync(p => p.PolicyNumber == policyNumber);
    }

    public async Task<bool> ExistsWithPolicyNumber(string policyNumber)
    {
        return await documentSession
            .Query<PolicyAccount>()
            .AnyAsync(p => p.PolicyNumber == policyNumber);
    }
}