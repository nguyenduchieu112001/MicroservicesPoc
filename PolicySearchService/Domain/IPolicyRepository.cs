using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicySearchService.Domain;

public interface IPolicyRepository
{
    Task Add(Policy policy);

    Task Delete(string policyNumber);

    Task<List<Policy>> Find(string queryText);
}