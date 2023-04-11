namespace Fluff
{
    public static class Log
    {
        /// <summary>
        /// The current instance of the Logger.
        /// </summary>
        public static Logger Logger { get; set; } = new Logger();

        /// <inheritdoc cref="Logger.Debug" />
        public static void Debug(string message, object? args = null) => Logger.Debug(message, args);

        /// <inheritdoc cref="Logger.Info" />
        public static void Info(string message, object? args = null) => Logger.Info(message, args);

        /// <inheritdoc cref="Logger.Warning" />
        public static void Warning(string message, object? args = null) => Logger.Warning(message, args);

        /// <inheritdoc cref="Logger.Error" />
        public static void Error(string message, object? args = null) => Logger.Error(message, args);

        /// <inheritdoc cref="Logger.Fatal" />
        public static void Fatal(string message, object? args = null) => Logger.Fatal(message, args);
    }
}