// <copyright file="ConstantNode.cs" company="Alex Lopez-Garcia">
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
    /// Constant Node which has a double.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// Constructor for Constant node that takes in value.
        /// </summary>
        /// <param name="value">Value passed in to put it into the property.</param>
        public ConstantNode(double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets Value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Just gets the value of the constant node.
        /// </summary>
        /// <returns>the double of the node.</returns>
        public override double Evaluate()
        {
            return this.Value;
        }
    }
}
