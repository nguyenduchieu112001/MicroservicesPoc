using AuthService.DataAccess.EF;
using System.Linq;

namespace AuthService.Init;

public class DataLoader
{
    private readonly InsuranceAgentDbContext dbContext;
    private readonly DemoInsuranceAgentFactory agentFactory;

    public DataLoader(InsuranceAgentDbContext context, DemoInsuranceAgentFactory agentFactory)
    {
        dbContext = context;
        this.agentFactory = agentFactory;
    }

    public void Seed()
    {
        dbContext.Database.EnsureCreated();
        if (dbContext.InsuranceAgents.Any()) return;

        dbContext.InsuranceAgents.Add(agentFactory.Agent1());
        dbContext.InsuranceAgents.Add(agentFactory.Agent2());
        dbContext.InsuranceAgents.Add(agentFactory.Agent3());

        dbContext.SaveChanges();
    }
}
