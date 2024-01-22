using NZWalks.API.Data;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;
using NZWalks.API.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Catel.Services;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();



// Add swager

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( options =>
{

    options.SwaggerDoc( "v1", new OpenApiInfo { Title = "Nz Walks API", Version = "v1" } );
    options.ResolveConflictingActions( apiDescriptions => apiDescriptions.First() );
    options.AddSecurityDefinition( JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    } );

    options.AddSecurityRequirement( new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            },
            Scheme = "Oauth2",
            Name = JwtBearerDefaults.AuthenticationScheme,
            In = ParameterLocation.Header
        },
        new List<string>()
    }
} );
} );

builder.Services.AddCors( options =>
{
    options.AddDefaultPolicy( builder =>
    {
        builder.WithOrigins( "https://localhost:7297/" );
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    } );
} );


builder.Services.AddDbContext<NzWalksDbContext>( options =>
{
    options.UseSqlServer( builder.Configuration.GetConnectionString( "NZWalksConnectionString" ) );
} );

builder.Services.AddDbContext<NzWalksAuthDbContext>( options =>
{
    options.UseSqlServer( builder.Configuration.GetConnectionString( "NZWalksAuthConnectionString" ) );
} );


builder.Services.AddScoped<IRegionRepository, SQLRegionRepository >();
builder.Services.AddScoped<IWalksRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>() ;
builder.Services.AddScoped<IImageRepository, LocalImageRepository > ();


//inject mapping into the controller 
builder.Services.AddAutoMapper(typeof ( AutoMapperProfiles ) );

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>( "NzWalks" )
    .AddEntityFrameworkStores<NzWalksAuthDbContext>()
    .AddDefaultTokenProviders();

//Configuring password requirements
builder.Services.Configure<IdentityOptions>( options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
} );

//adding authentication  and JWT bearer token to the service
builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
    .AddJwtBearer( options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration ["Jwt:Issuer"],
        ValidAudience = builder.Configuration ["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes( builder.Configuration ["Jwt:Key"] ) )
    } );

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


