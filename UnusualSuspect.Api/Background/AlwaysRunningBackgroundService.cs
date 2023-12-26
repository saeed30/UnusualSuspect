using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnusualSuspect.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using ElmahCore;

namespace UnusualSuspect.Api.Background;

public class AlwaysRunningBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
  // Override the ExecuteAsync method
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// Use a while loop with the cancellation token
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{

				using (var scope = scopeFactory.CreateScope())
				{

					// Do your work here
					await DoWorkAsync(scope, stoppingToken);

					// Wait for some time before the next iteration
				}
			}
			catch (Exception ex)
			{
				ElmahExtensions.RaiseError(ex);
			}
			await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
		}
	}

	// Define your work method
	private async Task DoWorkAsync(IServiceScope scope, CancellationToken stoppingToken)
	{
		IPreGameService preGameService = scope.ServiceProvider.GetRequiredService<IPreGameService>();
		await preGameService.CombineGroupsToStartGames(stoppingToken);
	}
}