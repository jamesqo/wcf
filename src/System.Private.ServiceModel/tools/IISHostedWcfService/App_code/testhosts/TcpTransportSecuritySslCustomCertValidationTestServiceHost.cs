﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace WcfService
{
    public class TcpTransportSecuritySslCustomCertValidationTestServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            TcpTransportSecuritySslCustomCertValidationTestServiceHost serviceHost = new TcpTransportSecuritySslCustomCertValidationTestServiceHost(serviceType, baseAddresses);
            return serviceHost;
        }
    }
    public class TcpTransportSecuritySslCustomCertValidationTestServiceHost : TestServiceHostBase<IWcfService>
    {
        protected override string Address { get { return "tcp-server-ssl-security-clientcredentialtype-certificate-customvalidator"; } }

        protected override Binding GetBinding()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            return binding;
        }

        protected override void ApplyConfiguration()
        {
 	        base.ApplyConfiguration();

            string thumbprint = Util.CertificateFromFridendlyName(StoreName.My, StoreLocation.LocalMachine, "WCF Bridge - Machine certificate generated by the CertificateManager").Thumbprint;

            this.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            this.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new MyX509CertificateValidator("DO_NOT_TRUST_WcfBridgeRootCA");
            this.Credentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine,
                                                      StoreName.My,
                                                      X509FindType.FindByThumbprint,
                                                      thumbprint);
        }

        public TcpTransportSecuritySslCustomCertValidationTestServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }
    }
}
