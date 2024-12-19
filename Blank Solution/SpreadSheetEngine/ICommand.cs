// <copyright file="ICommand.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    /// <summary>
    /// Interface class that that has a an excute for a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// method for classes to have when inheriting this class.
        /// </summary>
        public void Execute();
    }
}
