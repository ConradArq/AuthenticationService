using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using AuthenticationService.API.Filters;
using AuthenticationService.API.ModelBinders.Providers;
using System.Globalization;
using System.Text;
using AuthenticationService.Shared.Exceptions;
using AuthenticationService.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AuthenticationService.Application
{
    public static class APIServiceRegistration
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Localization

            // Uncomment this line to enable localization using a folder other than the default "Resources" folder for .resx files.
            // This will configure the application to load localized strings from .resx files located in the specified folder (e.g., "FolderName").
            // Example: Place your resource files in "FolderName/ValidationMessages.en.resx" or "FolderName/ValidationMessages.es.resx".
            ////services.AddLocalization(options => options.ResourcesPath = "FolderName");

            // Uncomment these two lines to enable localization from the database.
            // This setup overrides the default behavior of using the "Resources" folder by replacing the default IStringLocalizerFactory.
            // All calls to IStringLocalizer or IStringLocalizer<T> will fetch strings from the database instead of .resx files.
            ////services.AddScoped<ILocalizationService, LocalizationService>();
            ////services.AddSingleton<IStringLocalizerFactory, DatabaseStringLocalizerFactory>();

            // To use both .resx files and database localization:
            // 1. Uncomment "services.AddLocalization" to enable .resx files.
            // 2. Keep the DatabaseStringLocalizerFactory commented out to avoid overriding the default factory.
            // 3. Use ILocalizationService explicitly to fetch strings from the database, while .resx strings remain accessible via IStringLocalizer.
            // Example: Use IStringLocalizer for resources and ILocalizationService for database localization.

            // Configures localization options for the application
            services.Configure<RequestLocalizationOptions>(options =>
            {
                // Sets the default culture to "en" (English) when no culture is explicitly specified in the request
                options.DefaultRequestCulture = new RequestCulture("en");

                // Specifies the cultures supported for formatting (e.g., numbers, dates, currency)
                options.SupportedCultures = new[] { new CultureInfo("en"), new CultureInfo("es") };

                // Specifies the UI cultures supported for resource file localization (e.g., ValidationMessages.en.resx or ValidationMessages.es.resx)
                options.SupportedUICultures = new[] { new CultureInfo("en"), new CultureInfo("es") };

                // Adds culture providers to determine the request's culture dynamically.
                // QueryStringRequestCultureProvider checks the query string for culture parameters (e.g., ?culture=es&ui-culture=es).
                // Inserted at position 0 to give it the highest priority.
                options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());

                // Additional providers (enabled by default):
                // - CookieRequestCultureProvider: Reads culture preferences from cookies.
                // - AcceptLanguageHeaderRequestCultureProvider: Reads culture preferences from the Accept-Language header sent by the browser.
            });

            #endregion

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors(options =>
            {
                // Browsers block AllowAnyOrigin() when credentials like Authorization headers are sent, so origins in the CORS policy are used instead.
                options.AddPolicy("ClientApp", builder =>
                {

#if DEBUG
                    builder.WithOrigins("http://localhost:5170")
                           .AllowCredentials()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
#else
        builder.WithOrigins("https://client-app-url.com")
               .AllowCredentials()
               .AllowAnyHeader()
               .AllowAnyMethod();
#endif

                });

            });

            services.AddHttpContextAccessor();

            services.AddControllers(options =>
            {
                // The ModelBinderProviders are called in the order in which they are registered. For each provider, the framework calls
                // the GetBinder method to determine if the provider can supply a binder for the current model type and binding source.
                // Once a provider returns a binder, that binder is used to bind the model and the pipeline stops processing other
                // providers. If the binder cannot bind the model correctly, an error is raised, and the pipeline bypasses the rest.
                options.Filters.Add<CustomActionFilter>();
                options.ModelBinderProviders.Insert(0, new RouteParameterBinderProvider());
                options.ModelBinderProviders.Insert(1, new QueryStringBinderProvider());
                options.ModelBinderProviders.Insert(2, new JsonBinderProvider());
                // TODO: Implement FormDataBinderProvider to provide localized error messages for form data binding errors.
                // NOTE:
                // - The FormDataBinderProvider is responsible for handling models with BindingSource.Form.
                // - In general, BindingSource.Form applies to the entire form object.
                // - After the form object is bound, each property of the form model is passed through the binding pipeline.
                // - To localize or customize error messages for each individual property, custom binders for the corresponding
                //   property types (e.g., string, int, custom objects) would need to be implemented.
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    // This runs when validation errors occur. Remove errors from ModelState with key "request". The System.Text.Json
                    // serializer adds these messages when it fails to bind JSON to the model parameter (in this project it is always
                    // called "request"). These errors are often redundant or unclear, so they are excluded for cleaner responses.
                    var validationErrors = context.ModelState
                        .Where(x => x.Value != null && x.Key != "request" && x.Value.Errors.Count > 0)
                        .ToDictionary(x => x.Key, x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray())
                        .Where(x => x.Value.Length > 0)
                        .ToDictionary(x => x.Key, x => x.Value);

                    throw new ValidationException(validationErrors);
                };
            });

            services.AddEndpointsApiExplorer();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();

                c.SwaggerDoc("v1", new OpenApiInfo
                {

                    Title = "AuthenticationService",
                    Version = "v1",
                    Description = "A template project designed for efficiently and consistently building new applications using the CQRS pattern with MediatR for request handling and separation of concerns.",
                    Contact = new OpenApiContact()
                    {
                        Name = "Development by: conra.arq@gmail.com"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please insert the JWT token in this format: Bearer {your token here}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                // Uncomment to include XML documentation comments from the project's generated .xml file in Swagger.
                ////var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                ////c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            #region Auth

            // Uncomment to dynamically choose between JWT or Cookie authentication         
            ////services.AddAuthentication(options =>
            ////{
            ////    options.DefaultAuthenticateScheme = "MultiScheme";
            ////    options.DefaultChallengeScheme = "MultiScheme";
            ////})
            ////.AddPolicyScheme("MultiScheme", "MultiScheme", options =>
            ////{
            ////    options.ForwardDefaultSelector = context =>
            ////    {
            ////        return context.Request.Headers["Authorization"].FirstOrDefault()?.StartsWith("Bearer ") == true
            ////            ? JwtBearerDefaults.AuthenticationScheme
            ////            : CookieAuthenticationDefaults.AuthenticationScheme;
            ////    };
            ////})

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
                   ?? throw new InvalidOperationException("JwtSettings section is missing from configuration.");

            //Azure AD(Entra ID) uses cookies to track authentication sessions in the browser.
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Extend session duration
                options.SlidingExpiration = true; // Refresh session on activity
                options.Cookie.HttpOnly = true; // Secure session cookies
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure HTTPS
            });


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            // OAuth 2.0 Google Authentication
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"] ?? throw new InvalidOperationException("Google ClientId is missing from configuration.");
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret is missing from configuration.");
                googleOptions.CallbackPath = configuration["Authentication:Google:RedirectUrl"] ?? throw new InvalidOperationException("Google RedirectUrl is missing from configuration.");
            })
            // Microsoft Entra ID (Azure AD) Login
            .AddOpenIdConnect("Microsoft", options =>
            {
                options.Authority = $"https://login.microsoftonline.com/{configuration["Authentication:Microsoft:TenantId"] ?? throw new InvalidOperationException("Microsoft TenantId is missing from configuration.")}/v2.0";
                options.ClientId = configuration["Authentication:Microsoft:ClientId"] ?? throw new InvalidOperationException("Microsoft ClientId is missing from configuration.");
                options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"] ?? throw new InvalidOperationException("Microsoft ClientSecret is missing from configuration.");
                options.CallbackPath = configuration["Authentication:Microsoft:RedirectUrl"] ?? throw new InvalidOperationException("Microsoft RedirectUrl is missing from configuration.");
                options.ResponseType = "code"; // Use Authorization Code Flow
                options.SaveTokens = true;

                // Uncomment to fetch additional info from Microsoft Graph and add to claims
                ////options.Events.OnTokenValidated = async context =>
                ////{
                ////    var a = await context.HttpContext.AuthenticateAsync();
                ////    var accessToken = context.SecurityToken as JwtSecurityToken;
                ////    if (accessToken == null) return;

                ////    var httpClient = new HttpClient();
                ////    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.RawData);

                ////    var response = await httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");
                ////    if (!response.IsSuccessStatusCode) return;

                ////    var json = await response.Content.ReadAsStringAsync();
                ////    var userData = JsonConvert.DeserializeObject<dynamic>(json);

                ////    var identity = (ClaimsIdentity?)context.Principal?.Identity;
                ////    if (identity != null )
                ////    {
                ////        identity.AddClaim(new Claim(ClaimTypes.GivenName, (string?)userData?.givenName ?? ""));
                ////        identity.AddClaim(new Claim(ClaimTypes.Surname, (string?)userData?.surname ?? ""));
                ////    }
                ////};

                // Uncomment to request specific scopes if needed
                ////options.Scope.Add("api://2534dfed-110d-4ff3-83b1-d0a7cdff6b39/Files.Read");
                ////options.Scope.Add("https://graph.microsoft.com/.default");
                ////options.Scope.Add("openid");
                ////options.Scope.Add("profile");
                ////options.Scope.Add("email");

                //// Uncomment to fetch additional user info via Microsoft Graph API
                ////options.Scope.Add("Files.Read");
                ////options.Scope.Add("User.Read");

                // Uncomment to enable SSO by avoiding unnecessary login prompts
                ////options.Prompt = "none"; // Silent login if already authenticated
            })
            // ASP.NET Core's authentication system automatically maps all claims from IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames
            // to Security.Claims.ClaimTypes (e.g. JwtRegisteredClaimNames.Sub to ClaimTypes.NameIdentifier).
            // When retrieving claims from HttpContext.User, use ClaimTypes instead of JwtRegisteredClaimNames to ensure compatibility.
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Explicitly setting NameClaimType to JwtRegisteredClaimNames.Sub to support OAuth providers
                    // (Google, Azure AD, etc.), as many external providers use "sub" as the unique user identifier.        
                    NameClaimType = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,

                    // Audience and issuer validation
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,

                    // Ensure tokens are signed correctly
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                    // Token expiration validation
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Ensure immediate expiration validation

                    // Uncomment to allow tokens without an exp claim while validating those with exp claim (for testing purposes, not production)
                    ////LifetimeValidator = (notBefore, expires, token, parameters) =>
                    ////{
                    ////    if (!expires.HasValue)
                    ////    {
                    ////        return true;
                    ////    }
                    ////    return DateTime.UtcNow < expires.Value;
                    ////}
                };
            })
            // Cookie Authentication for Web Users
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true; // Prevent JavaScript access
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Enforce HTTPS
                options.Cookie.SameSite = SameSiteMode.Strict; // Prevent CSRF
                options.Cookie.Name = "AuthToken"; // Name of the authentication cookie
                options.ExpireTimeSpan = TimeSpan.FromHours(1); // Cookie expiration
                options.LoginPath = "/account/login"; // Redirect if not authenticated
                options.LogoutPath = "/account/logout";
            });

            services.AddAuthorization(options =>
            {
                // Registering services.AddIdentity() after services.AddAuthentication() overrides the default JWT scheme, 
                // forcing cookie authentication and redirecting unauthorized requests to a login page instead of returning 401.
                // To prevent this, we explicitly set the default authorization policy to require JWT authentication. 
                // This ensures [Authorize] attributes enforce JWT tokens without manually specifying AuthenticationSchemes.
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("EntityOwnershipPolicy", policy =>
                {
                    policy.Requirements.Add(new EntityOwnershipRequirement());
                    // Optionally add a specific entity type and ID parameter for customization.
                    // Use this if the entity name does not match the controller name or the ID parameter is not "id".
                    ////policy.Requirements.Add(new EntityOwnershipRequirement(entityType: typeof(ApplicationRole), idParameterName: "customId"));
                });
            });

            services.AddSingleton<IAuthorizationHandler, EntityOwnershipHandler>();

            #endregion

            return services;
        }
    }
}
