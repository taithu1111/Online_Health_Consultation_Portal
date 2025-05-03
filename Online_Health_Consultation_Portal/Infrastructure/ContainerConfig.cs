using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.Variance;
using MediatR;
using MediatR.Pipeline;
using Online_Health_Consultation_Portal.Application.CQRS.Command;
using Online_Health_Consultation_Portal.Application.CQRS.Handler.Command;
using Online_Health_Consultation_Portal.Application.CQRS.Handler.Querries;
using Online_Health_Consultation_Portal.Application.CQRS.Querries;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Domain.Interface;
using Online_Health_Consultation_Portal.Infrastructure.Hubs;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Infrastructure
{
    public static class ContainerConfig
    {
        public static ContainerBuilder AddGenericHandlers(this ContainerBuilder builder)
        {
            // Enable contravariant resolution for notification handlers
            builder.RegisterSource(new ContravariantRegistrationSource());

            // MediatR core registrations
            builder.RegisterType<Mediator>()
                   .As<IMediator>()
                   .InstancePerLifetimeScope();

            var handlerAssemblies = new[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(GetMessagesByConversationIdQuery).Assembly,
                typeof(SendMessageCommand).Assembly
            };

            // Pipeline behaviors
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>))
                   .As(typeof(IPipelineBehavior<,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>))
                   .As(typeof(IPipelineBehavior<,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>))
                   .As(typeof(IPipelineBehavior<,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>))
                   .As(typeof(IPipelineBehavior<,>))
                   .InstancePerLifetimeScope();

            // Explicitly register command handlers
            builder.RegisterType<SendMessageCommandHandler>()
                   .As<IRequestHandler<SendMessageCommand, int>>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<MarkMessageAsReadCommandHandler>()
<<<<<<< HEAD
                .As<IRequestHandler<MarkMessageAsReadCommand>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CreatePrescriptionCommandHandler>()
                .As<IRequestHandler<CreatePrescriptionCommand, Prescription>>()
                .InstancePerLifetimeScope();

=======
                   .As<IRequestHandler<MarkMessageAsReadCommand>>()
                   .InstancePerLifetimeScope();
>>>>>>> 738aa228cdb979423fe5ed2525c5e724919a7378

            // Explicitly register query handlers
            builder.RegisterType<GetMessagesByConversationIdQueryHandler>()
                   .As<IRequestHandler<GetMessagesByConversationIdQuery, List<Message>>>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<GetMessageByIdQueryHandler>()
                   .As<IRequestHandler<GetMessageByIdQuery, Message>>()
                   .InstancePerLifetimeScope();
<<<<<<< HEAD
            builder.RegisterType<GetPrescriptionByIdQueryHandler>()
                   .As<IRequestHandler<GetPrescriptionByIdQuery, Prescription>>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<GetPrescriptionsByPatientIdQueryHandler>()
                   .As<IRequestHandler<GetPrescriptionsByPatientIdQuery, List<Prescription>>>()
                   .InstancePerLifetimeScope();
=======
>>>>>>> 738aa228cdb979423fe5ed2525c5e724919a7378

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
<<<<<<< HEAD
            builder.RegisterType<PrescriptionRepository>()
                   .As<IPrescriptionRepository>()
                   .InstancePerLifetimeScope();
=======
>>>>>>> 738aa228cdb979423fe5ed2525c5e724919a7378

            // Register MediatR's request handler delegate factory
            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => componentContext.Resolve(t);
            });

            // Populate ASP.NET Core services
            var services = new ServiceCollection();

            // Add SignalR services
            services.AddSignalR();

            builder.Populate(services);

            return builder;
        }
    }
}
