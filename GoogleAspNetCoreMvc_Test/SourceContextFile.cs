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
        private static Lazy<string> s_sourceContextFilePath = new Lazy<string>(FindSourceContextFile);
        private static Lazy<SourceContext> s_sourceContext = new Lazy<SourceContext>(OpenParseSourceContextFile);

        /// <summary>
        /// The source context file name.
        /// </summary>
        public const string SourceContextFileName = "source-context.json";

        /// <summary>
        /// Gets the source context file path if it exists.
        /// Returns null if the file is not found at root path.
        /// </summary>
        public static string SourceContextFilePath => s_sourceContextFilePath.Value;

        /// <summary>
        /// Returns the <seealso cref="SourceContext"/> object deserealized from the source context file.
        /// Returns null if the file does not exist, or failed to open/parse the file.
        /// </summary>
        public static SourceContext SourceContext => s_sourceContext.Value;

        private static SourceContext OpenParseSourceContextFile()
        {
            string sourceContext = ReadSourceContextFile();
            if (sourceContext == null)
            {
                return null;
            }

            try
            {
                return JsonParser.Default.Parse<SourceContext>(sourceContext);
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

        private static string FindSourceContextFile()
        {
            string root = AppContext.BaseDirectory;

            #region Debug, remove before checkin
            if (s_firstTime)
            {
                var rootAssembly = System.Reflection.Assembly.GetEntryAssembly();

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
                DirectoryInfo dInfo = new DirectoryInfo(root);
                var allFiles = String.Join("; ", dInfo.EnumerateFiles().Select(x => x.Name));
                WriteEntry(
                    $"files under AppContext.BaseDirectory {allFiles}",
                    appendGit: false);

                if (string.IsNullOrWhiteSpace(root))
                {
                    WriteEntry("root is empty", appendGit: false);
                    return null;
                }
            }
            #endregion

            var fullPath = Path.Combine(root, SourceContextFileName);
            return File.Exists(fullPath) ? fullPath : null;
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