using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Online_Health_Consultation_Portal.Application.CQRS.Command;
using Online_Health_Consultation_Portal.Domain.Interface;
using Online_Health_Consultation_Portal.Infrastructure.Hubs;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Infrastructure
{
    public static class ContainerConfig
    {
        public static ContainerBuilder AddGenericHandlers(this ContainerBuilder builder)
        {
            // Register MediatR with scoped lifetime
            var applicationAssembly = Assembly.GetAssembly(typeof(SendMessageCommand));
            var mediatRConfiguration = MediatRConfigurationBuilder
                .Create(applicationAssembly)
                .WithRegistrationScope(RegistrationScope.Scoped)
                .Build();
            builder.RegisterMediatR(mediatRConfiguration);

            // Register SignalR hub
            builder.RegisterType<ChatHub>()
                .AsSelf()
                .InstancePerLifetimeScope();

            // EF Core DbContext & Repositories
            builder.RegisterType<AppDbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageRepository>()
                .As<IMessageRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<PrescriptionRepository>()
                .As<IPrescriptionRepository>()
                .InstancePerLifetimeScope();

            // Populate ASP.NET Core services
            var services = new ServiceCollection();
            services.AddSignalR();
            builder.Populate(services);

            return builder;
        }
    }
}