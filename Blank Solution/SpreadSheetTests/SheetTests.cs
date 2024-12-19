// <copyright file="SheetTests.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using NUnit.Framework;
using SpreadsheetEngine;
using SpreadSheetEngine;

namespace SpreadSheetTests
{
    /// <summary>
    /// public class sheetTests that has test method for the project.
    /// </summary>
    public class SheetTests
    {
        /// <summary>
        /// a private field to use for the test methods.
        /// </summary>
        private SpreadSheet testSheet;

        /// <summary>
        /// Setup method just creates a spreadsheet that is 10x10.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.testSheet = new SpreadSheet(10, 10); // Creating a 10x10 spreadsheet
        }

        /// <summary>
        /// Tests if the GetCell method actually returns the correct column and row and it's not null.
        /// </summary>
        [Test]
        public void GetCellReturnsCorrectCell()
        {
            Cell? testCell = this.testSheet.GetCell(5, 2); // Get the cell at row 5, column 2
            Assert.That(testCell, Is.Not.Null);
            Assert.That(testCell.RowIndex, Is.EqualTo(5));
            Assert.That(testCell.ColumnIndex, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests if Setting the text of a cell works.
        /// </summary>
        [Test]
        public void SetTextUpdatesTextProperty()
        {
            Cell? testCell = this.testSheet.GetCell(3, 4);
            if (testCell != null)
            {
                testCell.Text = "Hello"; // set text to hello.
                Assert.That(testCell.Text, Is.EqualTo("Hello"));
            }
        }

        /// <summary>
        /// Tests has two cells with text and the second one should copy from the first cell.
        /// </summary>
        [Test]
        public void SetFormulaTest()
        {
            Cell? cellA1 = this.testSheet.GetCell(0, 0);
            Cell? cellB1 = this.testSheet.GetCell(0, 1);
            if (cellA1 != null && cellB1 != null)
            {
                cellB1.Text = "100";
                cellA1.Text = "=B1";
                Assert.That(cellA1.Value, Is.EqualTo("100"));
            }
        }

        /// <summary>
        /// Tests to see that you copy from an empty cell.
        /// </summary>
        [Test]
        public void EmptyCellTextTest()
        {
            Cell? testCell = this.testSheet.GetCell(2, 2);
            if (testCell != null)
            {
                testCell.Text = "=A1";
                Assert.That(testCell.Value, Is.EqualTo("0"));
            }
        }

        /// <summary>
        /// Test to see if two cells are able to have text set.
        /// </summary>
        [Test]
        public void TwoCellsHaveTextTest()
        {
            if (this.testSheet != null)
            {
                // Set some random text
                Cell? testCell = this.testSheet.GetCell(4, 4);
                if (testCell != null)
                {
                    testCell.Text = "Hello";
                }

                Cell? testCell2 = this.testSheet.GetCell(6, 7);
                if (testCell2 != null)
                {
                    testCell2.Text = "World";
                }

                // Check if the correct cells have been updated
                Assert.That(this.testSheet.GetCell(4, 4)?.Text, Is.EqualTo("Hello"));
                Assert.That(this.testSheet.GetCell(6, 7)?.Text, Is.EqualTo("World"));
            }
        }

        /// <summary>
        /// Edge case and putting text in the last cell.
        /// </summary>
        [Test]
        public void TextLastCellTest()
        {
            Cell? testCell = this.testSheet.GetCell(9, 9); // Last cell in a 10x10 grid
            if (testCell != null)
            {
                testCell.Text = "Last Cell";
                Assert.That(testCell.Text, Is.EqualTo("Last Cell"));
            }
        }

        /// <summary>
        /// tests copying a cell in the sheet.
        /// </summary>
        [Test]
        public void TextCopyCellTest()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);
            Cell? testCell2 = this.testSheet.GetCell(1, 2);

            if (testCell != null && testCell2 != null)
            {
                testCell.Text = "=77";
                testCell2.Text = "=A1";
                Assert.That(testCell2.Value, Is.EqualTo("77"));
            }
        }

        /// <summary>
        /// tests adding on cells.
        /// </summary>
        [Test]
        public void TextAddingCellTest()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);
            Cell? testCell2 = this.testSheet.GetCell(0, 1);
            Cell? testCell3 = this.testSheet.GetCell(1, 1);

            if (testCell != null && testCell2 != null && testCell3 != null)
            {
                testCell.Text = "77";
                testCell2.Text = "5";
                testCell3.Text = "=A1+B1";
                Assert.That(testCell3.Value, Is.EqualTo("82"));
            }
        }

        /// <summary>
        /// tests subtracting on cells.
        /// </summary>
        [Test]
        public void TextSubtractingCellTest()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);
            Cell? testCell2 = this.testSheet.GetCell(0, 1);
            Cell? testCell3 = this.testSheet.GetCell(1, 1);

            if (testCell != null && testCell2 != null && testCell3 != null)
            {
                testCell.Text = "77";
                testCell2.Text = "5";
                testCell3.Text = "=A1-B1";
                Assert.That(testCell3.Value, Is.EqualTo("72"));
            }
        }

        /// <summary>
        /// tests to see if dependencies work.
        /// </summary>
        [Test]
        public void TextUpdatingCellTest()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);
            Cell? testCell2 = this.testSheet.GetCell(0, 1);
            Cell? testCell3 = this.testSheet.GetCell(1, 0);

            if (testCell != null && testCell2 != null && testCell3 != null)
            {
                testCell.Text = "77";
                testCell2.Text = "5";
                testCell3.Text = "=A1-B1";
                testCell2.Text = "10";
                Assert.That(testCell3.Value, Is.EqualTo("67"));
            }
        }

        /// <summary>
        /// testing Edge case for only having one token.
        /// </summary>
        [Test]
        public void TestEdgeCaseSingleToken()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);

            if (testCell != null)
            {
                testCell.Text = "=77";
                Assert.That(testCell.Value, Is.EqualTo("77"));
            }
        }

        /// <summary>
        /// tests for an invalid formula.
        /// </summary>
        [Test]
        public void InvalidExpressionTest()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);

            if (testCell != null)
            {
                testCell.Text = "=[";
                Assert.That(testCell.Value, Is.EqualTo("Error: Invalid Formula."));
            }
        }

        /// <summary>
        /// tests a formula with an invalid variable name.
        /// </summary>
        [Test]
        public void InvalidVariableNameTest()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);
            Cell? testCell2 = this.testSheet.GetCell(0, 1);
            if (testCell != null && testCell2 != null)
            {
                testCell.Text = "7";
                testCell2.Text = "=A1+HI";
                Assert.That(testCell2.Value, Is.EqualTo("Error: Invalid variable name."));
            }
        }

        /// <summary>
        /// tests a formula with an invalid variable name.
        /// </summary>
        [Test]
        public void UndoTextTest()
        {
            Cell? cell = this.testSheet.GetCell(0, 0);
            if (cell != null)
            {
                cell.Text = "hello";

                cell.Text = "Test";
                this.testSheet.AddToUndo(new SetTextCommand(cell, "hello"));
                this.testSheet.Undo();
                Assert.That(cell.Text, Is.EqualTo("hello"));
            }
        }

        /// <summary>
        /// tests undo and redo noramlly.
        /// </summary>
        [Test]
        public void RedoTextTest()
        {
            Cell? cell = this.testSheet.GetCell(0, 0);
            if (cell != null)
            {
                cell.Text = "hello";

                cell.Text = "Test";
                this.testSheet.AddToUndo(new SetTextCommand(cell, "hello"));
                this.testSheet.Undo();
                this.testSheet.Redo();
                Assert.That(cell.Text, Is.EqualTo("Test"));
            }
        }

        /// <summary>
        /// Tests calling Redo with an empty redo stack.
        /// </summary>
        [Test]
        public void RedoWithEmptyStackTest()
        {
            Cell? cell = this.testSheet.GetCell(0, 0);
            if (cell != null)
            {
                cell.Text = "test";
                this.testSheet.Redo();
                Assert.That(cell.Text, Is.EqualTo("test"));
            }
        }

        /// <summary>
        /// Tests calling Undo with an empty undo stack.
        /// </summary>
        [Test]
        public void UndoWithEmptyStackTest()
        {
            Cell? cell = this.testSheet.GetCell(0, 0);
            if (cell != null)
            {
                cell.Text = "test";
                this.testSheet.Undo();
                Assert.That(cell.Text, Is.EqualTo("test"));
            }
        }

        /// <summary>
        /// Tests partial undo and redo sequence for multiple changes on the same cell.
        /// </summary>
        [Test]
        public void PartialUndoRedoSequenceTest()
        {
            Cell? cell = this.testSheet.GetCell(0, 0);
            if (cell != null)
            {
                cell.Text = "hello";
                cell.Text = "test";
                this.testSheet.AddToUndo(new SetTextCommand(cell, "hello"));

                cell.Text = "test2";
                this.testSheet.AddToUndo(new SetTextCommand(cell, "test"));

                cell.Text = "test3";
                this.testSheet.AddToUndo(new SetTextCommand(cell, "test2"));

                this.testSheet.Undo();
                this.testSheet.Undo();

                Assert.That(cell.Text, Is.EqualTo("test"));
                this.testSheet.Redo();
                Assert.That(cell.Text, Is.EqualTo("test2"));
                this.testSheet.Redo();
                Assert.That(cell.Text, Is.EqualTo("test3"));
            }
        }

        /// <summary>
        /// normal case of loading from a file.
        /// </summary>
        [Test]
        public void TestLoads()
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\epich\\Downloads\\testoutput.xml"))
            {
                this.testSheet.Load(reader);
            }

            Assert.That(this.testSheet.GetCell(0, 0)?.BGColor.ToString("X8"), Is.EqualTo("FFFF0000"));
            Assert.That(this.testSheet.GetCell(0, 0)?.Text, Is.EqualTo("10"));

            Assert.That(this.testSheet.GetCell(0, 1)?.BGColor.ToString("X8"), Is.EqualTo("FFFF0000"));
            Assert.That(this.testSheet.GetCell(0, 1)?.Text, Is.EqualTo("=A1"));

            Assert.That(this.testSheet.GetCell(0, 2)?.BGColor.ToString("X8"), Is.EqualTo("FFFF0000"));
            Assert.That(this.testSheet.GetCell(0, 2)?.Text, Is.EqualTo("=A1+B1"));
        }

        /// <summary>
        /// Exception case for loading from a file that doesnt exist.
        /// </summary>
        [Test]
        public void LoadWithInvalidFilePathTest()
        {
            Cell? cell = this.testSheet.GetCell(0, 0);
            if (cell != null)
            {
                try
                {
                    using StreamReader reader = new StreamReader("C:\\InvalidPath\\nonexistent.xml");
                    this.testSheet.Load(reader);
                }
                catch (DirectoryNotFoundException)
                {
                    Assert.That(cell.Text, Is.EqualTo(null));
                }
            }
        }

        /// <summary>
        /// tests the saving functionaility of saving a spreadsheet.
        /// </summary>
        [Test]
        public void SaveTest()
        {
            Cell? cell1 = this.testSheet.GetCell(0, 0);
            Cell? cell2 = this.testSheet.GetCell(0, 1);
            Cell? cell3 = this.testSheet.GetCell(0, 2);

            if (cell1 != null && cell2 != null && cell3 != null)
            {
                cell1.Text = "10";
                cell1.BGColor = 0xFFFF0000;
                cell2.Text = "=A1";
                cell2.BGColor = 0xFFFF0000;
                cell3.Text = "=A1+B1";
                cell3.BGColor = 0xFFFF0000;
            }

            string filePath = "C:\\Users\\epich\\Downloads\\testoutput1.xml";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                this.testSheet.Save(writer);
            }

            SpreadSheet? newTestSheet = new SpreadSheet(10, 10);
            using (StreamReader reader = new StreamReader(filePath))
            {
                newTestSheet.Load(reader);
            }

            Assert.That(newTestSheet.GetCell(0, 0)?.Text, Is.EqualTo("10"));
            Assert.That(newTestSheet.GetCell(0, 0).BGColor.ToString("X8"), Is.EqualTo("FFFF0000"));
            Assert.That(newTestSheet.GetCell(0, 1)?.Text, Is.EqualTo("=A1"));
            Assert.That(newTestSheet.GetCell(0, 1).BGColor.ToString("X8"), Is.EqualTo("FFFF0000"));
            Assert.That(newTestSheet.GetCell(0, 2)?.Text, Is.EqualTo("=A1+B1"));
            Assert.That(newTestSheet.GetCell(0, 2).BGColor.ToString("X8"), Is.EqualTo("FFFF0000"));
        }

        /// <summary>
        /// Tests to see if theres a self reference.
        /// </summary>
        [Test]
        public void TestSelfReference()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);

            if (testCell != null)
            {
                testCell.Text = "=A1";

                Assert.That(testCell.Value, Is.EqualTo("Error: Referenced cell is the same as current cell."));
            }
        }

        /// <summary>
        /// Tests to see if theres a circular reference.
        /// </summary>
        [Test]
        public void TestCircularReference()
        {
            Cell? testCell = this.testSheet.GetCell(0, 0);
            Cell? testCell2 = this.testSheet.GetCell(0, 1);
            Cell? testCell3 = this.testSheet.GetCell(1, 1);
            Cell? testCell4 = this.testSheet.GetCell(1, 0);
            if (testCell != null && testCell2 != null && testCell3 != null && testCell4 != null)
            {
                testCell.Text = "=B1";
                testCell2.Text = "=B2";
                testCell3.Text = "=A2";
                testCell4.Text = "=A1";

                Assert.That(testCell4.Value, Is.EqualTo("Error: Circular Reference detected."));
            }
        }
    }
}
