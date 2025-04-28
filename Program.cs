
using Routes;
var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                      policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                                                .AllowAnyMethod();
                      ;
                    });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
app.ConfigureRoutes();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();




app.Run();

