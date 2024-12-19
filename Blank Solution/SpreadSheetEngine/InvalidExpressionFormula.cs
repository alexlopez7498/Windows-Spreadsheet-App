// <copyright file="InvalidExpressionFormula.cs" company="Alex Lopez-Garcia">
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
    /// Exception for the spreadsheet to throw when you give an invalid expression formula.
    /// </summary>
    public class InvalidExpressionFormula : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpressionFormula"/> class.
        /// </summary>
        public InvalidExpressionFormula()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpressionFormula"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        public InvalidExpressionFormula(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpressionFormula"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        /// <param name="inner">The things that threw it.</param>
        public InvalidExpressionFormula(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
