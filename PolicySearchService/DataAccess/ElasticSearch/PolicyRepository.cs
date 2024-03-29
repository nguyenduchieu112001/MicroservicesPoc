﻿using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using PolicySearchService.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySearchService.DataAccess.ElasticSearch;

public class PolicyRepository : IPolicyRepository
{
    private readonly ElasticsearchClient elasticClient;

    public PolicyRepository(ElasticsearchClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public async Task Add(Policy policy)
    {
        await elasticClient.IndexAsync(policy);
    }

    public async Task<List<Policy>> Find(string queryText)
    {
        var result = await elasticClient
            .SearchAsync<Policy>(
                s =>
                    s.From(0)
                        .Size(10)
                        .Query(q =>
                            q.MultiMatch(mm =>
                                mm.Query(queryText)
                                    .Fields(Infer.Fields<Policy>(p => p.PolicyNumber, p => p.PolicyHolder))
                                    .Type(TextQueryType.BestFields)
                                    .Fuzziness(new Fuzziness("AUTO"))
                            )
                        ));

        return result.Documents.ToList() ?? new List<Policy>();
    }

    public async Task Delete(string policyNumber)
    {
        var hits = await GetIdAsync(policyNumber);
        foreach (var hit in hits)
            await elasticClient.DeleteAsync<Policy>(hit.Id);
    }

    private async Task<List<Hit<Policy>>> GetIdAsync(string policyNumber)
    {
        var result = await elasticClient
            .SearchAsync<Policy>(
                s =>
                    s.From(0)
                        .Size(10)
                        .Query(q =>
                            q.MultiMatch(mm =>
                                mm.Query(policyNumber)
                                    .Fields(Infer.Fields<Policy>(p => p.PolicyNumber, p => p.PolicyHolder))
                                    .Type(TextQueryType.BestFields)
                                    .Fuzziness(new Fuzziness("AUTO"))
                            )
                        ));
        return result.Hits.ToList();
    }
}