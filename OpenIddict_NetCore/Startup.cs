using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation;

namespace OpenIddict_NetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<DbContext>(options =>
            {
                // Configure the context to use an in-memory store.
                options.UseInMemoryDatabase(nameof(DbContext));
                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });

            services.AddOpenIddict()
                // Register the OpenIddict core services.
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the EF Core stores/models.
                    options.UseEntityFrameworkCore()
                            .UseDbContext<DbContext>();
                })
                // Register the OpenIddict server handler.
                .AddServer(options =>
                {
                            // Register the ASP.NET Core MVC services used by OpenIddict.
                            // Note: if you don't call this method, you won't be able to
                            // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                            options.UseMvc();
                            // Enable the token endpoint.
                            options.EnableTokenEndpoint("/connect/token");
                            // Enable the password flow.
                            options.AllowPasswordFlow();
                            // Accept anonymous clients (i.e clients that don't send a client_id).
                            options.AcceptAnonymousClients();
                            // During development, you can disable the HTTPS requirement.
                            options.DisableHttpsRequirement();
                })
                // Register the OpenIddict validation handler.
                // Note: the OpenIddict validation handler is only compatible with the
                // default token format or with reference tokens and cannot be used with
                // JWT tokens. For JWT tokens, use the Microsoft JWT bearer handler.
                .AddValidation();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
