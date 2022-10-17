using System;
using NUnit.Framework;

using SimpleChess.Pieces;
using SimpleChess.Logging;
using SimpleChess.Chessboard;
using SimpleChess.Rules;

namespace SimpleChess.Tests.Pieces;

public class PawnTests
{
    private readonly Pawn _white = new Pawn(true);
    private readonly Pawn _black = new Pawn(false);
    private readonly Logger _logger = new Logger();

    [SetUp]
    public void Setup()
    {
        Console.WriteLine("Running pawn tests");
    }

    [Test]
    public void Move1ForwardWhite()
    {
        var (from, to) = (new Tile(1, 1), new Tile(2, 1));
        var move = new Move(_logger, from, to);

        Assert.IsTrue(_white.IsValidMove(move));
    }

    [Test]
    public void SecondMove2ForwardWhite()
    {
        var (from, to) = (new Tile(2, 1), new Tile(4, 1));
        var move = new Move(_logger, from, to);

        Assert.IsTrue(_white.IsValidMove(move));
        Assert.IsFalse(_white.IsValidMove(move));
    }

    [Test]
    public void SecondMove2ForwardBlack()
    {
        var (from, to) = (new Tile(2, 1), new Tile(0, 1));
        var move = new Move(_logger, from, to);

        Assert.IsTrue(_black.IsValidMove(move));
        Assert.IsFalse(_black.IsValidMove(move));
    }

    [Test]
    public void FirstMove2ForwardWhite()
    {
        var (from, to) = (new Tile(1, 1), new Tile(3, 1));
        var move = new Move(_logger, from, to);

        Assert.IsTrue(_white.IsValidMove(move));
    }

    [Test]
    public void MoveBackwardsWhite()
    {
        var (from, to) = (new Tile(3, 1), new Tile(2, 1));
        var move = new Move(_logger, from, to);

        Assert.IsFalse(_white.IsValidMove(move));
    }

    [Test]
    public void MoveForwardBlack()
    {
        var (from, to) = (new Tile(3, 1), new Tile(2, 1));
        var move = new Move(_logger, from, to);

        Assert.IsTrue(_black.IsValidMove(move));
    }

    [Test]
    public void Move2ForwardBlack()
    {
        var (from, to) = (new Tile(3, 1), new Tile(1, 1));
        var move = new Move(_logger, from, to);

        Assert.IsTrue(_black.IsValidMove(move));
    }

    [Test]
    public void MoveBackwardsBlack()
    {
        var (from, to) = (new Tile(1, 1), new Tile(2, 1));
        var move = new Move(_logger, from, to);

        Assert.IsFalse(_black.IsValidMove(move));
    }

    [Test]
    public void MoveTopLeft()
    {
        var (from, to) = (new Tile(1, 1), new Tile(2, 0));
        var move = new Move(_logger, from, to);

        Assert.IsFalse(_white.IsValidMove(move));
    }

    [Test]
    public void MoveTopRight()
    {
        var (from, to) = (new Tile(1, 1), new Tile(2, 2));
        var move = new Move(_logger, from, to);

        Assert.IsFalse(_white.IsValidMove(move));
    }

    [Test]
    public void MoveBottomLeft()
    {
        var (from, to) = (new Tile(1, 1), new Tile(0, 0));
        var move = new Move(_logger, from, to);

        Assert.IsFalse(_white.IsValidMove(move));
    }

    [Test]
    public void MoveBottomRight()
    {
        var (from, to) = (new Tile(1, 1), new Tile(0, 2));
        var move = new Move(_logger, from, to);

        Assert.IsFalse(_white.IsValidMove(move));
    }
}
