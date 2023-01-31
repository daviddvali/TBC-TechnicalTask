using Microsoft.EntityFrameworkCore;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;
using TBC.Task.Repository;
using TBC.Task.Repository.Database;
using TBC.Task.Service;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PersonsDbContext>(options =>
	options.UseSqlServer(configuration.GetConnectionString("TbcPersons")));

builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<IRelatedPersonRepository, RelatedPersonRepository>();

builder.Services.AddTransient<ICityService, CityService>();
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IRelatedPersonService, RelatedPersonService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();