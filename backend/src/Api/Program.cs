using Ecommerce.Domain;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// AGREGACION DE INFRASTRUCTURE SERVICES
builder.Services.AddInfrastructureServices(builder.Configuration /* este es el objeto que me permite agarrar los valores del archivo JSON*/);


// AGREGACION DE CONTEXTO
builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DockerConexion"),
        b => b.MigrationsAssembly(typeof(EcommerceDbContext).Assembly.FullName));
    // para que aparezca en consola todos los querys que se hacen en la utilizacion del sistema.
});
// Add services to the container.

builder.Services.AddControllers(options =>
{
    // SEGURIDAD POR JWT A LOS ENDPOINTS DEL PROYECTO
    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser().Build();

    options.Filters.Add(new AuthorizeFilter(policy));

});

IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<Usuario>();

identityBuilder = new IdentityBuilder(identityBuilder.UserType, identityBuilder.Services);

identityBuilder.AddRoles<IdentityRole>().AddDefaultTokenProviders();
identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario, IdentityRole>>();

identityBuilder.AddEntityFrameworkStores<EcommerceDbContext>();
identityBuilder.AddSignInManager<SignInManager<Usuario>>();

builder.Services.TryAddSingleton<ISystemClock, SystemClock>();



// CONFIGURACION DEL TOKEN 
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSuperMaestro:key"]!));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateAudience = false,
            ValidateIssuer = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

// 
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    // INSTANCIAR EL CONTEXT, ROL, ETC.
    try
    {
        var context = service.GetRequiredService<EcommerceDbContext>();
        var usuarioManager = service.GetRequiredService<UserManager<Usuario>>();
        var rolManager = service.GetRequiredService<RoleManager<IdentityRole>>();
    
        // MIGRAR 
        await context.Database.MigrateAsync();

        //LLAMAR A LA DATA
        await EcommerceDbContextData.LoadDataAsync(context, usuarioManager, rolManager, loggerFactory);
    }
    catch (Exception e)
    {

        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(e, "Error en la migración"); 
    }
};


app.Run();
