using LanguageBackend.Application.Features.Auth.Commands.Register;
using LanguageBackend.Application.Interfaces;
using LanguageBackend.Domain.Entities;
using LanguageBackend.Infrastructure.Services;
using LanguageBackend.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

using LanguageBackend.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();

// appsettıng json dosyasındas yazdıgımız verı tabanı baglantı cumlesının  verı tabanı ıle koprusunu sagladık 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




//Kullanıcı tipi → AppUser kullanıcılar app user uzerınden yonetılecek 
//Rol tipi → IdentityRole   rol sıstemı kullanılabılecek 
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // kolaylık acısından sıfre kurallarını gevsettık 
    options.Password.RequiredLength = 6; // mınımum 6 karakter yeterlı 
    options.Password.RequireNonAlphanumeric = false;// sayı zorunlulugunu kapattık
    options.Password.RequireDigit = false; // ozel karakter zorunlulugunu kapttık 
    options.Password.RequireLowercase = false;// 
    options.Password.RequireUppercase = false;//usetık ve bu kod ıle buyuk kucuk  harf kuralını kapattık 

    options.SignIn.RequireConfirmedEmail = true; // sısteme gırıs yapmak ıcın e maıl ıle dogrulama sartı koyduk user taplosunda bunun ıle ılgılı bır  bool alan var 
})
.AddEntityFrameworkStores<AppDbContext>() // ıdentıtıy verlerım Appcontext uzerınden verıtabanına kayıt edılecek yanı taplolar ıdentıty kutuphanesı tabloları olacak 
.AddDefaultTokenProviders();   // tokan  sıstemını actım e maıl dogrulamatokanı vs uretımı            

//yazdıgımız Email servisin kaydını yaptık scope 
builder.Services.AddScoped<IEmailService, EmailService>();

// token services kaydı 
builder.Services.AddScoped<ITokenService, TokenService>();

//mediatr kaydı 
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));

//JWT Authentication Ayarları
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
    };
});

builder.Services.AddControllers(); // end poıntler controllar uzerınden yazılacak 

// CORS Ayarları (Frontend'in Backend'e erişebilmesi için)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IUserWordRepository, UserWordRepository>();

builder.Services.AddEndpointsApiExplorer();// swager aktıf edıldı 

// Gemini servisi HttpClient ile kaydedildi
builder.Services.AddHttpClient<IGeminiService, GeminiService>();


//4. Swagger'a JWT Yetkilendirme Butonu Ekleme
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Language API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT token girmek için 'Bearer boşluk token' formatında yazınız. Örnek: 'Bearer eyJhb...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build(); // uygulama ayagı kaldır 

// development ortamında  ısen swagger ac 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAll"); // CORS politikasını aktifleştir
app.UseAuthentication(); // kullanıcı kımlıgını dogrular bu kullanıcı kım sorunun cevabı 
app.UseAuthorization();// yetkı kontrolu yaparız  bu kullanıcı bura gırebılır mı 

app.MapControllers(); // controllerlar route ye baglandı 

app.Run();// uygulamayaı baslat server ayaga kalktı 
