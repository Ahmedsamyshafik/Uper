using Driver.Models.Data;
using Driver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Driver.Service;
using Driver.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Driver.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


#region Swagger Authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo", Version = "v1" });
});
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 5 Web API",
        Description = " ITI Projrcy"
    });

    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Go on",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
            new string[] {}
        }
                });
});
#endregion

//---- My Services----------
//----Context + Identity + Authentication
#region Context + Identity + Authentication 

builder.Services.AddDbContext<AppDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("defualtConnection"))
    );
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{
    // Password settings.
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 4;
    option.Password.RequiredUniqueChars = 0;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
//{
//    o.TokenLifespan = TimeSpan.FromHours(2);
//});
//builder.Services.AddScoped<IUserTwoFactorTokenProvider<ApplicationUser>, EmailTokenProvider<ApplicationUser>>();


#endregion

//----- JWT + Cros
#region JWT + Cros
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = false,
        //   ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("MyPolicy", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        corsPolicyBuilder.WithOrigins("http://localhost:3001", "http://localhost:3000");

    });
});
//WithOrignal("http://") React Port
#endregion

//Dependencies Injection
#region Dependencies Injection
builder.Services.AddRepoDependencies();
builder.Services.AddServicesDependencies();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline. // Swagger
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


//Seeding
#region Data Seeder
var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    DataSeeder.Seed(service);
}
#endregion
app.Run();
