using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PaymentService.Domain;

namespace PaymentService.DataAccess.Marten;

public static class MartenInstaller
{
    public static void AddMarten(this IServiceCollection services, string cnnString)
    {
        services.AddMarten(options =>
        {
            options.Connection(cnnString);
            options.DatabaseSchemaName = "payment_service";
            options.Serializer(CustomizeJsonSerializer());
            options.Schema.For<PolicyAccount>()
                .Identity(t => t.Id)
                .Duplicate(t => t.PolicyNumber, "varchar(50)", configure: idx => idx.IsUnique = true);
        });
        //services.AddSingleton(CreateDocumentStore(cnnString));
        //services.AddScoped<IDocumentSession>(e => e.GetRequiredService<IDocumentStore>().LightweightSession());
        services.AddTransient<IDataStore, MartenDataStore>();
        services.AddTransient<IPolicyAccountRepository, MartenPolicyAccountRepository>();
    }

    private static IDocumentStore CreateDocumentStore(string cn)
    {
        return DocumentStore.For(_ =>
        {
            _.Connection(cn);
            _.DatabaseSchemaName = "payment_service";

            _.Serializer(CustomizeJsonSerializer());
            _.Schema.For<PolicyAccount>()
                .Identity(t => t.Id)
                .Duplicate(t => t.PolicyNumber, "varchar(50)", configure: idx => idx.IsUnique = true);
        });
    }

    private static JsonNetSerializer CustomizeJsonSerializer()
    {
        var serializer = new JsonNetSerializer();

        serializer.Customize(_ =>
        {
            _.ContractResolver = new ProtectedSettersContractResolver();
            _.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        });

        return serializer;
    }
}