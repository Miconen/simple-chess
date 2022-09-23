using System;

namespace Chess.Error
{
    public enum Level {
        Normal,
        Warning,
        Error
    }

    public class ErrorHandler 
    {
        public List<string> Buffer;
        public Level Level;

        public ErrorHandler()
        {
            this.Buffer = new List<string>();
            // By default don't show any errors or warnings
            this.Level = (Level)0;
        }

        public void ShowNormal() { _setLevel(0); }
        public void ShowWarnings() { _setLevel(1); }
        public void ShowErrors() { _setLevel(2); }

        public void New(string message, int severity = 1)
        {
            if (this.Level >= (Level)severity) this.Buffer.Add(message);
        }

        public void Flush()
        {
            this.Buffer.Clear(); 
        }

        public bool IsEmpty()
        {
            return (this.Buffer.Count == 0) ? true : false;
        }

        public void Write()
        {
            if (this.IsEmpty()) return;

            foreach (string error in this.Buffer)
            {
                Console.WriteLine(error);
            }
        }

        private void _setLevel(int severity) { this.Level = (Level)severity; }
    }
}

