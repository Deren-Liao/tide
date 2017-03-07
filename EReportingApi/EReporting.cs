using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Clouderrorreporting.v1beta1;
using Google.Apis.Clouderrorreporting.v1beta1.Data;
using Google.Apis.Services;
using static Google.Apis.Clouderrorreporting.v1beta1.ProjectsResource.EventsResource;
using static WriteTraceToFile.TextTrace;

namespace EReportingApi
{
    public static class EReporting
    {
        private const string ProjectId = "pacific-wind";

        /// <summary>
        /// Create the Error Reporting service (<seealso cref="ClouderrorreportingService"/>)
        /// with the Application Default Credentials and the proper scopes.
        /// See: https://developers.google.com/identity/protocols/application-default-credentials.
        /// </summary>
        private static ClouderrorreportingService CreateErrorReportingClient()
        {
            // Get the Application Default Credentials.
            GoogleCredential credential = GoogleCredential.GetApplicationDefaultAsync().Result;

            // Add the needed scope to the credentials.
            credential.CreateScoped(ClouderrorreportingService.Scope.CloudPlatform);

            // Create the Error Reporting Service.
            ClouderrorreportingService service = new ClouderrorreportingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
            });
            return service;
        }

        /// <summary>
        /// Creates a <seealso cref="ReportRequest"/> from a given exception.
        /// </summary>
        private static ReportRequest CreateReportRequest(Exception e)
        {
            // Create the service.
            ClouderrorreportingService service = CreateErrorReportingClient();

            // Format the project id to the format Error Reporting expects. See:
            // https://cloud.google.com/error-reporting/reference/rest/v1beta1/projects.events/report
            string formattedProjectId = string.Format("projects/{0}", ProjectId);

            // Add a service context to the report.  For more details see:
            // https://cloud.google.com/error-reporting/reference/rest/v1beta1/projects.events#ServiceContext
            ServiceContext serviceContext = new ServiceContext()
            {
                Service = "e_reporting",
                Version = "the-exceptional-api-version"
            };

            ErrorContext errorContext = new ErrorContext()
            {
                 ReportLocation = new SourceLocation()
                 {
                      FunctionName = "A hidden Name"
                 }
            };

            ReportedErrorEvent errorEvent = new ReportedErrorEvent()
            {
                Message = e.ToString(),
                ServiceContext = serviceContext,
                Context = errorContext
            };
            return new ReportRequest(service, errorEvent, formattedProjectId);
        }

        /// <summary>
        /// Report an exception to the Error Reporting service.
        /// </summary>
        public static void Report(Exception e)
        {
            // Create the report and execute the request.
            ReportRequest request = CreateReportRequest(e);
            request.Execute();
            TestTrace($"");
        }
    }
}
