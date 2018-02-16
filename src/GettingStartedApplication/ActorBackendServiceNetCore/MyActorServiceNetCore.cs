// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.ApplicationInsights.ServiceFabric.Module;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Fabric;

namespace ActorBackendServiceNetCore
{
    internal class MyActorServiceNetCore : ActorService
    {
        public MyActorServiceNetCore(
            StatefulServiceContext context,
            ActorTypeInformation actorTypeInfo,
            Func<ActorService, ActorId, ActorBase> actorFactory = null,
            Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null,
            IActorStateProvider stateProvider = null,
            ActorServiceSettings settings = null)
        : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
        {
            FabricTelemetryInitializerExtension.SetServiceCallContext(this.Context);

            var config = TelemetryConfiguration.Active;
            config.InstrumentationKey = "989f0ae5-5ea1-4756-b739-6d47ce9ed79e";
            config.TelemetryInitializers.Add(FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(this.Context));

            var requestTrackingModule = new ServiceRemotingRequestTrackingTelemetryModule();
            var dependencyTrackingModule = new ServiceRemotingDependencyTrackingTelemetryModule();
            requestTrackingModule.Initialize(config);
            dependencyTrackingModule.Initialize(config);
        }
    }
}
