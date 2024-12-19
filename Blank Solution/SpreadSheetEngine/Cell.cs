// <copyright file="Cell.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System.ComponentModel;
using SpreadSheetEngine;

namespace SpreadsheetEngine
{
    /// <summary>
    /// An abstract cell class that acts as a broadcaster.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// Protected text field for the text of a cell.
        /// </summary>
        protected string? text;

        /// <summary>
        /// Protected value field for the value of a cell.
        /// </summary>
        protected string? value;

        /// <summary>
        /// The background color of the cell.
        /// </summary>
        protected uint bgColor;

        /// <summary>
        /// A list of cells that are dependent on this cell.
        /// </summary>
        private List<Cell> dependentCells;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// Cell constructor that takes in row index and column index.
        /// </summary>
        /// <param name="rowIndex">row index parameter that takes row index.</param>
        /// <param name="columnIndex">column index parameter that takes column index.</param>
        protected Cell(int rowIndex, int columnIndex)
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.dependentCells = new List<Cell>();
            this.bgColor = 0xFFFFFFFF;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { }; // styleCop changed this line to this.

        /// <summary>
        /// Gets rowIndex property to get the row index of the cell.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Gets or Sets the list of referenced cells.
        /// </summary>
        public List<Cell> DependentCells
        {
            get => this.dependentCells;
            set
            {
                this.dependentCells = value;
            }
        }

        /// <summary>
        /// Gets ColumnIndex property to get the column index of the cell.
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        /// Gets or sets text Property that has a getter and setter that is boardcasts a signal if it changes.
        /// </summary>
        public string? Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value == this.text)
                {
                    return;
                }

                this.text = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text")); // StyleCop changed to this format.
            }
        }

        /// <summary>
        /// Gets value property that just gets the value.
        /// </summary>
        public string? Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets or sets the background color Property that has a getter and setter that is boardcasts a signal if it changes..
        /// </summary>
        public uint BGColor
        {
            get
            {
                return this.bgColor;
            }

            set
            {
                if (value == this.bgColor)
                {
                    return;
                }

                this.bgColor = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
            }
        }

        /// <summary>
        /// recrusive function to check if theres a ciruclar reference.
        /// </summary>
        /// <param name="startCell">the original starting point of the cells cycle.</param>
        /// <exception cref="CircularReferenceException">throws exception if circular refrence is found.</exception>
        public void CircularReferenceChecker(Cell startCell)
        {
            if (this.dependentCells.Contains(startCell))
            {
                throw new CircularReferenceException();
            }
            else
            {
                foreach (Cell cell in this.dependentCells)
                {
                    cell.CircularReferenceChecker(startCell);
                }
            }
        }
    }
}