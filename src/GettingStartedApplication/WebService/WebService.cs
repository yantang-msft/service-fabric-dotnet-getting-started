// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace WebService
{
    using System.Collections.Generic;
    using System.Fabric;
    using System.IO;
    using System.Net.Http;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.ServiceFabric;
    using Microsoft.ApplicationInsights.ServiceFabric.Module;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;

    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class WebService : StatelessService
    {
        public WebService(StatelessServiceContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                //new ServiceInstanceListener(
                //    serviceContext =>
                //        new WebListenerCommunicationListener(
                //            serviceContext,
                //            "ServiceEndpoint",
                //            (url, listener) =>
                //            {
                //                ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting WebListener on {url}");

                //                return new WebHostBuilder()
                //                    .UseWebListener()
                //                    .ConfigureServices(
                //                        services => services
                //                            .AddSingleton<ConfigSettings>(new ConfigSettings(serviceContext))
                //                            .AddSingleton<HttpClient>(new HttpClient())
                //                            .AddSingleton<FabricClient>(new FabricClient())
                //                            .AddSingleton<StatelessServiceContext>(serviceContext)
                //                            .AddSingleton<ITelemetryInitializer>((serviceProvider) => FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(serviceContext))
                //                            .AddSingleton<ITelemetryModule>(new ServiceRemotingDependencyTrackingTelemetryModule())
                //                            .AddSingleton<ITelemetryModule>(new ServiceRemotingRequestTrackingTelemetryModule())
                //                            )
                //                    .UseContentRoot(Directory.GetCurrentDirectory())
                //                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                //                    .UseStartup<Startup>()
                //                    .UseUrls(url)
                //                    .UseApplicationInsights()
                //                    .Build();
                //            }))

                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<ConfigSettings>(new ConfigSettings(serviceContext))
                                            .AddSingleton<HttpClient>(new HttpClient())
                                            .AddSingleton<FabricClient>(new FabricClient())
                                            .AddSingleton<StatelessServiceContext>(serviceContext)
                                            .AddSingleton<ITelemetryInitializer>((serviceProvider) => FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(serviceContext))
                                            .AddSingleton<ITelemetryModule>(new ServiceRemotingDependencyTrackingTelemetryModule())
                                            .AddSingleton<ITelemetryModule>(new ServiceRemotingRequestTrackingTelemetryModule())
                                            )
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .UseApplicationInsights()
                                    .Build();
                    }))
            };
        }
    }
}