using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Google.Cloud.Logging.V2;
using Google.Cloud.Logging.Type;
using Google.Cloud.Metadata.V1;
using Google.Api;
using Google.Apis.Compute.v1.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Google.Protobuf;
using ProtoWellKnownTypes = Google.Protobuf.WellKnownTypes;

using static WriteTraceToFile.TextTrace;

public static class ExceptionalLogging
{
    private const string LogId = "log_exception_sunny";
    private const string ProjectId = "pacific-wind";  // TODO: use Api.Gax... like what log4net does.
    private const string MessageFieldName = "message";

    private const string JsonTemplateText =
@"
{
  ""eventTime"": """",
  ""serviceContext"": {
    ""service"": ""tytan-1"",
    ""version"": ""v2.1""
  },
  ""message"": """"
}
";

    private static ProtoWellKnownTypes.Struct CreateJsonPayload()
    {
        return JsonParser.Default.Parse<ProtoWellKnownTypes.Struct>(JsonTemplateText);
    }

    public static Lazy<LoggingServiceV2Client> _client = new Lazy<LoggingServiceV2Client>(
        () => LoggingServiceV2Client.Create());

    public static void WriteExceptionalLog(Exception exceptionalSunny)
    {        
        LogName logName = new LogName(ProjectId, LogId);
        var jsonPayload = CreateJsonPayload();
        var value = new ProtoWellKnownTypes.Value() { StringValue = exceptionalSunny.ToString() };
        jsonPayload.Fields[MessageFieldName] = value;
        LogEntry logEntry = new LogEntry
        {
            LogName = logName.ToString(),
            Severity = LogSeverity.Error,
            JsonPayload = jsonPayload
        };

        MonitoredResource resource = new MonitoredResource { Type = "global" };
        // global does not use label.
        // resource.Labels["name"] = "This_is_another_name";

        IDictionary<string, string> entryLabels = new Dictionary<string, string>
        {
            { "size", "large" },
            { "color", "red" }
        };

        _client.Value.WriteLogEntries(LogNameOneof.From(logName), resource, entryLabels, new[] { logEntry }, null);
        TestTrace($"Written entry {logEntry.ToString()}");
    }
}
