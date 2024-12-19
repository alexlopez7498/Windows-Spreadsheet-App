// <copyright file="NoVariableValueException.cs" company="Alex Lopez-Garcia">
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
    /// Exception for the spreadsheet to throw when a variable doesn't have a value.
    /// </summary>
    public class NoVariableValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoVariableValueException"/> class.
        /// </summary>
        public NoVariableValueException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoVariableValueException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        public NoVariableValueException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoVariableValueException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        /// <param name="inner">The things that threw it.</param>
        public NoVariableValueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
