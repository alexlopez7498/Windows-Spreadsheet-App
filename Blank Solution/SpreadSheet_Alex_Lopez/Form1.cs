// <copyright file="Form1.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.IO;
using SpreadsheetEngine;
using SpreadSheetEngine;

namespace SpreadSheet_Alex_Lopez
{
    /// <summary>
    /// partial class form1 that inherits Form.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Sheet field to we can make a instance of it in the constructor.
        /// </summary>
        private SpreadSheet? sheet;

        /// <summary>
        /// saves the original text for cellbegin.
        /// </summary>
        private string ogText = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// that calls initializeComponent and InitializeDataGrid methods.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.InitializeDataGrid();
            this.sheet = new SpreadSheet(26, 50);
            this.sheet.PropertyChanged += this.SheetPropertyChanged;

            this.dataGridView1.CellBeginEdit += this.DataGridView1CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.DataGridView1CellEndEdit;
            this.sheet.PropertyChanged += this.SheetPropertyChangedCellColorChanged;
        }

        /// <summary>
        /// Property changed mehtod so that the spreadsheet can communicate with the UI.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">the events data.</param>
        private void SheetPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Cell? cell = (Cell?)sender;

            if (cell != null)
            {
                int row = cell.RowIndex;
                int col = cell.ColumnIndex;
                this.dataGridView1.Rows[row].Cells[col].Value = cell.Value;
            }
        }

        /// <summary>
        /// Property changed mehtod so that the spreadsheet can communicate with the UI.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">the events data.</param>
        private void SheetPropertyChangedCellColorChanged(object? sender, PropertyChangedEventArgs e)
        {
            Cell? cell = (Cell?)sender;
            if (cell != null && e.PropertyName != null)
            {
                this.dataGridView1[cell.ColumnIndex, cell.RowIndex].Style.BackColor = Color.FromArgb((int)cell.BGColor);
            }
        }

        /// <summary>
        /// Method that just adds columns A-Z and adds 50 rows.
        /// </summary>
        private void InitializeDataGrid()
        {
            // Clear any existing columns if added through the designer
            this.dataGridView1.Columns.Clear();

            // Add columns A to Z
            for (char column = 'A'; column <= 'Z'; column++)
            {
                this.dataGridView1.Columns.Add(column.ToString(), column.ToString());
            }

            // Add 50 rows
            this.dataGridView1.Rows.Add(50);

            // Label the row headers with numbers 1 to 50
            for (int i = 1; i <= 50; i++)
            {
                this.dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
        }

        /// <summary>
        /// method for the demo button when it is clicked, that put text of the cells, 50 random ones, all in B and A columns.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">the events data.</param>
        private void DemoButtonClick(object sender, EventArgs e)
        {
            if (this.sheet != null)
            {
                Random rand = new Random();

                // Update 50 random cells
                for (int i = 0; i < 50; i++)
                {
                    int row = rand.Next(0, 50);
                    int col = rand.Next(0, 26);
                    Cell? cell = this.sheet.GetCell(row, col);
                    if (cell != null)
                    {
                        cell.Text = "Hello World!";
                    }
                }

                // Set text in every cell in column B
                for (int i = 0; i < 50; i++)
                {
                    Cell? cell = this.sheet.GetCell(i, 1);
                    if (cell != null)
                    {
                        cell.Text = $"This is cell B{i + 1}";
                    }
                }

                // Set text in every cell in column A
                for (int i = 0; i < 50; i++)
                {
                    Cell? cell = this.sheet.GetCell(i, 0);
                    if (cell != null)
                    {
                        cell.Text = $"=B{i + 1}";
                    }
                }
            }
        }

        /// <summary>
        /// Triggered when a cell begins editing to store the original text and display it in the editor.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">the events data.</param>
        private void DataGridView1CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            Cell? cell = this.sheet?.GetCell(row, col);

            if (cell != null && cell.Text != null)
            {
                this.dataGridView1.Rows[row].Cells[col].Value = cell.Text;
            }
        }

        /// <summary>
        /// Triggered when a cell ends editing to update the cell's text in the spreadsheet model.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">the events data.</param>
        private void DataGridView1CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            Cell? cell = this.sheet?.GetCell(row, col);

            if (cell != null)
            {
                if (this.dataGridView1.Rows[row].Cells[col].Value == null)
                {
                    if (this.sheet != null)
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        this.sheet.AddToUndo(new SetTextCommand(
                        this.sheet.GetCell(e.RowIndex, e.ColumnIndex), this.sheet.GetCell(e.RowIndex, e.ColumnIndex).Text));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }

                    cell.Text = string.Empty;
                }
                else
                {
                    if (this.sheet != null)
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        this.sheet.AddToUndo(new SetTextCommand(
                        this.sheet.GetCell(e.RowIndex, e.ColumnIndex), this.sheet.GetCell(e.RowIndex, e.ColumnIndex).Text));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        cell.Text = this.dataGridView1.Rows[row].Cells[col].Value?.ToString();
                    }
                }

                // Update the Text property of the cell in the model with the new user input
                this.dataGridView1.Rows[row].Cells[col].Value = cell.Value;
                this.CheckButtons();
            }
        }

        /// <summary>
        /// Button to have the user choose a color of the selected cells.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event data.</param>
        private void ChangeBackgroundColorToolStripMenuItemClick(object sender, EventArgs e)
        {
            ColorDialog colorPicker = new ColorDialog();

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                uint newColor = (uint)colorPicker.Color.ToArgb();
                List<Cell> cells = new List<Cell>();
                List<uint> initialColors = new List<uint>();

                foreach (DataGridViewCell selectedCell in this.dataGridView1.SelectedCells)
                {
                    if (this.sheet != null)
                    {
                        Cell? cell = this.sheet.GetCell(selectedCell.RowIndex, selectedCell.ColumnIndex);
                        if (cell != null)
                        {
                            cells.Add(cell);
                            initialColors.Add(cell.BGColor);
                            cell.BGColor = newColor;
                        }
                    }
                }

                if (cells.Count > 0 && this.sheet != null)
                {
                    this.sheet.AddToUndo(new ChangeColorCommand(cells, initialColors));
                }
            }

            this.CheckButtons();
            this.dataGridView1.ClearSelection();
        }

        /// <summary>
        /// Checks the buttons if they need to be enabled or not.
        /// </summary>
        private void CheckButtons()
        {
            if (this.sheet != null)
            {
                if (this.sheet.GetSizeOfUndo())
                {
                    this.undoToolStripMenuItem.Enabled = true;
                    var topUndoCommand = this.sheet.PeekUndo();
                    if (topUndoCommand != null)
                    {
                        if (topUndoCommand.GetType() == typeof(SetTextCommand))
                        {
                            this.undoToolStripMenuItem.Text = "Undo Text Change";
                        }
                        else
                        {
                            this.undoToolStripMenuItem.Text = "Undo Color Change";
                        }
                    }
                }
                else
                {
                    this.undoToolStripMenuItem.Enabled = false;
                    this.undoToolStripMenuItem.Text = "Undo";
                }

                if (this.sheet.GetSizeOfRedo())
                {
                    this.redoToolStripMenuItem.Enabled = true;
                    var topRedoCommand = this.sheet.PeekRedo();
                    if (topRedoCommand != null)
                    {
                        if (topRedoCommand.GetType() == typeof(SetTextCommand))
                        {
                            this.redoToolStripMenuItem.Text = "Redo Text Change";
                        }
                        else
                        {
                            this.redoToolStripMenuItem.Text = "Redo Color Change";
                        }
                    }
                }
                else
                {
                    this.redoToolStripMenuItem.Enabled = false;
                    this.redoToolStripMenuItem.Text = "Redo";
                }
            }
        }

        /// <summary>
        /// calls the redo function to undo the last command.
        /// </summary>
        /// <param name="sender">source of event.</param>
        /// <param name="e">the event data.</param>
        private void UndoToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (this.sheet != null)
            {
                this.sheet.Undo();
            }

            this.CheckButtons();
        }

        /// <summary>
        /// calls the redo function to redo the last command.
        /// </summary>
        /// <param name="sender">source of event.</param>
        /// <param name="e">the event data.</param>
        private void RedoToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (this.sheet != null)
            {
                this.sheet.Redo();
            }

            this.CheckButtons();
        }

        /// <summary>
        /// saves every cell that has non-default data in it into a xml file.
        /// </summary>
        /// <param name="sender">source of event.</param>
        /// <param name="e">the event data.</param>
        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();

            file.Filter = "xml files (*.xml)|*.xml";
            file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter outStream = new StreamWriter(file.FileName))
                {
                    if (this.sheet != null)
                    {
                        this.sheet.Save(outStream);
                    }
                }
            }
        }

        /// <summary>
        /// Opens your files for you to select a xml file to load into the spreadsheet.
        /// </summary>
        /// <param name="sender">source of event.</param>
        /// <param name="e">the event data.</param>
        private void LoadToolStripMenuItemClick(object sender, EventArgs e)
        {
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.Filter = "xml files (*.xml)|*.xml";
                file.FilterIndex = 2;
                file.RestoreDirectory = true;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader inStream = new StreamReader(file.OpenFile()))
                    {
                        if (this.sheet != null)
                        {
                            this.sheet.Load(inStream);
                        }
                    }
                }
            }
        }
    }
}