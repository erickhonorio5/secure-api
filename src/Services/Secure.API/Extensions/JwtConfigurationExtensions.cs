namespace Secure.API.Extensions;

public static class JwtConfigurationExtensions
{
    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            byte[] key = Encoding.ASCII.GetBytes("A1B2C3D4E5F67890AABBCCDDEEFF1122");
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false,
                RequireExpirationTime = false,
                ValidateIssuerSigningKey = true
            };
        });

        services.AddDefaultIdentity<IdentityUser>(opt =>
        {
            opt.SignIn.RequireConfirmedEmail = true;
            opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
        }).AddEntityFrameworkStores<AppDbContext>();
    }
}
