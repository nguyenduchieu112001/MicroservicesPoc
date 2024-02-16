using AuthService.Domain;
using System.Collections.Generic;

namespace AuthService.Init;

public class DemoInsuranceAgentFactory
{
    private readonly HashPassword hashPassword;
    private readonly UserType userType;
    private readonly UserType userTypeAdmin;
    private readonly Role roleUser;
    private readonly Role roleSalesman;
    public DemoInsuranceAgentFactory(HashPassword hashPassword)
    {
        this.hashPassword = hashPassword;
        userType = CreateUserType();
        roleUser = CreateRoleUser();
        roleSalesman = CreateRoleSalesman();
        userTypeAdmin = CreateUserTypeAdmin();
    }

    public UserType CreateUserType()
    {
        var userType = UserType.Add("SALESMAN");
        return userType;
    }

    public UserType CreateUserTypeAdmin()
    {
        return UserType.Add("ADMIN");
    }

    public Role CreateRoleUser()
    {
        return Role.Add("USER");
    }

    public Role CreateRoleSalesman()
    {
        return Role.Add("SALESMAN");
    }

    public InsuranceAgent Agent1()
    {
        var agent = InsuranceAgent.Create("jimmy.solid", "secret", "/b095180c-1759-4233-9cf3-fbd66099c558.png");
        var hash = hashPassword.Hash(agent);
        var agentHashPassword = InsuranceAgent.Create(agent.Login, hash, agent.Avatar);

        var listAgentRole = new List<InsuranceAgentRoles>
        {
            InsuranceAgentRoles.CreateInsuranceAgentRoles(agentHashPassword, roleUser),
            InsuranceAgentRoles.CreateInsuranceAgentRoles(agentHashPassword, roleSalesman),
        };
        agentHashPassword.AddAgentRoles(listAgentRole);

        var listAgentTypes = new List<InsuranceAgentUserTypes>
        {
            InsuranceAgentUserTypes.CreateInsuranceAgentTypes(agentHashPassword, userType),
        };
        agentHashPassword.AddAgentTypes(listAgentTypes);


        return agentHashPassword;
    }

    public InsuranceAgent Agent2()
    {
        var agent = InsuranceAgent.Create("danny.solid", "secret", "/bc77d8ee-b2a7-4167-a1c1-f9788f408620.png");
        var hash = hashPassword.Hash(agent);
        var agentHashPassword = InsuranceAgent.Create(agent.Login, hash, agent.Avatar);

        var listAgentRole = new List<InsuranceAgentRoles>
        {
            InsuranceAgentRoles.CreateInsuranceAgentRoles(agentHashPassword, roleUser),
            InsuranceAgentRoles.CreateInsuranceAgentRoles(agentHashPassword, roleSalesman),
        };
        agentHashPassword.AddAgentRoles(listAgentRole);

        var listAgentTypes = new List<InsuranceAgentUserTypes>
        {
            InsuranceAgentUserTypes.CreateInsuranceAgentTypes(agentHashPassword, userType),
        };
        agentHashPassword.AddAgentTypes(listAgentTypes);

        return agentHashPassword;
    }

    public InsuranceAgent Agent3()
    {
        var agent = InsuranceAgent.Create("admin", "admin", "/5e6f7002-d1b3-44d0-add3-870eb71ed72e.png");
        var hash = hashPassword.Hash(agent);
        var agentHashPassword = InsuranceAgent.Create(agent.Login, hash, agent.Avatar);

        var listAgentRole = new List<InsuranceAgentRoles>
        {
            InsuranceAgentRoles.CreateInsuranceAgentRoles(agentHashPassword, roleUser),
            InsuranceAgentRoles.CreateInsuranceAgentRoles(agentHashPassword, roleSalesman),
        };
        agentHashPassword.AddAgentRoles(listAgentRole);

        var listAgentTypes = new List<InsuranceAgentUserTypes>
        {
            InsuranceAgentUserTypes.CreateInsuranceAgentTypes(agentHashPassword, userTypeAdmin),
            InsuranceAgentUserTypes.CreateInsuranceAgentTypes(agentHashPassword, userType),
        };
        agentHashPassword.AddAgentTypes(listAgentTypes);

        return agentHashPassword;
    }
}
