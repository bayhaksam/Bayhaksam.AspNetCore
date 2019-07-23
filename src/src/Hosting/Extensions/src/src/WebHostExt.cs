//-----------------------------------------------------------------------
// <copyright file="WebHostExt.cs" company="Bayhaksam">
//      Copyright (c) Bayhaksam. All rights reserved.
// </copyright>
// <author>Samet Kurumahmut</author>
//-----------------------------------------------------------------------

namespace Bayhaksam.AspNetCore.Hosting.Extensions.src
{
	using Bayhaksam.Data;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using System;

	public static class WebHostExt
	{
		#region Public Methods
		public static void SeedDatabase<TDbSeeder, TLoggerCategoryName>(this IWebHost src)
			where TDbSeeder : IDbSeeder
		{
			using (var scope = src.Services.CreateScope())
			{
				SeedData<TDbSeeder, TLoggerCategoryName>(scope.ServiceProvider);
			}
		}

		public static void SeedDatabaseForDevelopment<TDbSeeder, TLoggerCategoryName>(this IWebHost src)
			where TDbSeeder : IDbSeeder
		{
			using (var scope = src.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				if (services.GetRequiredService<IHostingEnvironment>().IsDevelopment())
				{
					SeedData<TDbSeeder, TLoggerCategoryName>(services);
				}
			}
		}
		#endregion

		#region Methods
		static void SeedData<TDbSeeder, TLoggerCategoryName>(IServiceProvider services) where TDbSeeder : IDbSeeder
		{
			var env = services.GetRequiredService<IHostingEnvironment>();
			var logger = services.GetRequiredService<ILogger<TLoggerCategoryName>>();

			try
			{
				logger.LogInformation($"Seeding the database in '{env.EnvironmentName}' environment.");

				services.GetRequiredService<TDbSeeder>().Seed();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred while seeding the database.");
			}
		}
		#endregion
	}
}
