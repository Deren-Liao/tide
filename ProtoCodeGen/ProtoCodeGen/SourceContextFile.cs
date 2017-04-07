/*
 * Copyright 2017 Google Inc. All Rights Reserved.
 * Use of this source code is governed by a BSD-style
 * license that can be found in the LICENSE file or at
 * https://developers.google.com/open-source/licenses/bsd
 */

using System;
using System.IO;
using Google.Protobuf;
using ProtoWellKnownTypes = Google.Protobuf.WellKnownTypes;

using Google.Devtools.Source.V1;

namespace Google.Cloud.Logging.V2
{
    /// <summary>
    /// Read source-context.json from app domain root.
    /// </summary>
    public class SourceContextFile
    {
        private const string SourceContextFileName = "source-context.json";
        private readonly static Lazy<SourceContextFile> s_instance = 
            new Lazy<SourceContextFile>(() => new SourceContextFile());

        /// <summary>
        /// Best way to get application folder path.
        /// http://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
        /// </summary>
        private static string SourceContextFilePath {
            get
            {
                string root = null;
#if NET_CORE
                var rootAssembly = System.Reflection.Assembly.GetEntryAssembly();
                root = rootAssembly?.Location;
#else
                root = System.AppDomain.CurrentDomain.BaseDirectory;
#endif
                return Path.Combine(root, SourceContextFileName);
            }
        }
        //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SourceContextFileName);

        private SourceContext _sourceContextProto;

        private SourceContextFile() {
            _sourceContextProto = FindSourceContext();
        }

        public SourceContextFile(string fileName)
        {
            string jsonSourceContext;
            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
                {
                    jsonSourceContext = sr.ReadToEnd();
                }
            }
            catch (Exception ex) when (IsIOException(ex))
            {
                return;
            }
            try
            {
                _sourceContextProto = JsonParser.Default.Parse<SourceContext>(jsonSourceContext);
            }
            catch (Exception ex) when (IsProtobufParserException(ex))
            {
                return;
            }
        }

        /// <summary>
        /// The git SHA from Source Context file.
        /// </summary>
        public string GitSha => _sourceContextProto.Git.RevisionId;

        /// <summary>
        /// The git repo URL from Source Context file.
        /// </summary>
        public string GitRepoUrl => _sourceContextProto.Git.Url;

        /// <summary>
        /// The singleton instance of the <seealso cref="SourceContextFile"/> class.
        /// </summary>
        public static SourceContextFile Current => s_instance.Value;

        private static SourceContext FindSourceContext()
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
                return null;
            }
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