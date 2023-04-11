using System.Diagnostics;
using Pastel;

namespace Fluff
{
    public partial class Logger
    {
        /// <summary>
        /// Print formatted message with level Debug.
        /// </summary>
        /// <param name="message">The actual pre-formatted content of the message. Use <c>string.Format</c> for formatting.</param>
        /// <param name="args">Optional arguments to append to the end.</param>
        public void Debug(string message, object? args = null) => this.Print(Level.Debug, message, args);

        /// <summary>
        /// Print formatted message with level Info.
        /// </summary>
        /// <param name="message">The actual pre-formatted content of the message. Use <c>string.Format</c> for formatting.</param>
        /// <param name="args">Optional arguments to append to the end.</param>
        public void Info(string message, object? args = null) => this.Print(Level.Info, message, args);

        /// <summary>
        /// Print formatted message with level Warning.
        /// </summary>
        /// <param name="message">The actual pre-formatted content of the message. Use <c>string.Format</c> for formatting.</param>
        /// <param name="args">Optional arguments to append to the end.</param>
        public void Warning(string message, object? args = null) => this.Print(Level.Warning, message, args);

        /// <summary>
        /// Print formatted message with level Error.
        /// </summary>
        /// <param name="message">The actual pre-formatted content of the message. Use <c>string.Format</c> for formatting.</param>
        /// <param name="args">Optional arguments to append to the end.</param>
        public void Error(string message, object? args = null) => this.Print(Level.Error, message, args);

        /// <summary>
        /// Print formatted message with level Fatal.
        /// </summary>
        /// <param name="message">The actual pre-formatted content of the message. Use <c>string.Format</c> for formatting.</param>
        /// <param name="args">Optional arguments to append to the end.</param>
        public void Fatal(string message, object? args = null) => this.Print(Level.Fatal, message, args);

        /// <summary>
        /// Print formatted message.
        /// </summary>
        /// <param name="level">The debug level of the message.</param>
        /// <param name="message">The actual pre-formatted content of the message. Use <c>string.Format</c> for formatting.</param>
        /// <param name="args">Optional arguments to append to the end.</param>
        protected void Print(Level level, string message, object? args = null)
        {
            if(level < this.Options.Level)
            {
                return;
            }

            string content = string.Empty;

            /**
             *  Print date and/or time
             */

            if(Options.IncludeDate)
            {
                content = this.Options.DateFormat switch
                {
                    DateFormat.ISO      => DateTime.Now.ToString("yyyy-MM-dd"),
                    DateFormat.Unix     => new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(),
                    DateFormat.Short    => DateTime.Now.ToString("dd/MM/yyyy"),

                    _                   => throw new ArgumentException($"DateFormat not supported: {(int) this.Options.DateFormat}")
                };

                this.Write(content.Pastel(this.Colors.DateColor) + " ");
            }

            if(Options.IncludeTime)
            {
                content = this.Options.TimeFormat switch
                {
                    TimeFormat.Latin    => DateTime.Now.ToString("hh:mmtt"),

                    _                   => throw new ArgumentException($"DateFormat not supported: {(int) this.Options.DateFormat}")
                };

                this.Write(content.Pastel(this.Colors.TimeColor) + " ");
            }

            /**
             *  Print log level
             */

            content = level switch
            {
                Level.Debug   => "DEBU ",
                Level.Info    => "INFO ",
                Level.Warning => "WARN ",
                Level.Error   => "ERRO ",
                Level.Fatal   => "FATA ",

                _             => throw new ArgumentException($"Invalid Level: {level}")
            };

            this.Write(content.Pastel(this.Colors.LevelColors[level]));

            /**
             *  Print calling method
             */

            if(this.Options.IncludeCaller)
            {
                StackFrame frame = new StackFrame(1, true);

                content = string.Format(
                    "<{0}:{1}> ",
                    (frame.GetFileName() ?? "unknown").Split("/").Last(),
                    frame.GetFileLineNumber()
                );

                this.Write(content.Pastel(this.Colors.CallerColor));
            }

            /**
             *  Print prefix
             */

            if(!string.IsNullOrEmpty(this.Options.Prefix))
            {
                content = string.Format("{0}: ", this.Options.Prefix);

                this.Write(content.Pastel(this.Colors.PrefixColor));
            }

            /**
             *  Print message content
             */

            this.Write(message.Pastel(this.Colors.MessageColor));

            /**
             *  Print arguments
             */

            if(args != null)
            {
                this.EvaluateArguments(args);
            }

            this.WriteLine();

            if(level == Logger.Level.Fatal && this.Options.ThrowOnFatal)
            {
                throw new Exception("Fatal error occurred.");
            }
        }

        /// <summary>
        /// Evaluate arguments in a debug message and print them to the printer.
        /// </summary>
        /// <param name="args">The passed arguments.</param>
        protected void EvaluateArguments(object args)
        {
            foreach(var prop in args.GetType().GetProperties())
            {
                var value = prop.GetValue(args);

                if(prop.PropertyType.IsArray)
                {
                    this.EvaluateArrayArgument(prop.Name, value);
                    continue;
                }

                // Print name
                this.Write((" " + prop.Name + "=").Pastel(this.Colors.ArgumentNameColor));

                // Print value
                this.Write(value?.ToString().Pastel(this.Colors.ArgumentValueColor));
            }
        }

        /// <summary>
        /// Evaluate an array argument in a debug message and print them to the printer.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        protected void EvaluateArrayArgument(string name, object? value)
        {
            // Print name
            this.WriteLine(("\n  " + name + "=").Pastel(this.Colors.ArgumentNameColor));

            if(value == null)
            {
                this.Write("null".Pastel(this.Colors.ArgumentValueColor));
                return;
            }

            object[] values = (object[]) value;

            foreach(var elem in values)
            {
                this.Write("   │ ".Pastel(this.Colors.ArgumentNameColor));
                this.WriteLine(elem.ToString().Pastel(this.Colors.ArgumentValueColor));
            }
        }

        /// <summary>
        /// Print the specified message using the printer specified in the options.
        /// </summary>
        /// <param name="message">The message to print.</param>
        protected void Write(string? message)
            => this.Options.Printer(message);

        /// <summary>
        /// Print the specified message using the printer specified in the options, with a trailing newline.
        /// </summary>
        /// <param name="message">The message to print.</param>
        protected void WriteLine(string? message = "")
            => this.Write(message + "\n");
    }
}