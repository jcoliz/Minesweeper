using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Logic;
using System.Drawing;
using System.Linq;

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
            TestGame = new Game(new Size(SizeCols, SizeRows), 0);
        }
        [TestMethod]
        public void Empty()
        {
            Assert.IsNotNull(TestGame);
        }
        [TestMethod]
        public void ConstructSquare()
        {
            const int size = 3;
            var grid = new Game(new Size(size,size),0);

            Assert.AreEqual(size, grid.GameBoard.Dimensions.Width);
            Assert.AreEqual(size, grid.GameBoard.Dimensions.Height);
            Assert.AreEqual("#",grid.GameBoard[size - 1, size - 1].ToString());
        }
        [TestMethod]
        public void ConstructRectangular()
        {
            Assert.AreEqual(SizeCols, TestGame.GameBoard.Dimensions.Width);
            Assert.AreEqual(SizeRows, TestGame.GameBoard.Dimensions.Height);
            Assert.AreEqual("#", TestGame.GameBoard[SizeCols - 1, SizeRows - 1].ToString());
        }
        [TestMethod]
        public void RenderEmpty()
        {
            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual(SizeRows, rendered.Count);
            Assert.AreEqual(SizeCols * 4, rendered[0].Length);

            // Each line should be three occupied spaces
            foreach(var line in rendered)
            {
                Assert.AreEqual("[#] [#] [#] ", line);
            }
        }
        [TestMethod]
        public void PlayOrigin()
        {
            TestGame.GameBoard[1,1].isBomb = true;
            TestGame.PlayAt(new Point(0, 0));

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("[1] [#] [#] ", rendered[0]);
        }
        [TestMethod]
        public void PlaySingle()
        {
            TestGame.PlayAt(new Point(1, 1));

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("[ ] [ ] [ ] ", rendered[1]);
        }
        [TestMethod]
        public void Die()
        {
            TestGame.GameBoard[1,1].isBomb = true;

            var result = TestGame.PlayAt(new Point(1, 1));

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("[#] [@] [#] ", rendered[1]);
            Assert.AreEqual(Game.PlayResult.GameOver, result);
        }
        [TestMethod]
        public void ShowABomb()
        {
            TestGame.GameBoard[1,1].isBomb = true;

            var result = TestGame.PlayAt(new Point(2, 1));

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("[#] [#] [1] ", rendered[1]);
            Assert.AreEqual(Game.PlayResult.Continue, result);
        }
        [TestMethod]
        public void Show8Bombs()
        {
            TestGame.GameBoard[0, 0].isBomb = true;
            TestGame.GameBoard[1, 0].isBomb = true;
            TestGame.GameBoard[2, 0].isBomb = true;
            TestGame.GameBoard[0, 1].isBomb = true;
            TestGame.GameBoard[2, 1].isBomb = true;
            TestGame.GameBoard[0, 2].isBomb = true;
            TestGame.GameBoard[1, 2].isBomb = true;
            TestGame.GameBoard[2, 2].isBomb = true;

            var result = TestGame.PlayAt(new Point(1, 1));

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("[#] [8] [#] ", rendered[1]);
            Assert.AreEqual(Game.PlayResult.Continue, result);
        }

        [TestMethod]
        public void CantPlayDouble()
        {
            TestGame.PlayAt(new Point(1, 1));
            var result = TestGame.PlayAt(new Point(1, 1));

            Assert.AreEqual(Game.PlayResult.Invalid, result);
        }
        [TestMethod]
        public void CantPlayOffBoard()
        {
            var result = TestGame.PlayAt(new Point(SizeCols * 2, SizeRows * 2));

            Assert.AreEqual(Game.PlayResult.Invalid, result);
        }
        [TestMethod]
        public void Win()
        {
            // Plant 8 bombs
            TestGame.GameBoard[0, 0].isBomb = true;
            TestGame.GameBoard[1, 0].isBomb = true;
            TestGame.GameBoard[2, 0].isBomb = true;
            TestGame.GameBoard[0, 1].isBomb = true;
            TestGame.GameBoard[2, 1].isBomb = true;
            TestGame.GameBoard[0, 2].isBomb = true;
            TestGame.GameBoard[1, 2].isBomb = true;
            TestGame.GameBoard[2, 2].isBomb = true;

            // Empty the field below the 8 bombs
            var result = TestGame.PlayAt(new Point(1, 4));

            Assert.AreEqual(Game.PlayResult.Continue, result);

            // Now play the final space
            result = TestGame.PlayAt(new Point(1, 1));

            Assert.AreEqual(Game.PlayResult.Victory, result);
        }
        [TestMethod]
        public void ConstructRectangularWithBombs()
        {
            var num_bombs = SizeCols * SizeRows / 2;
            var grid = new Game(new Size(SizeCols, SizeRows), num_bombs);
            var actual_bombs = grid.GameBoard.Dimensions.GetEnumerator().Where(p => grid.GameBoard[p].isBomb).Count();

            Assert.AreEqual(num_bombs, actual_bombs);
        }

        [TestMethod]
        public void ClearMultipleAtOnce()
        {
            // Plant bombs around an empty space
            TestGame.GameBoard[0,0].isBomb = true;
            TestGame.GameBoard[1,0].isBomb = true;
            TestGame.GameBoard[2,0].isBomb = true;
            TestGame.GameBoard[0,1].isBomb = true;
            TestGame.GameBoard[0,2].isBomb = true;
            TestGame.GameBoard[0,3].isBomb = true;
            TestGame.GameBoard[0,4].isBomb = true;
            TestGame.GameBoard[1,4].isBomb = true;
            TestGame.GameBoard[2,4].isBomb = true;

            // Play in the empty space
            var result = TestGame.PlayAt(new Point(2, 2));

            // Check that a neighboring space is open
            var actual = TestGame.GameBoard[1,3].isShowing;
            Assert.AreEqual(true, actual);
        }
    }
}
