// <copyright file="Node.cs" company="Alex Lopez-Garcia">
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
    /// abstract class which has a abstract method for each child.
    /// </summary>
    internal abstract class Node
    {
        /// <summary>
        /// Abstract method that all childs will have to override.
        /// </summary>
        /// <returns>a double.</returns>
        public abstract double Evaluate();
    }
}
