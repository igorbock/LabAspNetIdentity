namespace ShieldJWT.Services;

public class CompanyService : IShieldCompany
{
    private readonly ShieldDbContext _context;

    public CompanyService(ShieldDbContext context)
    {
        _context = context;
    }

    public void CreateLog(Log log)
    {
        _context.Logs.Add(log);
        _context.SaveChanges();
    }

    public void ValidateCompany(Guid idCompany)
    {
        var company = _context.Companies.Find(idCompany);
        if (company == null)
            throw new ShieldException(401, "Empresa não autorizada");
    }
}
