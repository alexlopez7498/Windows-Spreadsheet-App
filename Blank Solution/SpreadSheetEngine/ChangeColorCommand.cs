// <copyright file="ChangeColorCommand.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System.Collections.Generic;
using SpreadsheetEngine;

namespace SpreadSheetEngine
{
    /// <summary>
    /// command class that inherits ICommand to have an execute method.
    /// </summary>
    public class ChangeColorCommand : ICommand
    {
        /// <summary>
        /// A list of cells.
        /// </summary>
        private List<Cell> cells;

        /// <summary>
        /// A list of colors for the cells.
        /// </summary>
        private List<uint> colors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeColorCommand"/> class.
        /// </summary>
        /// <param name="cells">The list of cells to change color.</param>
        /// <param name="colors">The list of colors to apply to each cell.</param>
        public ChangeColorCommand(List<Cell> cells, List<uint> colors)
        {
            this.cells = cells;
            this.colors = colors;
        }

        /// <summary>
        /// Gets the list of cells.
        /// </summary>
        public List<Cell> Cells
        {
            get { return this.cells; }
        }

        /// <summary>
        /// Gets the list of colors.
        /// </summary>
        public List<Cell> Colors
        {
            get { return this.Colors; }
        }

        /// <summary>
        /// Exectutes the command for setting the color.
        /// </summary>
        public void Execute()
        {
            for (int i = 0; i < this.cells.Count; i++)
            {
                this.cells[i].BGColor = this.colors[i];
            }
        }
    }
}
