namespace AuthService.Domain.Repository;

public interface IInsuranceAgents
{
    void Add(InsuranceAgent agent);

    InsuranceAgent FindByLogin(string login);
}