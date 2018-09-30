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
    }
}