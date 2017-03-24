using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Cloud.Logging.V2;
using Google.Cloud.Logging.Type;
using Google.Api;

namespace StackdriverLogging
{
    public static class ApiRandomLog
    {
        public static Lazy<LoggingServiceV2Client> _client = 
            new Lazy<LoggingServiceV2Client>(() => {
                return LoggingServiceV2Client.Create();
            });

        private const string LogId = "log_core_sunny";
        private const string ProjectId = "pacific-wind";  // TODO: use Api.Gax... like what log4net does.

        static public string WriteRandomEntry()
        {
            var msg = RandomString.Next();
            WriteEntry(msg);
            return msg;
        }

        static public void WriteEntry(string message, bool appendGit = true)
        {
            LogName logName = new LogName(ProjectId, LogId);

            LogEntry log = new LogEntry
            {
                LogName = logName.ToString(),
                Severity = LogSeverity.Info,
                TextPayload = message
            };

            IDictionary<string, string> entryLabels = new Dictionary<string, string>
            {
                { "size", "large" },
                { "color", "red" }
            };

            if (appendGit)
            {
                Modify_WriteLogEntriesRequest(ref entryLabels);
            }

            MonitoredResource resource = new MonitoredResource { Type = "global" };
            _client.Value.WriteLogEntries(
                LogNameOneof.From(logName), 
                resource, 
                entryLabels,
                new[] { log }, 
                null);
        }

        private const string SourceContextIDLabel = "source_context_id";
        private const string SecondarySourceContextIDLabel = "gcloud_source_context_id";

        private static void Modify_WriteLogEntriesRequest(ref IDictionary<string, string> labels)
        {
            var gitSha = SourceContextFile.SourceContext?.Git?.RevisionId;
            if (gitSha == null)
            {
                WriteEntry("git Sha is empty", appendGit: false);
                return;
            }

            labels.Add(SourceContextIDLabel, gitSha);
        }
    }
}
