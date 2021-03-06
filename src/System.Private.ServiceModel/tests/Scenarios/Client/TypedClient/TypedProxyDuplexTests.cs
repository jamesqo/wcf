// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Xunit;

public static class TypedProxyDuplexTests
{
    // ServiceContract typed proxy tests create a ChannelFactory using a provided [ServiceContract] Interface which...
    //       returns a generated proxy based on that Interface.
    // ChannelShape typed proxy tests create a ChannelFactory using a WCF understood channel shape which...
    //       returns a generated proxy based on the channel shape used, such as...
    //              IRequestChannel (for a request-reply message exchange pattern)
    //              IDuplexChannel (for a two-way duplex message exchange pattern)

    [Fact]
    [OuterLoop]
    [ActiveIssue(951, PlatformID.OSX)]
    public static void ServiceContract_TypedProxy_AsyncTask_CallbackReturn()
    {
        DuplexChannelFactory<IWcfDuplexTaskReturnService> factory = null;
        Guid guid = Guid.NewGuid();

        NetTcpBinding binding = new NetTcpBinding();
        binding.Security.Mode = SecurityMode.None;

        DuplexTaskReturnServiceCallback callbackService = new DuplexTaskReturnServiceCallback();
        InstanceContext context = new InstanceContext(callbackService);

        try
        {
            factory = new DuplexChannelFactory<IWcfDuplexTaskReturnService>(context, binding, new EndpointAddress(Endpoints.Tcp_NoSecurity_TaskReturn_Address));
            IWcfDuplexTaskReturnService serviceProxy = factory.CreateChannel();

            Task<Guid> task = serviceProxy.Ping(guid);

            Guid returnedGuid = task.Result;

            Assert.Equal(guid, returnedGuid);

            factory.Close();
        }
        finally
        {
            if (factory != null && factory.State != CommunicationState.Closed)
            {
                factory.Abort();
            }
        }
    }
}
