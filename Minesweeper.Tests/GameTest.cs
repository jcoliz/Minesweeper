using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Logic;

namespace Minesweeper.Tests
{
    [TestClass]
    public class GameTest
    {
        const int SizeCols = 3;
        const int SizeRows = 10;

        Game TestGame;

        [TestInitialize]
        public void SetUp()
        {
            TestGame = new Game(SizeCols, SizeRows, 0);
        }

        [TestMethod]
        public void Empty()
        {
            Assert.IsNotNull(TestGame);
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
            Assert.AreEqual(SizeRows, TestGame.GameBoard.Markers.Count);
            Assert.AreEqual(SizeCols, TestGame.GameBoard.Markers[0].Count);
        }
        [TestMethod]
        public void RenderEmpty()
        {
            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual(SizeRows, rendered.Count);
            Assert.AreEqual(SizeCols, rendered[0].Length);

            // Each line should be three occupied spaces
            foreach(var line in rendered)
            {
                Assert.AreEqual("###", line);
            }
        }
        [TestMethod]
        public void PlaySingle()
        {
            TestGame.PlayAt(1, 1);

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("# #", rendered[1]);
        }

        [TestMethod]
        public void Die()
        {
            TestGame.GameBoard.Markers[1][1].isBomb = true;

            var result = TestGame.PlayAt(1, 1);

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("#@#", rendered[1]);
            Assert.AreEqual(Game.PlayResult.GameOver, result);
        }

        [TestMethod]
        public void ShowABomb()
        {
            TestGame.GameBoard.Markers[1][1].isBomb = true;

            var result = TestGame.PlayAt(2, 1);

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("##1", rendered[1]);
            Assert.AreEqual(Game.PlayResult.Continue, result);
        }

        [TestMethod]
        public void Show8Bombs()
        {
            TestGame.GameBoard.Markers[0][0].isBomb = true;
            TestGame.GameBoard.Markers[0][1].isBomb = true;
            TestGame.GameBoard.Markers[0][2].isBomb = true;
            TestGame.GameBoard.Markers[1][0].isBomb = true;
            TestGame.GameBoard.Markers[1][2].isBomb = true;
            TestGame.GameBoard.Markers[2][0].isBomb = true;
            TestGame.GameBoard.Markers[2][1].isBomb = true;
            TestGame.GameBoard.Markers[2][2].isBomb = true;

            var result = TestGame.PlayAt(1, 1);

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("#8#", rendered[1]);
            Assert.AreEqual(Game.PlayResult.Continue, result);
        }

        [TestMethod]
        public void CantPlayDouble()
        {
            TestGame.PlayAt(1, 1);
            var result = TestGame.PlayAt(1, 1);

            Assert.AreEqual(Game.PlayResult.Invalid, result);
        }
        [TestMethod]
        public void CantPlayOffBoard()
        {
            var result = TestGame.PlayAt(SizeCols * 2, SizeRows * 2);

            Assert.AreEqual(Game.PlayResult.Invalid, result);
        }
        [TestMethod]
        public void Win()
        {
            // Plant 8 bombs
            TestGame.GameBoard.Markers[0][0].isBomb = true;
            TestGame.GameBoard.Markers[0][1].isBomb = true;
            TestGame.GameBoard.Markers[0][2].isBomb = true;
            TestGame.GameBoard.Markers[1][0].isBomb = true;
            TestGame.GameBoard.Markers[1][2].isBomb = true;
            TestGame.GameBoard.Markers[2][0].isBomb = true;
            TestGame.GameBoard.Markers[2][1].isBomb = true;
            TestGame.GameBoard.Markers[2][2].isBomb = true;

            // Play all the other places EXCEPT 1,1

            for(int row = 3; row < SizeRows; ++row)
            {
                for (int col = 0; col < SizeCols; ++col)
                {
                    var playresult = TestGame.PlayAt(col, row);
                    Assert.AreEqual(Game.PlayResult.Continue,playresult);
                }
            }

            // Now play the final space
            var result = TestGame.PlayAt(1, 1);

            Assert.AreEqual(Game.PlayResult.Victory, result);
        }
        [TestMethod]
        public void ConstructRectangularWithBombs()
        {
            var num_bombs = SizeCols * SizeRows / 2;
            var grid = new Game(SizeCols, SizeRows, num_bombs);

            var actual_bombs = 0;
            for (int row = 0; row < SizeRows; ++row)
            {
                for (int col = 0; col < SizeCols; ++col)
                {
                    if (grid.GameBoard.Markers[row][col].isBomb)
                        ++actual_bombs;
                }
            }

            Assert.AreEqual(num_bombs, actual_bombs);
        }

    }
}
