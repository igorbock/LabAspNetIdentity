namespace ShieldJWT.BackgroundServices;

public class LogService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly ShieldDbContext _context;

    public LogService(ShieldDbContext context)
    {
        _context = context;
    }

    private void CleanLogs(object? state)
    {
        _context.Logs.Load();
        var logs = _context.Logs.Local;

        try
        {
            var lastLogIteration = _context.LogDeleteIterations.OrderByDescending(a => a.LastIterationDate).FirstOrDefault();
            if (lastLogIteration is null)
                throw new ShieldException(200, "Excluindo logs...");

            var dateDiff = lastLogIteration!.LastIterationDate - DateTime.Now;
            if (dateDiff.TotalDays >= 90)
                throw new ShieldException(200, "Excluindo logs...");
        }
        catch (ShieldException)
        {
            _context.RemoveRange(logs);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CleanLogs, null, TimeSpan.Zero, TimeSpan.FromDays(15));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();
}
