using System.Collections.Generic;

namespace AuthService.Domain;

public class InsuranceAgent
{
    public InsuranceAgent() { }

    private InsuranceAgent(string login, string password, string avatar)
    {
        Login = login;
        Password = password;
        Avatar = avatar;
        AgentRoles = new List<InsuranceAgentRoles>();
        AgentTypes = new List<InsuranceAgentUserTypes>();
    }

    public long Id { get; }
    public string Login { get; }
    public string Password { get; }
    public string Avatar { get; }
    public long UserTypeId { get; }
    public IList<InsuranceAgentRoles> AgentRoles { get; private set; }
    public IList<InsuranceAgentUserTypes> AgentTypes { get; private set; }

    public static InsuranceAgent Create(string login, string password, string avatar)
    {
        return new InsuranceAgent(login, password, avatar);
    }

    public void AddAgentTypes(List<InsuranceAgentUserTypes> agentUserTypes)
    {
        foreach (var agentUserType in agentUserTypes)
            AgentTypes.Add(agentUserType);
    }

    public void AddAgentRoles(List<InsuranceAgentRoles> agentRoles)
    {
        foreach (var agentRole in agentRoles)
        {
            AgentRoles.Add(agentRole);
        }
    }
}