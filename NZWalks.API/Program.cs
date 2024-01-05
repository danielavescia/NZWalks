using NZWalks.API.Data;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;
using NZWalks.API.Mappings;

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

builder.Services.AddScoped < IRegionRepository, SQLRegionRepository >();
builder.Services.AddScoped<IWalksRepository, SQLWalkRepository>();

//inject mapping into the controller 
builder.Services.AddAutoMapper(typeof ( AutoMapperProfiles ) );

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


