using SehirRehberi.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SehirRehberi.API.Helpers;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));
//(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddCors(p => p.AddPolicy("cors-app", corsPolicyBuilder =>
{
    corsPolicyBuilder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


builder.Services.AddScoped<IAppRepository,AppRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey= true,
        IssuerSigningKey= new SymmetricSecurityKey(key),
        ValidateIssuer= false,
        ValidateAudience= false,
    };
});

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
app.UseCors("cors-app");

app.MapControllers();
app.UseAuthentication();

app.Run();
