namespace ShieldJWT.BackgroundServices;

public class LogService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IShieldEntity<Log> _logService;
    private readonly IShieldEntity<LogDeleteIteration> _logDeleteIterationService;

    public LogService(IShieldEntity<Log> logService, IShieldEntity<LogDeleteIteration> logDeleteIteration)
    {
        _logService = logService;
        _logDeleteIterationService = logDeleteIteration;
    }

    private void CleanLogs(object? state)
    {
        var logs = _logService.GetAll();

        try
        {
            var lastLogIteration = _logDeleteIterationService.GetAll().OrderByDescending(a => a.LastIterationDate).FirstOrDefault();
            if (lastLogIteration is null)
                throw new ShieldException(200, "Excluindo logs...");

            var dateDiff = lastLogIteration!.LastIterationDate - DateTime.Now;
            if (dateDiff.TotalDays >= 90)
                throw new ShieldException(200, "Excluindo logs...");
        }
        catch (ShieldException)
        {
            _logService.Delete();
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CleanLogs, null, TimeSpan.Zero, TimeSpan.FromDays(90));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();
}
