// <copyright file="SubtractNode.cs" company="Alex Lopez-Garcia">
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
    /// Subtracts Node which inherits the operator Node.
    /// </summary>
    internal class SubtractNode : OperatorNode
    {
        /// <summary>
        /// Gets the operator character for addition.
        /// </summary>
        public override char Operator => '-';

        /// <summary>
        /// Gets the precedence level of the addition operator.
        /// </summary>
        public override int Precedence => 1;

        /// <summary>
        /// Gets the associativity of the addition operator, which is left.
        /// </summary>
        public override string Associativity => "Left";

        /// <summary>
        /// Gets the precedence level of the addition operator.
        /// </summary>
        /// <returns>The precedence level as an integer.</returns>
        public override int GetPrecedence()
        {
            return 1; // Precedence level for addition
        }

        /// <summary>
        /// override Evaluate method which subtracts the childs.
        /// </summary>
        /// <returns>The differance of the childs.</returns>
        public override double Evaluate()
        {
            if (this.Left != null && this.Right != null)
            {
                return this.Left.Evaluate() - this.Right.Evaluate();
            }
            else
            {
                return 0;
            }
        }
    }
}
