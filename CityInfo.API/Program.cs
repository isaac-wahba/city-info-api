var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// add configs for the problem details, like when u get 404 not found

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (ctx) =>
    {
        // additionalInfo is a key, and Additional information about the problem. is the value
        ctx.ProblemDetails.Extensions.Add("additionalInfo", "Additional information about the problem.");
        // server is a key, and Environment.MachineName is the value
        ctx.ProblemDetails.Extensions.Add("server", Environment.MachineName);
    };
}
    
    );
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

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
