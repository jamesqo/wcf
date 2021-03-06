// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Claims;
using System.Runtime;
using System.Security.Principal; 

namespace System.ServiceModel
{
    public class SpnEndpointIdentity : EndpointIdentity
    {
        private static TimeSpan _spnLookupTime = TimeSpan.FromMinutes(1);

        public SpnEndpointIdentity(string spnName)
        {
            if (spnName == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("spnName");

            base.Initialize(Claim.CreateSpnClaim(spnName));
        }

        public SpnEndpointIdentity(Claim identity)
        {
            if (identity == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("identity");

            if (!identity.ClaimType.Equals(ClaimTypes.Spn))
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument(SR.Format(SR.UnrecognizedClaimTypeForIdentity, identity.ClaimType, ClaimTypes.Spn));

            base.Initialize(identity);
        }

        public static TimeSpan SpnLookupTime
        {
            get
            {
                return _spnLookupTime;
            }
            set
            {
                if (value.Ticks < 0)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                        new ArgumentOutOfRangeException("value", value.Ticks, SR.Format(SR.ValueMustBeNonNegative)));
                }
                _spnLookupTime = value;
            }            
        }
    }
}
