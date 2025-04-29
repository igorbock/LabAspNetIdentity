namespace ShieldJWT.Services;

public class ShieldLogDeleteIterationService : IShieldEntity<LogDeleteIteration>
{
    private readonly ShieldDbContext _context;

    public ShieldLogDeleteIterationService(ShieldDbContext context)
    {
        _context = context;

        _context.LogDeleteIterations.Load();
    }

    public int Create(IEnumerable<LogDeleteIteration> entities)
    {
        _context.AddRange(entities);
        return _context.SaveChanges();
    }

    public int Delete(int? id = null)
    {
        if (id is null)
            _context.LogDeleteIterations.RemoveRange();
        else
        {
            var entity = _context.LogDeleteIterations.Find(id) ?? throw new Exception("Iteração de log não encontrada");
            _context.Remove(entity);
        }
        
        return _context.SaveChanges();
    }

    public IEnumerable<LogDeleteIteration> GetAll(Func<LogDeleteIteration, bool> predicado = null)
        => predicado is null ? _context.LogDeleteIterations.Local : _context.LogDeleteIterations.Where(predicado);

    public int Update(IEnumerable<LogDeleteIteration> entities)
    {
        _context.UpdateRange(entities);
        return _context.SaveChanges();
    }
}
