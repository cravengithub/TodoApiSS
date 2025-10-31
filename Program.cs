using Microsoft.EntityFrameworkCore; // Tambahkan ini
using TodoApiSS.Data; // Tambahkan ini
using TodoApiSS.Interfaces; // <-- Tambahkan
using TodoApiSS.Repositories; // <-- Tambahkan
using TodoApiSS.Services; // <-- Tambahkan
using Microsoft.AspNetCore.Authentication.JwtBearer; // <-- Tambahkan
using Microsoft.IdentityModel.Tokens; // <-- Tambahkan
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Definisikan nama policy CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// --- Bagian Konfigurasi Layanan (Services) ---

// Tambahkan layanan CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        // Izinkan aplikasi Angular (localhost:4200)
        policy.WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod();
    });
});

// 1. Tambahkan layanan controller
builder.Services.AddControllers();

// 2. Konfigurasi EF Core dan SQL Server
// Kita mengambil Connection String dari appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(connectionString)); // <= UBAH INI


// === TAMBAHKAN INI ===
// Daftarkan lapisan arsitektur kita
// AddScoped: instance baru dibuat sekali per permintaan HTTP klien.
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
// =====================

// 3. Tambahkan Swagger/OpenAPI (biasanya sudah ada)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// === TAMBAHKAN KONFIGURASI AUTENTIKASI ===
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new
    SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
// ==========================================

// --- Bagian Pembangunan Aplikasi (App) ---
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Terapkan policy CORS
app.UseCors(MyAllowSpecificOrigins); // <= TAMBAHKAN INI (sebelum UseAuthorization)

// === TAMBAHKAN INI (Urutan SANGAT PENTING) ===
app.UseAuthentication(); // 1. Autentikasi (Cek siapa Anda)
app.UseAuthorization(); // 2. Otorisasi (Cek apa yang boleh Anda lakukan)
// ==========================================

// Map controller endpoints
app.MapControllers();

app.Run();
