using GoogleLogs4DotNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Set the environment variable for Google Cloud credentials
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Users\vkbar\Downloads\service-account.json");


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<GoogleLogger>(sp =>
{
    var projectId = builder.Configuration["GoogleCloud:ProjectId"];
    var uniqueLogId = builder.Configuration["GoogleCloud:LogId"];
    return new GoogleLogger(projectId, uniqueLogId);
});



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
