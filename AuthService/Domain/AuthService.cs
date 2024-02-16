using AuthService.Domain.Dtos;
using AuthService.Domain.Repository;
using AuthService.Init;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Domain;

public class AuthService
{
    private readonly AppSettings appSettings;
    private readonly IInsuranceAgentRepository agentRepository;
    private readonly HashPassword hashpassword;
    private readonly IWebHostEnvironment env;

    public AuthService(IOptions<AppSettings> appSettings, IInsuranceAgentRepository agentRepository, HashPassword password, IWebHostEnvironment env)
    {
        this.appSettings = appSettings.Value;
        this.agentRepository = agentRepository;
        this.hashpassword = password;
        this.env = env;
    }

    public async Task<string> Authenticate(string login, string pwd)
    {
        var agent = await agentRepository.FindByLogin(login);

        if (agent == null)
            return null;

        var isValid = hashpassword.Verify(agent, pwd);
        if (!isValid)
            return null;

        var claims = new List<Claim>
        {
            new Claim("sub", agent.Login),
            new Claim(ClaimTypes.Name, agent.Login),
            new Claim("avatar", agent.Avatar),
        };

        foreach (var agentRole in agent.AgentRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, agentRole.Role.Name));
        }

        foreach (var agentType in agent.AgentTypes)
        {
            claims.Add(new Claim("userType", agentType.UserType.Name));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public InsuranceAgentDto AgentFromLogin(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

        return new InsuranceAgentDto
        {
            Login = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value,
            Avatar = claimsPrincipal.FindFirst("avatar")?.Value,
            Roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
            UserTypes = claimsPrincipal.FindAll("userType")?.Select(c => c.Value).ToList(),
        };
    }

    public async Task<bool> Register(string login, string password, string avatar)
    {
        var existingAgent = await agentRepository.FindByLogin(login);
        if (existingAgent != null)
        {
            return false;
        }
        var agent = InsuranceAgent.Create(login, password, avatar);
        var hash = hashpassword.Hash(agent);
        var agentHashPassword = InsuranceAgent.Create(agent.Login, hash, agent.Avatar);

        var listAgentRole = new List<InsuranceAgentRoles>
        {
            InsuranceAgentRoles.CreateInsuranceAgentRoles(agentHashPassword, await CreateRoleUser()),
            InsuranceAgentRoles.CreateInsuranceAgentRoles(agentHashPassword, await CreateRoleSalesman()),
        };
        agentHashPassword.AddAgentRoles(listAgentRole);

        var listAgentType = new List<InsuranceAgentUserTypes>
        {
            InsuranceAgentUserTypes.CreateInsuranceAgentTypes(agentHashPassword, await CreateUserType()),
        };
        agentHashPassword.AddAgentTypes(listAgentType);


        await agentRepository.Add(agentHashPassword);
        return true;
    }
    private async Task<Role> CreateRoleUser()
    {
        return await agentRepository.GetRole("USER");

    }

    private async Task<Role> CreateRoleSalesman()
    {
        return await agentRepository.GetRole("SALESMAN");
    }
    private async Task<UserType> CreateUserType()
    {
        return await agentRepository.GetUserType("SALESMAN");
    }

    public async Task<string> UploadAvatar(IFormFile request, CancellationToken cancellationToken)
    {
        try
        {
            var contentPath = env.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Check the allowed extenstions
            var ext = Path.GetExtension(request.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(ext))
            {
                string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                throw new AggregateException(msg);
            }
            string uniqueString = Guid.NewGuid().ToString();
            var newFileName = uniqueString + ext;
            var fileWithPath = Path.Combine(path, newFileName);
            using var stream = new FileStream(fileWithPath, FileMode.Create);
            await request.CopyToAsync(stream, cancellationToken);
            return $"/{newFileName}";
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error has occurred", ex);
        }
    }
}