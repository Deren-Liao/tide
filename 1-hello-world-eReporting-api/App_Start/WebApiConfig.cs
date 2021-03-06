﻿// Copyright(c) 2016 Google Inc.
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
using static EReportingApi.EReporting;

namespace GoogleCloudSamples
{
    public class ErrorReportingApiTestException : Exception
    {
        public ErrorReportingApiTestException(string message) : base(message) { }
        public ErrorReportingApiTestException(string message, Exception inner) : base(message, inner) { }
    }

    public static class WebApiConfig
    {
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
                log.Info($"EReporting {counter}.");

                if ((counter%2) == 0)
                {
                    try
                    {
                        ThrowException();
                    }
                    catch (ErrorReportingApiTestException ex)
                    {
                        Report(ex);
                        throw;
                    }
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
        }

        public static void ThrowException()
        {
            var ex = ExceptionGenerator.GenerateException(
                () => ExceptionGenerator.LoopThenException(10, "inner exception test from error api"));
            throw new ErrorReportingApiTestException("error api exception test", ex);
        }
        // [END sample]
    }
}
