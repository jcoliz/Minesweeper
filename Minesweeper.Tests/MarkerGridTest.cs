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
            var grid = new MarkerGridInspectable(size);

            Assert.AreEqual(size, grid.MarkerStoreInspectable.GetLength(0));
            Assert.AreEqual(size, grid.MarkerStoreInspectable.GetLength(1));
        }

        [TestMethod]
        public void ConstructRectangular()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols,size_rows);

            Assert.AreEqual(size_rows, grid.MarkerStoreInspectable.GetLength(0));
            Assert.AreEqual(size_cols, grid.MarkerStoreInspectable.GetLength(1));
        }
        [TestMethod]
        public void RenderEmpty()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows);

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
            var grid = new MarkerGridInspectable(size_cols, size_rows);

            grid.PlayAt(1, 1);

            var rendered = grid.Render();

            Assert.AreEqual("# #", rendered[1]);
        }

        [TestMethod]
        public void Die()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new MarkerGridInspectable(size_cols, size_rows);

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
            var grid = new MarkerGridInspectable(size_cols, size_rows);

            // Plant a bomb!!
            grid.MarkerStoreInspectable[1, 1].isBomb = true;

            var result = grid.PlayAt(2, 1);

            var rendered = grid.Render();

            Assert.AreEqual("##1", rendered[1]);
            Assert.AreEqual(MarkerGrid.PlayResult.Continue, result);
        }

    }
}
