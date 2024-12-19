// <copyright file="SetTextCommand.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetEngine;

namespace SpreadSheetEngine
{
    /// <summary>
    /// command class that inherits ICommand to have an execute method.
    /// </summary>
    public class SetTextCommand : ICommand
    {
        /// <summary>
        /// cell that will be used.
        /// </summary>
        private Cell? cell;

        /// <summary>
        /// the text that will be set for the cell.
        /// </summary>
        private string? text;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetTextCommand"/> class.
        /// </summary>
        /// <param name="cell">The cell when the command was called.</param>
        /// <param name="text">The text to set the text of the cell.</param>
        public SetTextCommand(Cell? cell, string? text)
        {
            if (text == null)
            {
                text = string.Empty;
            }

            this.text = text;
            this.cell = cell;
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        public Cell? Cell
        {
            get
            {
                return this.cell;
            }
        }

        /// <summary>
        /// Exectutes the command for setting the text.
        /// </summary>
        public void Execute()
        {
            if (this.cell != null)
            {
                this.cell.Text = this.text;
            }
        }
    }
}
