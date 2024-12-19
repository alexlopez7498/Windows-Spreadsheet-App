// <copyright file="SameReferenceCellException.cs" company="Alex Lopez-Garcia">
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
    /// Exception for the spreadsheet to throw when the cell trys to reference the same cell.
    /// </summary>
    public class SameReferenceCellException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SameReferenceCellException"/> class.
        /// </summary>
        public SameReferenceCellException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SameReferenceCellException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        public SameReferenceCellException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SameReferenceCellException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        /// <param name="inner">The things that threw it.</param>
        public SameReferenceCellException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
