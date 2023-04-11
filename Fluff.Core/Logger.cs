using System.Drawing;

namespace Fluff
{
    public partial class Logger
    {
        /// <summary>
        /// Enumeration of all debug levels of the logger.
        /// </summary>
        public enum Level
        {
            /// <summary>
            /// Should be used for debugging messages/actions in the system.
            /// </summary>
            Debug,

            /// <summary>
            /// Should be used for events that may be worth noting.
            /// </summary>
            Info,

            /// <summary>
            /// Should be used for errors, which the system can handle itself, but are still noticable.
            /// </summary>
            Warning,

            /// <summary>
            /// Should be used for critical, non-crashing errors that may distrupt flow, but not severe.
            /// </summary>
            Error,

            /// <summary>
            /// Should be used when the system is unstable or should exit imminently.
            /// </summary>
            Fatal,
        }

        /// <summary>
        /// Enumeration of date-formats to use with the logger.
        /// </summary>
        public enum DateFormat
        {
            /// <summary>
            /// Print the date in ISO-format. Example: 2017-04-20
            /// </summary>
            ISO,

            /// <summary>
            /// Print the date in Unix seconds. Example: 1492702320
            /// </summary>
            Unix,

            /// <summary>
            /// Print the date in short format. Example: 10/14/2012
            /// </summary>
            Short,
        }

        /// <summary>
        /// Enumeration of time-formats to use with the logger.
        /// </summary>
        public enum TimeFormat
        {
            /// <summary>
            /// Print the time in Latin AM/PM format.
            /// </summary>
            Latin,
        }

        /// <summary>
        /// Options for defining a new Log-instance.
        /// </summary>
        public class LogOptions
        {
            /// <summary>
            /// Whether to include the calling function in the output.
            /// </summary>
            public bool IncludeCaller { get; set; } = false;

            /// <summary>
            /// Whether to include the date of the message in the output.
            /// </summary>
            public bool IncludeDate { get; set; } = false;

            /// <summary>
            /// The format of the date, if included.
            /// </summary>
            public DateFormat DateFormat { get; set; } = DateFormat.Short;

            /// <summary>
            /// Whether to include the time of the message in the output.
            /// </summary>
            public bool IncludeTime { get; set; } = true;

            /// <summary>
            /// The format of the time, if included.
            /// </summary>
            public TimeFormat TimeFormat { get; set; } = TimeFormat.Latin;

            /// <summary>
            /// Whether to throw an exception, when a fatal message is thrown.
            /// </summary>
            public bool ThrowOnFatal { get; set; } = true;

            /// <summary>
            /// The intial debug-level of the logger.
            /// </summary>
            public Level Level { get; set; } = Level.Warning;

            /// <summary>
            /// Prefixes to the any logged messages from this logger.
            /// </summary>
            public string Prefix { get; set; } = string.Empty;

            /// <summary>
            /// The function used to print messages. This can be the console output or a file output.
            /// </summary>
            public Action<string?> Printer { get; set; } = Console.Write;
        }

        /// <summary>
        /// Options for specifying colors for different elements in the messsage.
        /// </summary>
        public class LogColors
        {
            /// <summary>
            /// The color of the date-segment, if included.
            /// </summary>
            public Color DateColor { get; set; } = Color.FromArgb(236, 236, 236);

            /// <summary>
            /// The color of the time-segment, if included.
            /// </summary>
            public Color TimeColor { get; set; } = Color.FromArgb(236, 236, 236);

            /// <summary>
            /// The color of debug level segment.
            /// </summary>
            public Dictionary<Level, Color> LevelColors { get; set; } = new Dictionary<Level, Color>
            {
                { Level.Debug,      Color.FromArgb( 94,  95, 254) },
                { Level.Info,       Color.FromArgb( 98, 254, 218) },
                { Level.Warning,    Color.FromArgb(199, 234, 121) },
                { Level.Error,      Color.FromArgb(254,  95, 136) },
                { Level.Fatal,      Color.FromArgb(226,  42,  68) },
            };

            /// <summary>
            /// The color of the caller-segment, if included.
            /// </summary>
            public Color CallerColor { get; set; } = Color.FromArgb(140, 140, 140);

            /// <summary>
            /// The color of the prefix-segment, if included.
            /// </summary>
            public Color PrefixColor { get; set; } = Color.FromArgb(140, 140, 140);

            /// <summary>
            /// The color of the message.
            /// </summary>
            public Color MessageColor { get; set; } = Color.FromArgb(236, 236, 236);

            /// <summary>
            /// The color of the name of arguments.
            /// </summary>
            public Color ArgumentNameColor { get; set; } = Color.FromArgb(140, 140, 140);

            /// <summary>
            /// The color of the value of arguments.
            /// </summary>
            public Color ArgumentValueColor { get; set; } = Color.FromArgb(236, 236, 236);
        }

        /// <summary>
        /// Initial options for the logger.
        /// </summary>
        public LogOptions Options { get; set; } = new LogOptions();

        /// <summary>
        /// Initial colors for the logger.
        /// </summary>
        public LogColors Colors { get; set; } = new LogColors();

        /// <summary>
        /// Set whether to include the calling method in the output.
        /// </summary>
        /// <param name="include">Whether to include the calling method.</param>
        public void SetIncludeCaller(bool include) => this.Options.IncludeCaller = include;

        /// <summary>
        /// Set whether to include the date in the output.
        /// </summary>
        /// <param name="include">Whether to include the date.</param>
        public void SetIncludeDate(bool include) => this.Options.IncludeDate = include;

        /// <summary>
        /// Set the format of the date, if included.
        /// </summary>
        /// <param name="format">The new date format to use in the output.</param>
        public void SetDateFormat(DateFormat format) => this.Options.DateFormat = format;

        /// <summary>
        /// Set whether to include the time in the output.
        /// </summary>
        /// <param name="include">Whether to include the time.</param>
        public void SetIncludeTime(bool include) => this.Options.IncludeTime = include;

        /// <summary>
        /// Set the format of the time, if included.
        /// </summary>
        /// <param name="format">The new time format to use in the output.</param>
        public void SetTimeFormat(TimeFormat format) => this.Options.TimeFormat = format;

        /// <summary>
        /// Set whether to throw when a fatal message is received.
        /// </summary>
        /// <param name="throwOnFatal">Whether to throw when a fatal message is received.</param>
        public void SetThrowOnFatal(bool throwOnFatal) => this.Options.ThrowOnFatal = throwOnFatal;

        /// <summary>
        /// Set the minimum debug level for the logger.
        /// </summary>
        /// <param name="level">The minimum debug level for the logger.</param>
        public void SetMinimumLevel(Level level) => this.Options.Level = level;

        /// <summary>
        /// Set the prefix for the logger.
        /// </summary>
        /// <param name="prefix">The prefix for the logger.</param>
        public void SetPrefix(string prefix) => this.Options.Prefix = prefix;
    }
}