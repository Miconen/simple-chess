using System;
using Chess.Error.Levels;

namespace Chess.Error
{
    public class ErrorHandler 
    {
        public Level Level;
        private List<string> _buffer;
        private List<string> _printBuffer;

        public ErrorHandler()
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

        public void New(string message, Level severity = Level.Warning)
        {
            // Don't add debug or info messages to error buffer, instead we use a special method later on to print them instantly
            if (severity != Level.Debug && severity != Level.Info) this._buffer.Add(message);

            if (!_checkLevel(severity)) return;

            switch (severity)
            {
                case Level.Debug:
                    _printDebugMessage(severity, message);
                    break;
                case Level.Info:
                    _printInfoMessage(severity, message);
                    break;
                case Level.Warning:
                    _printWarningMessage(severity, message);
                    break;
                case Level.Error:
                    _printErrorMessage(severity, message);
                    break;
                default:
                    this._printBuffer.Add(
                        $"[{DateTime.Now}] {message}"
                    );
                    break;
            }

        }

        public void Flush()
        {
            this._buffer.Clear(); 
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
            this._buffer.Add(
                $"[{DateTime.Now.ToString("HH':'mm':'ss")}] ({formatStart + severity + formatEnd}) : {formatStart + message + formatEnd}"
            );
        }
    }
}

