using System.Diagnostics;
using BigBadBlog.Data.Postgres;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace BigBadBlog.Service.DatabaseMigration;

public class Worker : BackgroundService
{
	private readonly IServiceProvider serviceProvider;
	private readonly IHostApplicationLifetime hostApplicationLifetime;
	private readonly ILogger<Worker> _logger;

	internal const string ActivityName = "MigrationService";
	private static readonly ActivitySource _activitySource = new(ActivityName);

	public Worker(IServiceProvider serviceProvider,
		IHostApplicationLifetime hostApplicationLifetime,
		ILogger<Worker> logger)
	{
		this.serviceProvider = serviceProvider;
		this.hostApplicationLifetime = hostApplicationLifetime;
		_logger = logger;
	}

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
		using var activity = _activitySource.StartActivity("Migrating database", ActivityKind.Client);

		try
		{
			using var scope = serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			await dbContext.Database.MigrateAsync(stoppingToken);

		}
		catch (Exception ex)
		{
			activity?.RecordException(ex);
			throw;
		}

		hostApplicationLifetime.StopApplication();
    }
}
