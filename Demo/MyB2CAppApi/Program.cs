using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(
    _ => { },
    identityOptions =>
    {
        //typically this is configured via settings in AppSettings
        identityOptions.Instance = "https://[Your Tenant Name].b2clogin.com";//The name of your tenant
        identityOptions.Domain = "[Your Tenant Name].onmicrosoft.com";//The name of your tenant
        identityOptions.ClientId = "";//The Application (client) Id of the AppRegistration (API) you want to use
        identityOptions.SignedOutCallbackPath = "/signout/B2C_1_SignUp_SignIn";//The Sign Up and Sign In you want to use
        identityOptions.SignUpSignInPolicyId = "B2C_1_SignUp_SignIn";//The Sign Up and Sign In you want to use
    });

//typically the above is configured via settings in AppSettings, like this:
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication(); //Needs to go first, before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
