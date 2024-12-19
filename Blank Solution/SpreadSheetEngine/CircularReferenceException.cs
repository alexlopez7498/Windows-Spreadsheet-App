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
    public class CircularReferenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircularReferenceException"/> class.
        /// </summary>
        public CircularReferenceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularReferenceException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        public CircularReferenceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCellException"/> class.
        /// </summary>
        /// <param name="message">details of exceptions.</param>
        /// <param name="inner">The things that threw it.</param>
        public CircularReferenceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
