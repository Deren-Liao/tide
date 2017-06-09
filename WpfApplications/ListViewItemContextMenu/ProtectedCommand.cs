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

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoogleCloudExtension.Utils
{
    /// <summary>
    /// This class implements the <seealso cref="ICommand"/> interface and wraps the action for the command
    /// in <seealso cref="ErrorHandlerUtils.HandleExceptions(Action)"/> to handle the exceptions that could escape.
    /// </summary>
    public class ProtectedCommand : ICommand
    {
        private bool _canExecuteCommand;
        private Action _action;

        /// <summary>
        /// Initializes a new instance of ProtectedCommand.
        /// </summary>
        /// <param name="handler">The action to execute when the command is executed.</param>
        /// <param name="canExecuteCommand">Whether the command is enabled or not.</param>
        public ProtectedCommand(Action handler, bool canExecuteCommand = true)
        {
            _action = handler;
            this.CanExecuteCommand = canExecuteCommand;
        }


        #region ICommand implementation.

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => CanExecuteCommand;

        public void Execute(object parameter)
        {
            _action();
        }

        #endregion

        /// <summary>
        /// Gets/sets whether the command can be executed or not.
        /// </summary>
        public bool CanExecuteCommand
        {
            get { return _canExecuteCommand; }
            set
            {
                if (_canExecuteCommand != value)
                {
                    _canExecuteCommand = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}