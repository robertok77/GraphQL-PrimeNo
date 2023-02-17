
using Application.GraphQL_PrimeNo.Schema;
using Application.GraphQL_PrimeNo.Service;
using Application.PrimeNumbers.Service;
using Domain.PrimeNumbers.Contract;
using Domain.PrimeNumbers.Domain;
using GraphQL;

namespace ServerAPI.GraphQL_PrimeNo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton<IPrimeNoService, PrimeNoService>();
        builder.Services.AddSingleton<IConsumerProducerPrimesService, ConsumerProducerPrimesService>();
        builder.Services.AddTransient<IEratosthenesSieveAlgorithm, EratosthenesSieveAlgorithm>();
        builder.Services.AddHostedService<EratosthenesSieveBackgroudService>();
        builder.Services.AddGraphQL(b => b.AddAutoSchema<Query>(s=>s
                .WithMutation<Mutation>()
                .WithSubscription<Subscription>())
            .AddSystemTextJson());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseDeveloperExceptionPage();
        app.UseWebSockets();

        app.UseGraphQL("/graphql");
        // configure Playground at "/"
        app.UseGraphQLPlayground(
            "/",
            new GraphQL.Server.Ui.Playground.PlaygroundOptions
            {
                GraphQLEndPoint = "/graphql",
                SubscriptionsEndPoint = "/graphql",
            });


        app.Run();
    }
}
