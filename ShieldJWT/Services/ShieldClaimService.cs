namespace ShieldJWT.Services;

public class ShieldClaimService : IShieldEntity<ShieldClaim>
{
    private readonly ShieldDbContext _context;

    public ShieldClaimService(ShieldDbContext context)
    {
        _context = context;
    }

    public int Create(IEnumerable<ShieldClaim> entities) => throw new NotImplementedException();

    public int Delete(int id) => throw new NotImplementedException();

    public IEnumerable<ShieldClaim> GetAll(Func<ShieldClaim, bool> predicado = null) 
        => predicado == null ? _context.Claims.AsEnumerable() : _context.Claims.Where(predicado);

    public int Update(IEnumerable<ShieldClaim> entities) => throw new NotImplementedException();
}
