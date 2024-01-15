using NZWalks.API.Data;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;
using NZWalks.API.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( options =>
{
    options.ResolveConflictingActions( apiDescriptions => apiDescriptions.First() );
} );

builder.Services.AddDbContext<NzWalksDbContext>( options =>
{
    options.UseSqlServer( builder.Configuration.GetConnectionString( "NZWalksConnectionString" ) );
} );

builder.Services.AddDbContext<NzWalksAuthDbContext>( options =>
{
    options.UseSqlServer( builder.Configuration.GetConnectionString( "NZWalksAuthConnectionString" ) );
} );


builder.Services.AddScoped < IRegionRepository, SQLRegionRepository >();
builder.Services.AddScoped<IWalksRepository, SQLWalkRepository>();

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
    .AddJwtBearer( options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration ["Jwt: Issuer"],
        ValidAudience = builder.Configuration ["Jwt: Audience"],
        IssuerSigningKey = new SymmetricSecurityKey( 
            Encoding.UTF8.GetBytes( builder.Configuration ["Jwt:Key"] ))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


