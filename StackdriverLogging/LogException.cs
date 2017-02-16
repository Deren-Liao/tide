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

using System.Runtime.CompilerServices;
using System.Reflection;

public static class ExceptionalLogging
{
    //private static readonly Lazy<JObject> s_jsonTemplateLazy = new Lazy<JObject>(ReadJsonFromFile);
    //private static JObject s_jsonTemplate => s_jsonTemplateLazy.Value;

    private const string LogId = "log_exception_sunny";
    private const string ProjectId = "pacific-wind";  // TODO: use Api.Gax... like what log4net does.
    private const string MessageFieldName = "message";

    private const string JsonTemplateText =
@"
{
  ""eventTime"": """",
  ""serviceContext"": {
    ""service"": ""frontend"",
    ""version"": ""bf6b5b09b9d3da92c7bf964ab1664fe751104517""
  },
  ""message"": """"
}
";

    //private static JObject ReadJsonFromFile()
    //{
    //    return JObject.Parse(JsonTemplateText);
    //}

    private static ProtoWellKnownTypes.Struct CreateJsonPayload()
    {
        return JsonParser.Default.Parse<ProtoWellKnownTypes.Struct>(JsonTemplateText);
    }

    public static Lazy<LoggingServiceV2Client> _client = new Lazy<LoggingServiceV2Client>(
        () => LoggingServiceV2Client.Create());

    public static void WriteLog(Exception exceptionalSunny)
    {        
        LogName logName = new LogName(ProjectId, LogId);
        var jsonPayload = CreateJsonPayload();
        var value = new ProtoWellKnownTypes.Value() { StringValue = exceptionalSunny.Message };
        jsonPayload.Fields[MessageFieldName] = value;
        LogEntry logEntry = new LogEntry
        {
            LogName = logName.ToString(),
            Severity = LogSeverity.Error,
            JsonPayload = jsonPayload
        };

        MonitoredResource resource = new MonitoredResource { Type = "global" };
        resource.Labels["name"] = "This_is_another_name";

        IDictionary<string, string> entryLabels = new Dictionary<string, string>
        {
            { "size", "large" },
            { "color", "red" }
        };

        _client.Value.WriteLogEntries(LogNameOneof.From(logName), resource, entryLabels, new[] { logEntry }, null);
    }
}
