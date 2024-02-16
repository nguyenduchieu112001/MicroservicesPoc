namespace AuthService.Domain
{
    public class InsuranceAgentRoles
    {
        private InsuranceAgentRoles() { }

        private InsuranceAgentRoles(InsuranceAgent insuranceAgent, Role role)
        {
            InsuranceAgent = insuranceAgent;
            Role = role;
        }

        public long Id { get; }
        public long AgentId { get; }
        public InsuranceAgent InsuranceAgent { get; private set; }
        public long RoleId { get; }
        public Role Role { get; private set; }

        public static InsuranceAgentRoles CreateInsuranceAgentRoles(InsuranceAgent insuranceAgent, Role role)
        {
            return new InsuranceAgentRoles(insuranceAgent, role);
        }
    }
}
