namespace Chess.Logging.Levels {
    [Flags]
    public enum Level {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 4,
        Error = 8
    }
}
