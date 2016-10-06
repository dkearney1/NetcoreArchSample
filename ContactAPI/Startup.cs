using System;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContactAPI
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }
        public IContainer ApplicationContainer{ get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            containerBuilder
                .Register(ctx => Configuration)
                .As<IConfigurationRoot>()
                .SingleInstance();

            containerBuilder
                .Register(ctx => RabbitConfig.Deserialize(ctx.Resolve<IConfigurationRoot>()))
                .SingleInstance();

            containerBuilder
                .RegisterType<RabbitConnectionFactory>()
                .As<IConnectionFactory>()
                .SingleInstance();

            containerBuilder
                .Register(ctx => new UTF8Encoding(false))
                .As<Encoding>()
                .SingleInstance();

            ApplicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
