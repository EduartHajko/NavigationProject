using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Navigation.Application.Extensions;
using Navigation.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.EventHandlers.Domain
{
    public class JourneyCreatedEventHandler
     (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<JourneyCreatedEventHandler> logger)
     : INotificationHandler<JourneyCreatedEvent>
    {
        public async Task Handle(JourneyCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

            if (await featureManager.IsEnabledAsync("JourneyProcessing"))
            {
                var journeyCreatedIntegrationEvent = domainEvent.Journey.ToDto(); // Map Journey to  JourneyDto
                await publishEndpoint.Publish(journeyCreatedIntegrationEvent, cancellationToken);
            }
        }
    }
}
