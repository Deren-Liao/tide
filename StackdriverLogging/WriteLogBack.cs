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

namespace StackdriverLogging
{
    public static class StackdriverLog
    {
        private static readonly Guid _guid = new Guid("C85424B9-1B61-4F28-9CCD-AF0EFDA8115B");
        public static void Main(string[] args)
        {
            WriteLog($"Test writing {DateTime.Now}", _guid);

            // WriteLog();
        }

        #region Error Reporting
        private static JObject ReadJsonFromFile()
        {
            return JObject.Parse(File.ReadAllText(@"json1.json"));

        }



        private static ProtoWellKnownTypes.Struct ReadJson()
        {
            string jstring = File.ReadAllText(@"json1.json");
            return JsonParser.Default.Parse<ProtoWellKnownTypes.Struct>(jstring);
        }

        public static Lazy<LoggingServiceV2Client> _client = new Lazy<LoggingServiceV2Client>(
            () => LoggingServiceV2Client.Create());

        public static void WriteLog()
        {        
            LogName logName = new LogName("mars-148601", "log_id_tide_2");
            LogEntry logEntry = new LogEntry
            {
                LogName = logName.ToString(),
                Severity = LogSeverity.Error,
                JsonPayload = ReadJson()
            };

            MonitoredResource resource = new MonitoredResource { Type = "logging_log" };
            resource.Labels["name"] = "This_is_another_name";

            IDictionary<string, string> entryLabels = new Dictionary<string, string>
            {
                { "size", "large" },
                { "color", "red" }
            };

            _client.Value.WriteLogEntries(LogNameOneof.From(logName), resource, entryLabels, new[] { logEntry }, null);

            Console.WriteLine($"Created log entry {logEntry}.");
        }
        #endregion

        private const string _logid = "log_id_mars_1";
        private const string _projectId = "pacific-wind";   // mars-148601
        private static readonly Lazy<MonitoredResource> _resource = new Lazy<MonitoredResource>(GetResourceType);

        private static MonitoredResource GetResourceType()
        {
            var resource = new MonitoredResource { Type = "gce_instance" };
            resource.Labels["instance_id"] = s_instanceId.Value.ToString();
            resource.Labels["project_id"] = _projectId;
            resource.Labels["zone"] = s_instance.Value.Zone.ToString();
            return resource;
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.NoInlining)]
        public static string WriteLog(
            string msg,
            Guid guid,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0,
            [CallerMemberName] string memberName = "")
        {
            var assembly = Assembly.GetCallingAssembly();
            var assemblyName = assembly.GetName();

            LogName logName = new LogName(_projectId, _logid);
            LogEntry logEntry = new LogEntry
            {
                LogName = logName.ToString(),
                Severity = LogSeverity.Error,
                TextPayload = msg
            };

            IDictionary<string, string> entryLabels = new Dictionary<string, string>
            {
                { "source_file",  sourceFilePath},
                { "source_line", sourceLineNumber.ToString() },
                { "member_name", memberName },
                { "assembly_name", assemblyName.Name},
                { "assembly_version", assemblyName.Version.ToString()},
                { "guid", guid.ToString() }
            };

            _client.Value.WriteLogEntries(LogNameOneof.From(logName), _resource.Value, entryLabels, new[] { logEntry }, null);
            return logEntry.ToString();
        }

        public static string WriteLogV2(
            string msg,
            Guid guid,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0,
            [CallerMemberName] string memberName = "")
        {
            var assembly = Assembly.GetCallingAssembly();
            var assemblyName = assembly.GetName();

            LogName logName = new LogName(_projectId, _logid);
            LogEntry logEntry = new LogEntry
            {
                LogName = logName.ToString(),
                Severity = LogSeverity.Error,
                TextPayload = msg,
                SourceLocation = new LogEntrySourceLocation()
                {
                    File = sourceFilePath,
                    Line = sourceLineNumber,
                    Function = $"[{assembly.FullName}].{memberName}"
                }
            };

            IDictionary<string, string> entryLabels = new Dictionary<string, string>
            {
                { "source_file",  sourceFilePath},
                { "source_line", sourceLineNumber.ToString() },
                { "member_name", memberName },
                { "assembly_name", assemblyName.Name},
                { "assembly_version", assemblyName.Version.ToString()},
                { "guid", guid.ToString() }
            };

            _client.Value.WriteLogEntries(LogNameOneof.From(logName), _resource.Value, entryLabels, new[] { logEntry }, null);
            return logEntry.ToString();
        }

        private static Lazy<Instance> s_instance = new Lazy<Instance>(GetMetadata);
        private static ulong? s_instanceId => s_instance.Value.Id;
        private static string s_instanceName => s_instance.Value.Name;

        private static Instance GetMetadata()
        {
            MetadataClient client = MetadataClient.Create();
            var instance = client.GetInstanceMetadata();
            return instance;
        }
    }
}