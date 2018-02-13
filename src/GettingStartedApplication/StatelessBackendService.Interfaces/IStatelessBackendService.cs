// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

//[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace StatelessBackendService.Interfaces
{
    public interface IStatelessBackendService : IService
    {
        Task<long> GetCountAsync();
    }
}