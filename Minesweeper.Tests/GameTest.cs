using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Logic;

namespace Minesweeper.Tests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void Empty()
        {
        }

        [TestMethod]
        public void ConstructSquare()
        {
            var size = 3;
            var grid = new Game(size, null,0);

            Assert.AreEqual(size, grid.GameBoard.Markers.Count);
            Assert.AreEqual(size, grid.GameBoard.Markers[0].Count);
        }

        [TestMethod]
        public void ConstructRectangular()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new Game(size_cols, size_rows, 0);

            Assert.AreEqual(size_rows, grid.GameBoard.Markers.Count);
            Assert.AreEqual(size_cols, grid.GameBoard.Markers[0].Count);
        }
        [TestMethod]
        public void RenderEmpty()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new Game(size_cols, size_rows, 0);

            var rendered = grid.GameBoard.Render();

            Assert.AreEqual(size_rows, rendered.Count);
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
            var grid = new Game(size_cols, size_rows, 0);

            grid.PlayAt(1, 1);

            var rendered = grid.GameBoard.Render();

            Assert.AreEqual("# #", rendered[1]);
        }

        [TestMethod]
        public void Die()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new Game(size_cols, size_rows, 0);

            // Plant a bomb!!
            grid.GameBoard.Markers[1][1].isBomb = true;

            var result = grid.PlayAt(1, 1);

            var rendered = grid.GameBoard.Render();

            Assert.AreEqual("#@#", rendered[1]);
            Assert.AreEqual(Logic.Game.PlayResult.GameOver, (Logic.Game.PlayResult)result);
        }

        [TestMethod]
        public void ShowABomb()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new Game(size_cols, size_rows, 0);

            // Plant a bomb!!
            grid.GameBoard.Markers[1][1].isBomb = true;

            var result = grid.PlayAt(2, 1);

            var rendered = grid.GameBoard.Render();

            Assert.AreEqual("##1", rendered[1]);
            Assert.AreEqual(Logic.Game.PlayResult.Continue, (Logic.Game.PlayResult)result);
        }

        [TestMethod]
        public void Show8Bombs()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new Game(size_cols, size_rows, 0);

            // Plant 8 bombs
            grid.GameBoard.Markers[0][0].isBomb = true;
            grid.GameBoard.Markers[0][1].isBomb = true;
            grid.GameBoard.Markers[0][2].isBomb = true;
            grid.GameBoard.Markers[1][0].isBomb = true;
            grid.GameBoard.Markers[1][2].isBomb = true;
            grid.GameBoard.Markers[2][0].isBomb = true;
            grid.GameBoard.Markers[2][1].isBomb = true;
            grid.GameBoard.Markers[2][2].isBomb = true;

            var result = grid.PlayAt(1, 1);

            var rendered = grid.GameBoard.Render();

            Assert.AreEqual("#8#", rendered[1]);
            Assert.AreEqual(Logic.Game.PlayResult.Continue, (Logic.Game.PlayResult)result);
        }

        [TestMethod]
        public void CantPlayDouble()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new Game(size_cols, size_rows, 0);

            grid.PlayAt(1, 1);
            var result = grid.PlayAt(1, 1);

            Assert.AreEqual(Logic.Game.PlayResult.Invalid, (Logic.Game.PlayResult)result);
        }
        [TestMethod]
        public void CantPlayOffGrid()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new Game(size_cols, size_rows, 0);

            var result = grid.PlayAt(20, 20);

            Assert.AreEqual(Logic.Game.PlayResult.Invalid, (Logic.Game.PlayResult)result);
        }
        [TestMethod]
        public void Win()
        {
            var size_cols = 3;
            var size_rows = 10;
            var grid = new Game(size_cols, size_rows, 0);

            // Plant 8 bombs
            grid.GameBoard.Markers[0][0].isBomb = true;
            grid.GameBoard.Markers[0][1].isBomb = true;
            grid.GameBoard.Markers[0][2].isBomb = true;
            grid.GameBoard.Markers[1][0].isBomb = true;
            grid.GameBoard.Markers[1][2].isBomb = true;
            grid.GameBoard.Markers[2][0].isBomb = true;
            grid.GameBoard.Markers[2][1].isBomb = true;
            grid.GameBoard.Markers[2][2].isBomb = true;

            // Play all the other places EXCEPT 1,1

            for(int row = 3; row < size_rows; ++row)
            {
                for (int col = 0; col < size_cols; ++col)
                {
                    var playresult = grid.PlayAt(col, row);
                    Assert.AreEqual(Logic.Game.PlayResult.Continue, (Logic.Game.PlayResult)playresult);
                }
            }

            // Now play the final space
            var result = grid.PlayAt(1, 1);

            Assert.AreEqual(Logic.Game.PlayResult.Victory, (Logic.Game.PlayResult)result);
        }
        [TestMethod]
        public void ConstructRectangularWithBombs()
        {
            var size_cols = 3;
            var size_rows = 10;
            var num_bombs = size_cols * size_rows / 2;
            var grid = new Game(size_cols, size_rows, num_bombs);

            var actual_bombs = 0;
            for (int row = 0; row < size_rows; ++row)
            {
                for (int col = 0; col < size_cols; ++col)
                {
                    if (grid.GameBoard.Markers[row][col].isBomb)
                        ++actual_bombs;
                }
            }

            Assert.AreEqual(num_bombs, actual_bombs);
        }

    }
}
