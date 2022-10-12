using System;
using System.Collections.Generic;
using Chess.Logging.Levels;

namespace Chess.Logging;

public class Logger
{
    public Level Level;
    private List<string> _buffer;
    private List<string> _printBuffer;

    public Logger()
    {
        // List of all errors
        this._buffer = new List<string>();
        // List of only the errors we are going to print
        this._printBuffer = new List<string>();
        // By default don't show any errors or warnings
        this.Level = Level.None;
    }

    public void ToggleDebug() { _toggleLevel(Level.Debug); }
    public void ToggleInformation() { _toggleLevel(Level.Info); }
    public void ToggleWarnings() { _toggleLevel(Level.Warning); }
    public void ToggleErrors() { _toggleLevel(Level.Error); }

    public void New(string message)
    {
        this._buffer.Add(message);
        this._printBuffer.Add(message);
    }

    public void Info(string message)
    {
        // Return out early if info message logging is not toggled on
        if (!_checkLevel(Level.Info)) return;
        _printInfoMessage(Level.Info, message);
    }

    public void Debug(string message)
    {
        // Return out early if debug message logging is not toggled on
        if (!_checkLevel(Level.Debug)) return;
        _printDebugMessage(Level.Debug, message);
    }

    public void Warning(string message)
    {
        // Add warning to non visible buffer
        this._buffer.Add(message);
        // Return out early if VISIBLE warning message logging is not toggled on
        if (!_checkLevel(Level.Warning)) return;
        _printWarningMessage(Level.Warning, message);
    }

    public void Error(string message)
    {
        // Add error to non visible buffer
        this._buffer.Add(message);
        // Return out early if VISIBLE error message logging is not toggled on
        if (!_checkLevel(Level.Error)) return;
        _printErrorMessage(Level.Error, message);
    }

    public void Flush()
    {
        this._buffer.Clear(); 
        this._printBuffer.Clear(); 
    }

    public bool IsEmpty()
    {
        return (this._buffer.Count == 0) ? true : false;
    }

    public void Write()
    {
        if (this.IsEmpty()) return;

        foreach (string error in this._printBuffer)
        {
            Console.WriteLine(error);
        }
    }

    private void _toggleLevel(Level severity)
    {
        if (_checkLevel(severity)) _removeLevel(severity);
        else _addLevel(severity);
    }

    private void _addLevel(Level severity)
    {
        this.Level = (this.Level | severity);
    }

    private void _removeLevel(Level severity)
    {
        this.Level = (this.Level & (~severity));
    }

    private bool _checkLevel(Level severity)
    {
        return (this.Level & severity) != 0;
    }

    private void _printDebugMessage(Level severity, string message)
    {
        string formatStart = "\u001b[37m";
        string formatEnd = "\u001b[0m";

        Console.WriteLine($"[{DateTime.Now.ToString("HH':'mm':'ss")}] ({formatStart + severity + formatEnd}) : {formatStart + message + formatEnd}");
    }

    private void _printInfoMessage(Level severity, string message)
    {
        string formatStart = "\u001b[34m";
        string formatEnd = "\u001b[0m";

        Console.WriteLine($"[{DateTime.Now.ToString("HH':'mm':'ss")}] ({formatStart + severity + formatEnd}) : {formatStart + message + formatEnd}");
    }

    private void _printWarningMessage(Level severity, string message)
    {
        string formatStart = "\u001b[33m";
        _printMessage(severity, message, formatStart);
    }

    private void _printErrorMessage(Level severity, string message)
    {
        string formatStart = "\u001b[31m";
        _printMessage(severity, message, formatStart);
    }

    private void _printMessage(Level severity, string message, string formatStart)
    {
        string formatEnd = "\u001b[0m";
        this._printBuffer.Add(
            $"[{DateTime.Now.ToString("HH':'mm':'ss")}] ({formatStart + severity + formatEnd}) : {formatStart + message + formatEnd}"
        );
    }
}

