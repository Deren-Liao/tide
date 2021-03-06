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
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using IOPath = System.IO.Path;
using System.Linq;
using System.Runtime.InteropServices;
namespace GoogleCloudExtension.SolutionUtils
{
    internal class ProjectHelper
    {
        private const string CSharpProjectKind = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
        private const string AssemblyVersionProperty = "AssemblyVersion";
        private const string AssemblyNameProperty = "AssemblyName";

        private readonly Project _project;

        public List<ProjectSourceFile> SourceFiles => GetSourceFiles();
        public string Name => _project.Name;
        public string Version { get; private set; }
        public string AssemblyName { get; private set; }
        public readonly string FullName;
        public readonly string[] SubPaths;
        public readonly string UniqueName;

        /// <summary>
        /// The project root directory. 
        /// It can be null if <seealso cref="UniqueName"/> is not end of <seealso cref="FullName"/> directory name
        /// </summary>
        public readonly string ProjectRoot;

        private ProjectHelper(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException($"Input parameter {nameof(project)} is null.");
            }

            _project = project;

            try
            {
                FullName = project.FullName.ToLowerInvariant();
                SubPaths = FullName.Split(IOPath.DirectorySeparatorChar);
                UniqueName = _project.UniqueName.ToLowerInvariant();
                int idx = FullName.LastIndexOf(UniqueName);
                if (FullName.Length - idx == UniqueName.Length){
                    ProjectRoot = FullName.Substring(0, idx);
                }

                ParseProperties();
            }
            catch (COMException ex)
            {
                Debug.WriteLine($"{ex}");
            }
        }

        private List<ProjectSourceFile> GetSourceFiles()
        {
            var items = new List<ProjectSourceFile>();
            foreach (ProjectItem projectItem in _project.ProjectItems)
            {
                AddSourceFiles(projectItem, items);
            }

            return items;
        }

        private void AddSourceFiles(ProjectItem projectItem, List<ProjectSourceFile> items)
        {
            var sourceFile = ProjectSourceFile.Create(projectItem, this);
            if (sourceFile != null)
            {
                items.Add(sourceFile);
            }

            foreach (ProjectItem nestedItem in projectItem.ProjectItems)
            {
                AddSourceFiles(nestedItem, items);
            }
        }

        /// <summary>
        /// Create a <seealso cref="ProjectHelper"/> object wrapping up a Project interface.
        /// Together with private constructor, this ensures object creation won't run into exception. 
        /// </summary>
        /// <param name="project">Project interface.</param>
        /// <returns>
        /// The created object.
        /// Or null if the project is null, or not supported project type.
        /// </returns>
        public static ProjectHelper Create(Project project)
        {
            if (!IsValidSupported(project))
            {
                return null;
            }

            return new ProjectHelper(project);
        }

        private static bool IsValidSupported(Project project)
        {
            try
            {
                return project != null && project.Kind == CSharpProjectKind && project.FullName != null && project.Properties != null;
            }
            catch (COMException ex)
            {
                return false;
            }
        }

        private void ParseProperties()
        {
            foreach (Property property in _project.Properties)
            {
                switch (property.Name)
                {
                    case AssemblyNameProperty:
                        AssemblyName = property.Value as string;
                        break;
                    case AssemblyVersionProperty:
                        Version = property.Value as string;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
