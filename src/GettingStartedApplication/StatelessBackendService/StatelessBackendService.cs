// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace StatelessBackendService
{
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.Threading;
    using System.Threading.Tasks;
    using global::StatelessBackendService.Interfaces;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Remoting.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;
    using Microsoft.ApplicationInsights.ServiceFabric;
    using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
    using Microsoft.ApplicationInsights.ServiceFabric.Remoting.Activities;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class StatelessBackendService : StatelessService, IStatelessBackendService
    {
        private long iterations = 0;
        private TelemetryClient client;

        public StatelessBackendService(StatelessServiceContext context)
            : base(context)
        {
            client = new TelemetryClient();
            FabricTelemetryInitializerExtension.SetServiceCallContext(this.Context);
        }

        public Task<long> GetCountAsync()
        {
            client.TrackTrace(new TraceTelemetry("Processing GetCount request"));
            return Task.FromResult(this.iterations);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            var telemetryConfig = TelemetryConfiguration.Active;
            //telemetryConfig.InstrumentationKey = "7b60216d-2809-409b-8f2e-3ec921e8f502";

            return new ServiceInstanceListener[1]
            {
                //new ServiceInstanceListener(this.CreateServiceRemotingListener)
                new ServiceInstanceListener(context => new Microsoft.ServiceFabric.Services.Remoting.V1.FabricTransport.Runtime.FabricTransportServiceRemotingListener(context, new CorrelatingRemotingMessageHandler(context, this)))
            };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ++this.iterations;

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", this.iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}