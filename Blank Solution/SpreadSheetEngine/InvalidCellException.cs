// <copyright file="InvalidCellException.cs" company="Alex Lopez-Garcia">
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
    /// Exception for the spreadsheet to throw when you give an invalid cell.
    /// </summary>
    public class InvalidCellException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCellException"/> class.
        /// </summary>
        public InvalidCellException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCellException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        public InvalidCellException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCellException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        /// <param name="inner">The things that threw it.</param>
        public InvalidCellException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
