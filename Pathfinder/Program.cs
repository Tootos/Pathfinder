using Google.Maps.Places.V1;
using Google.Maps.Routing.V2;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Pathfinder.Models;
using Pathfinder.Services.JourneyService;
using Pathfinder.Services.UserService;
using System.Text.Json.Serialization;


//Initialize new web application builder
var builder = WebApplication.CreateBuilder(args);

//Define a string constant for the CORS policy name
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//Register CORS service and define a policy 
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        //Specify allowed clients origins that are permitted to use our API 
        //This host port belongs to our client application 
        policy.WithOrigins("https://localhost:7269")
        .AllowAnyHeader()//Allow to send any Headers 
        .AllowAnyMethod();//and use any HTTP methods
    });
}
);

//My Services 
builder.Services.AddScoped<IJourneyService, JourneyService>();
builder.Services.AddScoped<IUserService, UserService>();    
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Instructs the serializer to stop following reference loops, 
    // preventing the infinite recursion error in Swagger.
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// 1. Add Swagger/OpenAPI Service Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pathfinder API", Version = "v1" });
});

//MongoDB 
builder.Services.Configure<NemunaDBSettings>(builder.Configuration.GetSection("NemunasDB"));
builder.Services.AddSingleton<IMongoClient>(sp =>
             new MongoClient(sp.GetRequiredService<IOptions<NemunaDBSettings>>().Value.ConnectionString));




builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var apiKey = config["GoogleApi:Key"];
    return new PlacesClientBuilder { ApiKey = apiKey }.Build();
});



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 2. Add Swagger Middleware
    app.UseSwagger();
    // This makes the UI available at /swagger and the JSON spec at /swagger/v1/swagger.json
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pathfinder API V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

//Enables defined CORS policy
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
