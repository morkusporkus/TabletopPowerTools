using Microsoft.Extensions.Logging;
using DMPowerTools.Data;
using MudBlazor.Services;
using System.Linq.Expressions;
using System;

namespace DMPowerTools;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddDbContext<ApplicationDbContext>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        builder.Services.AddMudServices();
        var app = builder.Build();

		using var scope = app.Services.CreateScope();

		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		try
		{ dbContext.Database.EnsureCreated(); }
		catch (Exception e)
		{
		var a=	e.Message;
		
		}

		return app;
	}
}
