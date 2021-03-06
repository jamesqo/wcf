﻿//  Copyright (c) Microsoft Corporation.  All Rights Reserved.
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.Text;

namespace WcfService
{
    public class HttpSoap11TestServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            HttpSoap11TestServiceHost serviceHost = new HttpSoap11TestServiceHost(serviceType, baseAddresses);
            return serviceHost;
        }
    }
    public class HttpSoap11TestServiceHost : TestServiceHostBase<IWcfService>
    {
        protected override string Address { get { return "http-soap11"; } }

        protected override Binding GetBinding()
        {
            return new CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpTransportBindingElement());
        }

        public HttpSoap11TestServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }
    }
}
