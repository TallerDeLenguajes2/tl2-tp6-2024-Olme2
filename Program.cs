var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IClientesRepository,ClientesRepository>();
builder.Services.AddSingleton<IPresupuestosRepository, PresupuestosRepository>();
builder.Services.AddSingleton<IProductosRepository, ProductosRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
// Add services to the container.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible desde HTTP, no JavaScript
    options.Cookie.IsEssential = true; // Necesario incluso si el usuario no acepta cookies
});
builder.Services.AddControllersWithViews();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Presupuestos}/{action=Index}/{id?}");

app.Run();