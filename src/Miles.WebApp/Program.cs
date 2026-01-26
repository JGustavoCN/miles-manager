// using Miles.WebApp.Components;
//  using Miles.Infrastructure.Data;
// using Microsoft.EntityFrameworkCore;
// using MudBlazor.Services;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddRazorComponents()
//     .AddInteractiveServerComponents();

// // Configuração do Entity Framework Core com SQL Server
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("DefaultConnection"),
//         sqlOptions => sqlOptions.EnableRetryOnFailure(
//             maxRetryCount: 5,
//             maxRetryDelay: TimeSpan.FromSeconds(30),
//             errorNumbersToAdd: null
//         )
//     )
// );

// // Registrar serviços do MudBlazor
// builder.Services.AddMudServices();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error", createScopeForErrors: true);
//     app.UseHsts();
// }

// app.UseStatusCodePagesWithReExecute("/Error");
// app.UseHttpsRedirection();

// app.UseAntiforgery();

// app.MapStaticAssets();
// app.MapRazorComponents<App>()
//     .AddInteractiveServerRenderMode();

// app.Run();

