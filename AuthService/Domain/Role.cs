using System.Collections.Generic;

namespace AuthService.Domain
{
    public class Role
    {
        private Role() { }
        private Role(string name)
        {
            Name = name;
            AgentRoles = new List<InsuranceAgentRoles>();
        }
        public long Id { get; }
        public string Name { get; }
        public IList<InsuranceAgentRoles> AgentRoles { get; private set; }

        public static Role Add(string name)
        {
            return new Role(name);
        }
    }
}
