// <copyright file="SpreadSheet.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SpreadSheetEngine;

namespace SpreadsheetEngine
{
    /// <summary>
    /// SpreadSheet class that creates a 2d array of cells.
    /// </summary>
    public class SpreadSheet
    {
        /// <summary>
        /// A 2d array of cells.
        /// </summary>
        private Cell[,] sheetCells;

        /// <summary>
        /// The stack in which undo commands are stored.
        /// </summary>
        private Stack<ICommand> undoStack;

        /// <summary>
        /// The stack in which undo commands are stored.
        /// </summary>
        private Stack<ICommand> redoStack;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadSheet"/> class.
        /// Constructor for the SpreadSheet class which takes in columnCount and rowCount.
        /// </summary>
        /// <param name="columnCount"> Takes in columnCount for the number of columns. </param>
        /// <param name="rowCount"> Takes in rowCount for the number of rows. </param>
        public SpreadSheet(int columnCount, int rowCount)
        {
            this.ColumnCount = columnCount;
            this.RowCount = rowCount;
            this.sheetCells = new Cell[this.RowCount, this.ColumnCount];
            this.undoStack = new Stack<ICommand>();
            this.redoStack = new Stack<ICommand>();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    this.sheetCells[i, j] = new SpreadSheetCell(i, j, string.Empty);
                    this.sheetCells[i, j].PropertyChanged += this.CellPropertyChanged; // subscribe to each cell.
                }
            }
        }

        /// <summary>
        /// PropertyChanged delegate list that connects with the UI.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets or sets columnCount property to get and set the columns count.
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Gets or sets rowCount property to get and set the rows count.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// GetCell method that gets a cell depending on the row and column index and if not found
        /// then it returns null.
        /// </summary>
        /// <param name="rowIndex"> takes in row index.</param>
        /// <param name="columnIndex">takes in column index.</param>
        /// <returns>Either returns null or the cell is getting found.</returns>
        public Cell? GetCell(int rowIndex, int columnIndex)
        {
            if (rowIndex >= 0 && rowIndex < this.RowCount && columnIndex >= 0 && columnIndex < this.ColumnCount)
            {
                return this.sheetCells[rowIndex, columnIndex];
            }
            else
            {
                throw new InvalidCellException();
            }
        }

        /// <summary>
        /// returns the top command of the undo stack.
        /// </summary>
        /// <returns>top command of the undo stack or null if empty.</returns>
        public object? PeekUndo()
        {
            if (this.undoStack.Count > 0)
            {
                return this.undoStack.Peek();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// returns the top command of the redo stack.
        /// </summary>
        /// <returns>top command of the redo stack or null if empty.</returns>
        public object? PeekRedo()
        {
            if (this.redoStack.Count > 0)
            {
                return this.redoStack.Peek();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Undos the command that was last done.
        /// </summary>
        public void Undo()
        {
            if (this.undoStack.Count > 0)
            {
                // pop to command from stack.
                object command = this.undoStack.Pop();

                switch (command)
                {
                    // if the command was a text command
                    case SetTextCommand setTextCommand:
                        if (setTextCommand.Cell != null)
                        {
                            this.AddToRedo(new SetTextCommand(setTextCommand.Cell, setTextCommand.Cell.Text));
                        }

                        setTextCommand.Execute();
                        break;

                    // if command was a color command.
                    case ChangeColorCommand setColorCommand:
                        List<Cell> cells = new List<Cell>();
                        List<uint> initialColors = new List<uint>();

                        foreach (Cell cell in setColorCommand.Cells)
                        {
                            cells.Add(cell);
                            initialColors.Add(cell.BGColor);  // Store the current color before redoing
                        }

                        this.AddToRedo(new ChangeColorCommand(cells, initialColors));

                        setColorCommand.Execute();
                        break;
                }
            }
        }

        /// <summary>
        /// Redos the command that was last done.
        /// </summary>
        public void Redo()
        {
            if (this.redoStack.Count > 0)
            {
                // pop to command from stack.
                object command = this.redoStack.Pop();

                switch (command)
                {
                    // if the command was a text command
                    case SetTextCommand setTextCommand:
                        if (setTextCommand.Cell != null)
                        {
                            this.AddToUndo(new SetTextCommand(setTextCommand.Cell, setTextCommand.Cell.Text));
                        }

                        setTextCommand.Execute();
                        break;

                    // if command was a color command.
                    case ChangeColorCommand setColorCommand:
                        List<Cell> cells = new List<Cell>();
                        List<uint> initialColors = new List<uint>();

                        foreach (Cell cell in setColorCommand.Cells)
                        {
                            cells.Add(cell);
                            initialColors.Add(cell.BGColor);
                        }

                        this.AddToUndo(new ChangeColorCommand(cells, initialColors));

                        setColorCommand.Execute();
                        break;
                }
            }
        }

        /// <summary>
        /// saves a spreadsheet from a xml file.
        /// </summary>
        /// <param name="fileStream">pointer to a file that will be written to.</param>
        public void Save(StreamWriter fileStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Async = true;
            int column = 0;
            char columnChar;
            using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
            {
                writer.WriteStartElement("spreadsheet");
                foreach (Cell cell in this.sheetCells)
                {
                    if (cell.Text != null || cell.BGColor != 0xFFFFFFFF)
                    {
                        column = cell.ColumnIndex + 'A';
                        columnChar = (char)column;
                        string saveCell = columnChar.ToString() + (cell.RowIndex + 1);

                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", saveCell);
                        writer.WriteElementString("text", cell.Text);
                        writer.WriteElementString("color", cell.BGColor.ToString("X8"));

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// loads a spreadsheet from a xml file.
        /// </summary>
        /// <param name="fileStream">pointer to a file that will be read.</param>
        public void Load(StreamReader fileStream)
        {
            foreach (Cell cell in this.sheetCells)
            {
                cell.Text = string.Empty;
                cell.BGColor = 0xFFFFFFFF;
            }

            this.redoStack.Clear();
            this.undoStack.Clear();

            XDocument doc = XDocument.Load(fileStream);

            foreach (var element in doc.Descendants("cell"))
            {
                string? name = element.Element("name")?.Value;
                if (!string.IsNullOrEmpty(name))
                {
                    int row = int.Parse(name.Substring(1)) - 1;
                    int column = name[0] - 65;
                    Cell? cell = this.GetCell(row, column);
                    if (cell != null)
                    {
                        cell.Text = element.Element("text")?.Value;
                        cell.BGColor = Convert.ToUInt32(element.Element("color")?.Value, 16);
                    }
                }
            }

            foreach (SpreadSheetCell cell in this.sheetCells)
            {
                this.EvaulateFormula(cell);
            }
        }

        /// <summary>
        /// Pushes a command to the Undo Stack.
        /// </summary>
        /// <param name="command">The command to push.</param>
        public void AddToUndo(ICommand command)
        {
            this.undoStack.Push(command);
        }

        /// <summary>
        /// Checks to see if the stack is empty or not.
        /// </summary>
        /// <returns>returns true if theres something in the stack otherwise false.</returns>
        public bool GetSizeOfUndo()
        {
            if (this.undoStack.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the stack is empty or not.
        /// </summary>
        /// <returns>returns true if theres something in the stack otherwise false.</returns>
        public bool GetSizeOfRedo()
        {
            if (this.redoStack.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pushes a command to the Undo Stack.
        /// </summary>
        /// <param name="command">The command to push.</param>
        public void AddToRedo(ICommand command)
        {
            this.redoStack.Push(command);
        }

        /// <summary>
        /// Cell Property changed s off a signal to when a cell is changed.
        /// </summary>
        /// <param name="sender"> object sender for the certain that it is using.</param>
        /// <param name="e"> holds the data.</param>
        private void CellPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            bool checkException = false;
            SpreadSheetCell? cell = sender as SpreadSheetCell;
            if (e.PropertyName == "Color" && this.PropertyChanged != null)
            {
                this.PropertyChanged(cell, new PropertyChangedEventArgs(e.PropertyName));
            }

            if (cell != null)
            {
                checkException = this.EvaulateFormula(cell);
                if (!checkException)
                {
                    this.UpdateDependentCells(cell);
                }
            }
        }

        /// <summary>
        /// Updates the each cell that is dependent of the cell.
        /// </summary>
        /// <param name="cell">Takes in a cell which is used to to check if its in the dictionary.</param>
        private void UpdateDependentCells(Cell cell)
        {
            bool checkException = false;
            if (cell.DependentCells != null)
            {
                var dependentsCopy = new List<Cell>(cell.DependentCells);
                foreach (SpreadSheetCell neededCell in dependentsCopy)
                {
                    checkException = this.EvaulateFormula(neededCell);
                    if (!checkException)
                    {
                        this.UpdateDependentCells(neededCell);
                    }
                }
            }
        }

        /// <summary>
        /// Evaulates the formula that is in a cell.
        /// </summary>
        /// <param name="cell">Takes in a cell which is used to get the evaulated formula of it.</param>
        /// <returns>returns false if no exception was thrown otherwise.</returns>
        private bool EvaulateFormula(SpreadSheetCell cell)
        {
            char column;
            bool throwsException = false;
            string row = string.Empty;
            int columnIndex = 0;
            int rowIndex = 0;
            Cell? copyCell = null;
            ExpressionTree expressionTree;

            if (cell != null && cell.Text != null)
            {
                if (cell.Text.StartsWith("="))
                {
                    try
                    {
                        // Parse formula
                        string formula = cell.Text.Substring(1);
                        if (cell.Formula != formula && cell.Formula != null)
                        {
                            cell.DependentCells?.Clear();
                        }

                        cell.Formula = formula;
                        expressionTree = new ExpressionTree(formula);
                        foreach (string variable in expressionTree.Variables.Keys)
                        {
                            column = variable[0];
                            columnIndex = column - 'A';
                            row = variable.Substring(1);
                            if (!int.TryParse(row, out rowIndex))
                            {
                                throw new InvalidVariableException();
                            }

                            copyCell = this.GetCell(rowIndex - 1, columnIndex);

                            if (copyCell == cell)
                            {
                                throw new SameReferenceCellException();
                            }
                            else if (copyCell != null && double.TryParse(copyCell.Value, out double value))
                            {
                                expressionTree.SetVariable(variable, value);
                            }
                            else if (copyCell != null)
                            {
                                if (copyCell.Text == null)
                                {
                                    cell.SetValue("0");
                                }
                            }

                            if (cell.DependentCells != null && copyCell != null && !copyCell.DependentCells.Contains(cell))
                            {
                                copyCell.DependentCells.Add(cell);
                            }
                        }

                        // Evaluate formula and set cell value
                        double result = expressionTree.Evaluate();
                        if (result != 0)
                        {
                            cell.SetValue(result.ToString());
                        }
                        else
                        {
                            if (copyCell != null && copyCell.Value != null)
                            {
                                cell.SetValue(copyCell.Value);
                            }
                        }

                        cell.CircularReferenceChecker(cell);
                    }
                    catch (SameReferenceCellException)
                    {
                        cell.SetValue("Error: Referenced cell is the same as current cell.");
                        throwsException = true;
                    }
                    catch (NoVariableValueException)
                    {
                        cell.SetValue("Error: Referenced cell does not have a defined value.");
                    }
                    catch (InvalidExpressionFormula)
                    {
                        // Set error message in the cell’s text
                        cell.SetValue("Error: Invalid Formula.");
                    }
                    catch (InvalidVariableException)
                    {
                        cell.SetValue("Error: Invalid variable name.");
                    }
                    catch (InvalidCellException)
                    {
                        cell.SetValue("Error: Invalid cell, cell out of range.");
                    }
                    catch (CircularReferenceException)
                    {
                        cell.SetValue("Error: Circular Reference detected.");
                        throwsException = true;
                    }
                    catch
                    {
                        cell.SetValue("Error: Unknown.");
                    }
                }
                else if (cell.Text == string.Empty)
                {
                    cell.Text = null;
                    cell.SetValue(null);
                }
                else
                {
                    cell.SetValue(cell.Text);
                }

                this.PropertyChanged?.Invoke(cell, new PropertyChangedEventArgs(nameof(Cell.Value)));
            }

            return throwsException;
        }

        /// <summary>
        /// Private spreadsheetCell class that inherits abstract cell class.
        /// </summary>
        private class SpreadSheetCell : Cell
        {
            /// <summary>
            /// The formula for the spreadsheet cell.
            /// </summary>
            private string? formula;

            /// <summary>
            /// Initializes a new instance of the <see cref="SpreadSheetCell"/> class.
            /// Does the base classes constructor which is Cell in this case.
            /// </summary>
            /// <param name="rowIndex"> Takes in rowIndex. </param>
            /// <param name="columnIndex"> Takes in columnIndex. </param>
            /// <param name="expression">The expression of the cell.</param>
            public SpreadSheetCell(int rowIndex, int columnIndex, string? expression)
                : base(rowIndex, columnIndex)
            {
                this.formula = expression;
            }

            /// <summary>
            /// Gets or Sets the formula.
            /// </summary>
            public string? Formula { get; set; }

            /// <summary>
            /// Method to be able to set the value of the value of the field.
            /// </summary>
            /// <param name="value"> Takes in value so that i can change the class field value. </param>
            public void SetValue(string? value)
            {
                this.value = value;
            }
        }
    }
}
