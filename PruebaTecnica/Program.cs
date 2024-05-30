using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json.Serialization;
using PruebaTecnica.Api.Controllers.ErrorHandling;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCaching();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<ExceptionMiddleware>();

builder.Services.AddResponseCompression();

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
    {
        NamingStrategy = new SnakeCaseNamingStrategy()
    };
});

var configuration = builder.Configuration;
builder.Services.AddHttpClient("RestApi", client =>
{
    client.BaseAddress = new Uri(configuration.GetValue<string>("Endpoint:UrlHistory")!);
    client.Timeout = TimeSpan.FromMinutes(3);
});


builder.Services.AddMvc(options => { 
    options.EnableEndpointRouting = false;
    var jsonInputFormatter = options.InputFormatters.OfType<NewtonsoftJsonInputFormatter>().First();
    jsonInputFormatter.SupportedMediaTypes.Add("multipart/form-data");
}).AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/", "");
});

builder.Services.AddAuthentication("PruebaTecnicaAuthScheme")
    .AddCookie("PruebaTecnicaAuthScheme", options =>
    {
        options.Cookie.Name = ".PruebaTecnica.Cookies.login";
        options.LoginPath = "/";
    });


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCookiePolicy(
            new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always
            });


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseMvcWithDefaultRoute();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
