using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Logic;
using Minesweeper.Tests.Helpers;

namespace Minesweeper.Tests
{
    [TestClass]
    public class MarkerGridTest
    {
        [TestMethod]
        public void Empty()
        {
        }

        [TestMethod]
        public void ConstructSquare()
        {
            var size = 3;
            var grid = new MarkerGridInspectable(size,null,0);

            Assert.AreEqual(size, grid.MarkerStoreInspectable.GetLength(0));
            Assert.AreEqual(size, grid.MarkerStoreInspectable.GetLength(1));
        }

        [TestMethod]
        public void ConstructRectangular()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            Assert.AreEqual(size_rows, grid.MarkerStoreInspectable.GetLength(0));
            Assert.AreEqual(size_cols, grid.MarkerStoreInspectable.GetLength(1));
        }
        [TestMethod]
        public void RenderEmpty()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            var rendered = grid.Render();

            Assert.AreEqual(size_rows, rendered.Length);
            Assert.AreEqual(size_cols, rendered[0].Length);

            // Each line should be three occupied spaces
            foreach(var line in rendered)
            {
                Assert.AreEqual("###", line);
            }
        }
        [TestMethod]
        public void PlaySingle()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            grid.PlayAt(1, 1);

            var rendered = grid.Render();

            Assert.AreEqual("# #", rendered[1]);
        }

        [TestMethod]
        public void Die()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            // Plant a bomb!!
            grid.MarkerStoreInspectable[1, 1].isBomb = true;

            var result = grid.PlayAt(1, 1);

            var rendered = grid.Render();

            Assert.AreEqual("#@#", rendered[1]);
            Assert.AreEqual(MarkerGrid.PlayResult.GameOver, result);
        }

        [TestMethod]
        public void ShowABomb()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            // Plant a bomb!!
            grid.MarkerStoreInspectable[1, 1].isBomb = true;

            var result = grid.PlayAt(2, 1);

            var rendered = grid.Render();

            Assert.AreEqual("##1", rendered[1]);
            Assert.AreEqual(MarkerGrid.PlayResult.Continue, result);
        }

        [TestMethod]
        public void Show8Bombs()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            // Plant 8 bombs
            grid.MarkerStoreInspectable[0, 0].isBomb = true;
            grid.MarkerStoreInspectable[0, 1].isBomb = true;
            grid.MarkerStoreInspectable[0, 2].isBomb = true;
            grid.MarkerStoreInspectable[1, 0].isBomb = true;
            grid.MarkerStoreInspectable[1, 2].isBomb = true;
            grid.MarkerStoreInspectable[2, 0].isBomb = true;
            grid.MarkerStoreInspectable[2, 1].isBomb = true;
            grid.MarkerStoreInspectable[2, 2].isBomb = true;

            var result = grid.PlayAt(1, 1);

            var rendered = grid.Render();

            Assert.AreEqual("#8#", rendered[1]);
            Assert.AreEqual(MarkerGrid.PlayResult.Continue, result);
        }

        [TestMethod]
        public void CantPlayDouble()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            grid.PlayAt(1, 1);
            var result = grid.PlayAt(1, 1);

            Assert.AreEqual(MarkerGrid.PlayResult.Invalid, result);
        }
        [TestMethod]
        public void CantPlayOffGrid()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            var result = grid.PlayAt(20, 20);

            Assert.AreEqual(MarkerGrid.PlayResult.Invalid, result);
        }
        [TestMethod]
        public void Win()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows, 0);

            // Plant 8 bombs
            grid.MarkerStoreInspectable[0, 0].isBomb = true;
            grid.MarkerStoreInspectable[0, 1].isBomb = true;
            grid.MarkerStoreInspectable[0, 2].isBomb = true;
            grid.MarkerStoreInspectable[1, 0].isBomb = true;
            grid.MarkerStoreInspectable[1, 2].isBomb = true;
            grid.MarkerStoreInspectable[2, 0].isBomb = true;
            grid.MarkerStoreInspectable[2, 1].isBomb = true;
            grid.MarkerStoreInspectable[2, 2].isBomb = true;

            // Play all the other places EXCEPT 1,1

            for(int row = 3; row < size_rows; ++row)
            {
                for (int col = 0; col < size_cols; ++col)
                {
                    var playresult = grid.PlayAt(col, row);
                    Assert.AreEqual(MarkerGrid.PlayResult.Continue, playresult);
                }
            }

            // Now play the final space
            var result = grid.PlayAt(1, 1);

            Assert.AreEqual(MarkerGrid.PlayResult.Victory, result);
        }
        [TestMethod]
        public void ConstructRectangularWithBombs()
        {
            var size_cols = 3;
            var size_rows = 10;
            var num_bombs = size_cols * size_rows / 2;
            var grid = new MarkerGridInspectable(size_cols, size_rows, num_bombs);

            var actual_bombs = 0;
            for (int row = 0; row < size_rows; ++row)
            {
                for (int col = 0; col < size_cols; ++col)
                {
                    if (grid.MarkerStoreInspectable[row, col].isBomb)
                        ++actual_bombs;
                }
            }

            Assert.AreEqual(num_bombs, actual_bombs);
        }

    }
}
