using System.Collections.Generic;

namespace AuthService.Domain.Dtos
{
    public class InsuranceAgentDto
    {
        public string Login { get; set; }
        public string Avatar { get; set; }
        public List<string> Roles { get; set; }
        public List<string> UserTypes { get; set; }
    }
}
