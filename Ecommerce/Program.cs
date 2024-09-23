using Ecommerce.V1.Commons.EcommmerceContext;
using Ecommerce.V1.Commons.SwaggerConfigurations;
using Ecommerce.V1.Commons.TokenGenerator.Interface;
using Ecommerce.V1.Commons.TokenGenerator.Services;
using Ecommerce.V1.Roles.Entities;
using Ecommerce.V1.Users.Entities;
using Ecommerce.V1.Users.Interfaces;
using Ecommerce.V1.Users.Services;
using EcommerceApi.V1.Commons.ExtensionFuntions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.GetConnectionString("EcommerceDatabase");


////////////////////////////
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    // serialize enums as strings in api responses (e.g. Role)
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

});
//////////////////////////what is it 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    options.UseNpgsql(configuration);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddServiceConfiguration()
    .AddSwaggerService(builder.Configuration);

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<EcommerceDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
