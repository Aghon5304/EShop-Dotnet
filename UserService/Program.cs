using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;
using User.Domain.Exceptions;
using UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AdminOnly", policy =>
		policy.RequireRole("Administrator"));

// JWT config
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettings);
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Wpisz token w formacie: Bearer {token}",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement()
	  {
		{
		  new OpenApiSecurityScheme
		  {
			Reference = new OpenApiReference
			  {
				Type = ReferenceType.SecurityScheme,
				Id = "Bearer"
			  },
			  Scheme = "oauth2",
			  Name = "Bearer",
			  In = ParameterLocation.Header,

			},
			new List<string>()
		  }
		});
});
builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AdminOnly", policy =>
		policy.RequireRole("Administrator"))
	.AddPolicy("EmployeeOnly", policy =>
		policy.RequireRole("Employee"));
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	var rsa = RSA.Create();
	rsa.ImportFromPem(File.ReadAllText("../data/public.key"));
	var publicKey = new RsaSecurityKey(rsa);

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = "EShopNetCourse",
		ValidAudience = "Eshop",
		IssuerSigningKey = publicKey
	};
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
