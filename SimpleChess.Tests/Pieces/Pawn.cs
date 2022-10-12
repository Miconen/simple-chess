using NUnit.Framework;

using SimpleChess.Pieces;
using SimpleChess.Logging;
using SimpleChess.Chessboard;
using SimpleChess.Rules;

namespace SimpleChess.Tests.Pieces;

public class PawnTests
{
    public Pawn white;
    public Pawn black;
    public Logger logger;

    [SetUp]
    public void Setup()
    {
        this.logger = new Logger();
        this.white = new Pawn(true);
        this.black = new Pawn(false);
    }

    [Test]
    public void move1FowardWhite()
    {
        var (from, to) = (new Tile(1, 1), new Tile(2, 1));
        var move = new Move(this.logger, from, to);

        Assert.IsTrue(white.IsValidMove(move));
    }

    [Test]
    public void secondMove2ForwardWhite()
    {
        var (from, to) = (new Tile(2, 1), new Tile(4, 1));
        var move = new Move(this.logger, from, to);

        Assert.IsTrue(white.IsValidMove(move));
        Assert.IsFalse(white.IsValidMove(move));
    }

    [Test]
    public void secondMove2ForwardBlack()
    {
        var (from, to) = (new Tile(2, 1), new Tile(0, 1));
        var move = new Move(this.logger, from, to);

        Assert.IsTrue(black.IsValidMove(move));
        Assert.IsFalse(black.IsValidMove(move));
    }

    [Test]
    public void firstMove2FowardWhite()
    {
        var (from, to) = (new Tile(1, 1), new Tile(3, 1));
        var move = new Move(this.logger, from, to);

        Assert.IsTrue(white.IsValidMove(move));
    }

    [Test]
    public void moveBackwardsWhite()
    {
        var (from, to) = (new Tile(3, 1), new Tile(2, 1));
        var move = new Move(this.logger, from, to);

        Assert.IsFalse(white.IsValidMove(move));
    }

    [Test]
    public void moveFowardBlack()
    {
        var (from, to) = (new Tile(3, 1), new Tile(2, 1));
        var move = new Move(this.logger, from, to);

        Assert.IsTrue(black.IsValidMove(move));
    }

    [Test]
    public void move2FowardBlack()
    {
        var (from, to) = (new Tile(3, 1), new Tile(1, 1));
        var move = new Move(this.logger, from, to);

        Assert.IsTrue(black.IsValidMove(move));
    }

    [Test]
    public void moveBackwardsBlack()
    {
        var (from, to) = (new Tile(1, 1), new Tile(2, 1));
        var move = new Move(this.logger, from, to);

        Assert.IsFalse(black.IsValidMove(move));
    }

    [Test]
    public void moveTopLeft()
    {
        var (from, to) = (new Tile(1, 1), new Tile(2, 0));
        var move = new Move(this.logger, from, to);

        Assert.IsFalse(white.IsValidMove(move));
    }

    [Test]
    public void moveTopRight()
    {
        var (from, to) = (new Tile(1, 1), new Tile(2, 2));
        var move = new Move(this.logger, from, to);

        Assert.IsFalse(white.IsValidMove(move));
    }

    [Test]
    public void moveBottomLeft()
    {
        var (from, to) = (new Tile(1, 1), new Tile(0, 0));
        var move = new Move(this.logger, from, to);

        Assert.IsFalse(white.IsValidMove(move));
    }

    [Test]
    public void moveBottomRight()
    {
        var (from, to) = (new Tile(1, 1), new Tile(0, 2));
        var move = new Move(this.logger, from, to);

        Assert.IsFalse(white.IsValidMove(move));
    }
}