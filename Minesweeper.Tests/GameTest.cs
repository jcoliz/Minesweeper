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
            TestGame.GameBoard.Markers[1][1].isBomb = true;
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
            TestGame.GameBoard.Markers[1][1].isBomb = true;

            var result = TestGame.PlayAt(new Point(1, 1));

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("[#] [@] [#] ", rendered[1]);
            Assert.AreEqual(Game.PlayResult.GameOver, result);
        }
        [TestMethod]
        public void ShowABomb()
        {
            TestGame.GameBoard.Markers[1][1].isBomb = true;

            var result = TestGame.PlayAt(new Point(2, 1));

            var rendered = TestGame.GameBoard.Render();

            Assert.AreEqual("[#] [#] [1] ", rendered[1]);
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
            TestGame.GameBoard.Markers[0][0].isBomb = true;
            TestGame.GameBoard.Markers[0][1].isBomb = true;
            TestGame.GameBoard.Markers[0][2].isBomb = true;
            TestGame.GameBoard.Markers[1][0].isBomb = true;
            TestGame.GameBoard.Markers[1][2].isBomb = true;
            TestGame.GameBoard.Markers[2][0].isBomb = true;
            TestGame.GameBoard.Markers[2][1].isBomb = true;
            TestGame.GameBoard.Markers[2][2].isBomb = true;

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
            var actual_bombs = grid.GameBoard.Markers.Aggregate(0,(total, row) => total + row.Where(item => item.isBomb).Count());

            Assert.AreEqual(num_bombs, actual_bombs);
        }

        [TestMethod]
        public void ClearMultipleAtOnce()
        {
            // Plant bombs around an empty space
            TestGame.GameBoard.Markers[0][0].isBomb = true;
            TestGame.GameBoard.Markers[0][1].isBomb = true;
            TestGame.GameBoard.Markers[0][2].isBomb = true;
            TestGame.GameBoard.Markers[1][0].isBomb = true;
            TestGame.GameBoard.Markers[2][0].isBomb = true;
            TestGame.GameBoard.Markers[3][0].isBomb = true;
            TestGame.GameBoard.Markers[4][0].isBomb = true;
            TestGame.GameBoard.Markers[4][1].isBomb = true;
            TestGame.GameBoard.Markers[4][2].isBomb = true;

            // Play in the empty space
            var result = TestGame.PlayAt(new Point(2, 2));

            // Check that a neighboring space is open
            var actual = TestGame.GameBoard.Markers[3][1].isShowing;
            Assert.AreEqual(true, actual);
        }
    }
}
