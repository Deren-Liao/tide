/*
 * Copyright 2017 Google Inc. All Rights Reserved.
 * Use of this source code is governed by a BSD-style
 * license that can be found in the LICENSE file or at
 * https://developers.google.com/open-source/licenses/bsd
 */

using System;
using System.IO;
using System.Linq;
using Google.Protobuf;
using ProtoWellKnownTypes = Google.Protobuf.WellKnownTypes;
using static StackdriverLogging.ApiRandomLog;
using GoogleAspNetCoreMvc_Test;

using System.Collections.Generic;
using Google.Devtools.Source.V1;

namespace Google.Cloud.Logging.V2
{
    /// <summary>
    /// Read source-context.json from app domain root.
    /// </summary>
    public static class SourceContextFile
    {
        #region Debug, remove before checkin
        private static bool s_firstTime = true;
        #endregion

        private const string SourceContextFileName = "source-context.json";
        private ProtoWellKnownTypes.Struct _sourceContext => FindSourceContext();
        private ProtoWellKnownTypes.Struct _git => _sourceContext?.Fields["git"]?.StructValue;
        private readonly static Lazy<SourceContextFile> s_instance = new Lazy<SourceContextFile>(() => new SourceContextFile());

        /// <summary>
        /// Best way to get application folder path.
        /// http://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
        /// </summary>
        private static string SourceContextFilePath {
            get
            {
                string root = null;
                var rootAssembly = System.Reflection.Assembly.GetEntryAssembly();
                if (rootAssembly == null)
                {
                    WriteEntry("git Sha is empty", appendGit: false);
                    return null;
                }

                // Debug , remove before checkin
                root = rootAssembly?.Location;
                #region Debug, remove before checkin
                if (s_firstTime)
                {
                    s_firstTime = false;
                    WriteEntry(
                    $"rootAssembly?.Location is {rootAssembly.Location}",
                    appendGit: false);
                    WriteEntry(
                        $"swwwRootPath is {Startup.swwwRootPath}",
                        appendGit: false);
                    WriteEntry(
                        $"sAppPath is {Startup.sAppPath}",
                        appendGit: false);
                    // 
                    WriteEntry(
                        $"AppContext.BaseDirectory {AppContext.BaseDirectory}",
                        appendGit: false);
                    root = AppContext.BaseDirectory;
                DirectoryInfo dInfo = new DirectoryInfo(root);
                var allFiles = String.Join("; ", dInfo.EnumerateFiles().Select(x => x.Name));
                WriteEntry(
                    $"files under AppContext.BaseDirectory {allFiles}",
                    appendGit: false);
                }
                #endregion

                return Path.Combine(root, SourceContextFileName);
            }
        }
            //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SourceContextFileName);

        private SourceContextFile() { }

        /// <summary>
        /// The git SHA from Source Context file.
        /// </summary>
        public string GitSha => _git?.Fields["revisionId"].StringValue;

        /// <summary>
        /// The git repo URL from Source Context file.
        /// </summary>
        public string GitRepoUrl => _sourceContext?.Fields["url"].StringValue;

        /// <summary>
        /// The singleton instance of the <seealso cref="SourceContextFile"/> class.
        /// </summary>
        public static SourceContextFile Current => s_instance.Value;

        private static ProtoWellKnownTypes.Struct FindSourceContext()
        {
            string sourceContext = ReadSourceContextFile();
            if (sourceContext == null)
            {
                return null;
            }
            try
            {
                return JsonParser.Default.Parse<ProtoWellKnownTypes.Struct>(sourceContext);
            }
            catch (Exception ex) when (IsProtobufParserException(ex))
            {
                return null;
            }
        }


        /// <summary>
        /// Find source context file and open the content.
        /// </summary>
        private static string ReadSourceContextFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(SourceContextFilePath)))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex) when (IsIOException(ex))
            {
                WriteEntry($"IOException {ex.Message}", appendGit: false);
                return null;
            }
        }

        private static IEnumerable<string> BasePaths(string path)
        {

        }

        private static bool IsIOException(Exception ex)
        {
            return ex is FileNotFoundException
                || ex is DirectoryNotFoundException
                || ex is IOException;
        }

        private static bool IsProtobufParserException(Exception ex)
        {
            return ex is InvalidProtocolBufferException
                || ex is InvalidJsonException;
        }
    }
}