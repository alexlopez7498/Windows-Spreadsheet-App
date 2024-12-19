// <copyright file="InvalidVariableException.cs" company="Alex Lopez-Garcia">
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
    /// Exception for the spreadsheet to throw when you give an invalid variable name.
    /// </summary>
    public class InvalidVariableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidVariableException"/> class.
        /// </summary>
        public InvalidVariableException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidVariableException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        public InvalidVariableException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidVariableException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        /// <param name="inner">The things that threw it.</param>
        public InvalidVariableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
