using System.IO;
using Amazon.Lambda.Core;
using AWSLambda2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambda2
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public FunctionResponse FunctionHandler(FunctionRequest input, ILambdaContext context)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("connectionStrings.json")  // ‡@
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            // ‡A
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<MyDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString(MyDbContext.DefaultConnectionStringName));
            });
            serviceCollection.AddDbContext<MyDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString(MyDbContext.DefaultConnectionStringName)));
            serviceCollection.AddTransient<UserService>();
            serviceCollection.AddLogging(builder => builder.AddSerilog(dispose: true));
            var provider = serviceCollection.BuildServiceProvider();

            // ‡B
            var service = provider.GetService<UserService>();
            var userName = service.FindUser(input.Keyword);
            Log.Logger.Information("hoge");

            return new FunctionResponse
            {
                Result = userName
            };
        }
    }
}
