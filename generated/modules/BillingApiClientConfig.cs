using System;
using billingapi.Api;

namespace modules
{
    public class BillingApiClientConfig : billingapi.Client.Configuration
    {
        /// <summary>
        /// Sets the Referer header for the API requests.
        /// </summary>
        /// <param name="referer">Referer URL</param>
        public void SetReferer(string referer)
        {
            if (!string.IsNullOrEmpty(referer))
            {
                DefaultHeaders["Referer"] = referer;
            }
        }

        /// <summary>
        /// Sets the X-SaaSus-Referer header for the API requests.
        /// </summary>
        /// <param name="xSaaSusReferer">X-SaaSus-Referer URL</param>
        public void SetXSaaSusReferer(string xSaaSusReferer)
        {
            if (!string.IsNullOrEmpty(xSaaSusReferer))
            {
                DefaultHeaders["X-SaaSus-Referer"] = xSaaSusReferer;
            }
        }
    }
}
