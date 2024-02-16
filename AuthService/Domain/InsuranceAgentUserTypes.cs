namespace AuthService.Domain
{
    public class InsuranceAgentUserTypes
    {
        private InsuranceAgentUserTypes()
        {

        }
        private InsuranceAgentUserTypes(InsuranceAgent agent, UserType type)
        {
            InsuranceAgent = agent;
            UserType = type;
        }
        public long Id { get; }
        public long AgentId { get; }
        public InsuranceAgent InsuranceAgent { get; private set; }
        public long UserTypeId { get; }
        public UserType UserType { get; private set; }

        public static InsuranceAgentUserTypes CreateInsuranceAgentTypes(InsuranceAgent insuranceAgent, UserType type)
        {
            return new InsuranceAgentUserTypes(insuranceAgent, type);
        }
    }
}
