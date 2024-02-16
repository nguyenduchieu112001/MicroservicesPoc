using System.Collections.Generic;

namespace AuthService.Domain
{
    public class UserType
    {
        public UserType() { }
        private UserType(string name)
        {
            Name = name;
            AgentTypes = new List<InsuranceAgentUserTypes>();
        }
        public long Id { get; }
        public string Name { get; }
        public IList<InsuranceAgentUserTypes> AgentTypes { get; private set; }
        public static UserType Add(string name)
        {
            return new UserType(name);
        }
    }
}
