using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace ContactService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assemblyLocation = Assembly.GetEntryAssembly().Location;
            var assemblyDir = assemblyLocation.Replace(Path.GetFileName(assemblyLocation), string.Empty);

            var config = new ConfigurationBuilder()
                                .SetBasePath(assemblyDir)
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables()
                                .Build();

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .Register(ctx => config)
                .As<IConfigurationRoot>()
                .SingleInstance();

            containerBuilder
                .RegisterType<RabbitConnectionFactory>()
                .As<IConnectionFactory>()
                .SingleInstance();

            containerBuilder
                .Register(ctx => new UTF8Encoding(false))
                .As<Encoding>()
                .SingleInstance();

            containerBuilder
                .Register(ctx => new FakeContactRepository())
                .As<IContactRepository>()
                .SingleInstance();

            containerBuilder
                .Register(ctx => RabbitConfig.Deserialize(ctx.Resolve<IConfigurationRoot>()));

            containerBuilder
                .RegisterType<GetCountHandler>();

            containerBuilder
                .RegisterType<GetContactHandler>();

            containerBuilder
                .RegisterType<CreateContactHandler>();

            using (var container = containerBuilder.Build())
            using (var getCountHander = container.Resolve<GetCountHandler>())
            using (var getHander = container.Resolve<GetContactHandler>())
            using (var createHandler = container.Resolve<CreateContactHandler>())
            {
#if DEBUG
                Console.Clear();

                Console.Out.WriteLine("Starting Event Producer");

                getCountHander.Start();
                getHander.Start();
                createHandler.Start();

                var shouldExit = false;

                do
                {
                    Console.Out.WriteLine();
                    Console.Out.Write("Press \"x\" and <Enter> to quit: ");
                    var line = Console.In.ReadLine();
                    shouldExit = string.Compare("x", line.Trim(), StringComparison.OrdinalIgnoreCase) == 0;
                } while (!shouldExit);

                Console.Out.WriteLine("Stopping Event Producer");

                createHandler.Stop();
                getHander.Stop();
                getCountHander.Stop();

                Console.Out.WriteLine("Quitting");
#else
                getCountHander.Start();
                getHander.Start();
                createHandler.Start();

                while (!Environment.HasShutdownStarted)
                    System.Threading.Thread.Sleep(100);

                createHandler.Stop();
                getHander.Stop();
                getCountHander.Stop();
#endif
            }
        }
    }
}