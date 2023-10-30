using ErrandEventAPI.Data;
using ErrandEventAPI.Services;
using ErrandEventAPI.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddScoped<IEventService, EventService> ();
builder.Services.AddDbContext<EventDbContext> ();
builder.Services.AddScoped<IEventProducer, EventProducer> ();
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
