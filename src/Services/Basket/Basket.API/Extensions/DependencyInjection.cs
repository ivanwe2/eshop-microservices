using Basket.API.Data;
using Discount.Grpc.Protos;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Basket.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(config =>
                 {
                     config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                     config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                     config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                 })
                .AddMarten(options =>
                {
                    options.Connection(configuration.GetConnectionString("Database")!);
                    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
                }).ApplyAllDatabaseChangesOnStartup().UseLightweightSessions();


            services.AddScoped<IBasketRepository, BasketRepository>();
            //Best Practice to decorate
            services.Decorate<IBasketRepository, CachedBasketRepository>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                //options.InstanceName = "Basket";
            });
            return services;
        }
        public static IServiceCollection AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
            {
                options.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]!);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                return handler;
            });
            return services;
        }
        public static IServiceCollection AddCrossCuttingConcernsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddExceptionHandler<CustomExceptionHandler>()
                .AddCarter()
                .AddValidatorsFromAssembly(typeof(Program).Assembly);

            services.AddHealthChecks()
                            .AddNpgSql(configuration.GetConnectionString("Database")!)
                            .AddRedis(configuration.GetConnectionString("Redis")!);

            return services;
        }
    }
}
