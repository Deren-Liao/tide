﻿// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using EnvDTE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using IOPath = System.IO.Path;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoogleCloudExtension.SolutionUtils
{
    /// <summary>
    /// This class represents a project source file. 
    /// Typlically .cs file.
    /// </summary>
    internal class ProjectSourceFile
    {
        private static readonly string[] s_supportedFileExtension = { ".cs" };

        private readonly ProjectHelper _owningProject;
        private readonly ProjectItem _projectItem;
        private Window _window;

        /// <summary>
        /// The file path.
        /// </summary>
        public readonly string FullName;

        /// <summary>
        /// The path relative to the project root if <seealso cref="ProjectHelper.ProjectRoot"/> is valid.
        /// </summary>
        public readonly Lazy<string> RelativePath;

        /// <summary>
        /// The divided parts of the path. 
        /// example: c:\a\b\c.cs  --> c:, a, b, c.cs
        /// </summary>
        public readonly Lazy<string[]> SubPaths;

        /// <summary>
        /// Initializes an instance of <seealso cref="ProjectSourceFile"/> class.
        /// </summary>
        /// <param name="projectItem">A project item that is physical file.</param>
        /// <param name="project">The container project of type <seealso cref="ProjectHelper"/></param>
        private ProjectSourceFile(ProjectItem projectItem, ProjectHelper project)
        {
            _projectItem = projectItem;
            FullName = _projectItem.FileNames[0].ToLowerInvariant();
            _owningProject = project;
            SubPaths = new Lazy<string[]>(() => FullName.Split(IOPath.DirectorySeparatorChar));
            RelativePath = new Lazy<string>(GetRelativePath);
        }


        ///// <summary>
        ///// Source location information includes file name and line number etc when it is compiled.
        ///// Later on, the solution/project can be opened in different directory.
        ///// This method test if the target source location file name is a possible match to the project item.
        ///// </summary>
        ///// <param name="projectItem">A project item.</param>
        ///// <param name="sourceLocationFilePath">The source location file path.</param>
        ///// <returns></returns>
        //public static bool DoesPathMatch(ProjectItem projectItem, string sourceLocationFilePath)
        //{
        //    if (!IsValidSupportedItem(projectItem))
        //    {
        //        return false;
        //    }


        //    // TODO: Advanced matching algorithm.
        //    return NormalizePath(sourceLocationFilePath) == NormalizePath(projectItem.FileNames[0]);
        //}

        /// <summary>
        /// Verifies if a giving path match the source file item path.
        /// </summary>
        public bool DoesPathMatch(string filePath)
        {
            var path = filePath.ToLowerInvariant();
            return path.EndsWith(RelativePath.Value);
        }

        /// <summary>
        /// Create a <seealso cref="ProjectSourceFile"/> object wrapping up a ProjectItem interface.
        /// Together with private constructor, this ensures object creation won't run into exception. 
        /// </summary>
        /// <param name="projectItem">A project item.</param>
        /// <param name="project">The container project of type <seealso cref="ProjectHelper"/></param>
        /// <returns>
        /// The created object.
        /// Or null if the projectItem is null,  the item is not physical file.
        /// </returns>
        public static ProjectSourceFile Create(ProjectItem projectItem, ProjectHelper project)
        {
            if (!IsValidSupportedItem(projectItem) || project == null)
            {
                return null;
            }

            return new ProjectSourceFile(projectItem, project);
        }

        public Window GotoLine(int line)
        {
            Open();
            TextSelection selection = _window.Document.Selection as TextSelection;
            TextPoint tp = selection.TopPoint;
            selection.GotoLine(line, Select: false);
            return _window;
        }

        private string GetRelativePath()
        {
            if (_owningProject.ProjectRoot != null && FullName.StartsWith(_owningProject.ProjectRoot))
            {
                return FullName.Substring(_owningProject.ProjectRoot.Length);
            }
            else
            {
                // Fallback to compare the root
                int baseLength = 0;
                for (int i = 0; i < SubPaths.Value.Length; ++i)
                {
                    if (_owningProject.SubPaths[i] != SubPaths.Value[i])
                    {
                        break;
                    }
                    baseLength += SubPaths.Value[i].Length + 1;
                }

                return FullName.Substring(baseLength + 1);
            }
        }

        private void Open()
        {
            _window = _projectItem.Open(EnvDTE.Constants.vsViewKindPrimary);  // TODO: should it be Constants.vsViewKindCode ?
            Debug.Assert(_window != null, "If the _window is null, there is a code bug");
            _window.Visible = true;
        }

        private static bool IsValidSupportedItem(ProjectItem projectItem)
        {
            if (EnvDTE.Constants.vsProjectItemKindPhysicalFile != projectItem?.Kind ||
                !s_supportedFileExtension.Contains(IOPath.GetExtension(projectItem.Name).ToLower()))
            {
                return false;
            }

            if (projectItem.FileCount != 1)
            {
                Debug.WriteLine($"project item file count is {projectItem.FileCount}. Expects 1");
                return false;
            }

            return true;
        }
    }
}
