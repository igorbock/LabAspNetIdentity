namespace ShieldJWT.Services;

public class ShieldLogService : IShieldEntity<Log>
{
    private readonly ShieldDbContext _context;

    public ShieldLogService(ShieldDbContext context)
    {
        _context = context;

        _context.Logs.Load();
    }

    public int Create(IEnumerable<Log> entities)
    {
        _context.AddRange(entities);
        return _context.SaveChanges();
    }

    public int Delete(int? id = null)
    {
        if (id == null)
            _context.Logs.RemoveRange();
        else
        {
            var entity = _context.Logs.Find(id) ?? throw new Exception("Log não encontrado");
            _context.Remove(entity);
        }
        
        return _context.SaveChanges();
    }

    public IEnumerable<Log> GetAll(Func<Log, bool> predicado = null) => predicado is null ? _context.Logs.Local : _context.Logs.Where(predicado);

    public int Update(IEnumerable<Log> entities)
    {
        _context.Update(entities);
        return _context.SaveChanges();
    }
}
