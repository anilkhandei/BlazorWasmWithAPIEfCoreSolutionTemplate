var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Allow CORS
string AllowBlazorWasm = "AllowBlazorWasm";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowBlazorWasm,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7243")
                                .AllowAnyHeader()
                                .AllowAnyMethod();

                      });
});
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

app.UseCors(AllowBlazorWasm);

app.UseAuthorization();

app.MapControllers();

app.Run();
