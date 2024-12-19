// <copyright file="OperatorNode.cs" company="Alex Lopez-Garcia">
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
    /// abstract class for nodes to inherit.
    /// </summary>
    internal abstract class OperatorNode : Node
    {
        /// <summary>
        /// Gets the operator.
        /// </summary>
        public abstract char Operator { get; }

        /// <summary>
        /// Gets the precedence level.
        /// </summary>
        public abstract int Precedence { get; }

        /// <summary>
        /// Gets the associativity.
        /// </summary>
        public abstract string Associativity { get; }

        /// <summary>
        /// Gets or sets left Child of operator node.
        /// </summary>
        public Node? Left { get; set; }

        /// <summary>
        /// Gets or sets right Child of operator node.
        /// </summary>
        public Node? Right { get; set; }

        /// <summary>
        /// Gets the precedence level of the operator.
        /// </summary>
        /// <returns>The precedence level as an integer.</returns>
        public abstract int GetPrecedence();
    }
}
