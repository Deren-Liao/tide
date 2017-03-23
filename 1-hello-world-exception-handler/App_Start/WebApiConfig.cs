// Copyright(c) 2016 Google Inc.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License. You may obtain a copy of
// the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations under
// the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

using log4net;
using Google.Cloud.Diagnostics.AspNet;
using System.Web.Http.ExceptionHandling;

namespace GoogleCloudSamples
{
    public static class WebApiConfig
    {
        private static void ExceptionalStone()
        {
            throw new Exception("This is pacific application exception.");
        }

        // [START sample]
        /// <summary>
        /// The simplest possible HTTP Handler that just returns "Hello World."
        /// </summary>
        public class HelloWorldHandler : HttpMessageHandler
        {
            private static int counter = 0;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                ++counter;

                // Retrieve a logger for this context.
                ILog log = LogManager.GetLogger(typeof(WebApiConfig));
               
                // Log some information to Google Stackdriver Logging.
                log.Info($"exceptional {counter}.");

                if ((counter%2) == 0)
                {
                    ExceptionalStone();
                }

                return Task.FromResult(new HttpResponseMessage()
                {
                    Content = new ByteArrayContent(Encoding.UTF8.GetBytes("Hello World."))
                });
            }
        };

        public static void Register(HttpConfiguration config)
        {
            var emptyDictionary = new HttpRouteValueDictionary();
            // Add our one HttpMessageHandler to the root path.
            config.Routes.MapHttpRoute("index", "", emptyDictionary, emptyDictionary,
                new HelloWorldHandler());

            // Add a catch all for the uncaught exceptions.
            string projectId = "pacific-wind";
            string serviceName = "myservice";
            string version = "version1";
            // Add a catch all for the uncaught exceptions.
            config.Services.Add(typeof(IExceptionLogger),
                ErrorReportingExceptionLogger.Create(projectId, serviceName, version));
        }
        // [END sample]
    }
}
