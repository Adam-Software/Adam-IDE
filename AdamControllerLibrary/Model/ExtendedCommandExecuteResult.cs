using System;

namespace AdamControllerLibrary.Model
{
    public class ExtendedCommandExecuteResult
    {
        /// <summary>
        /// The succeeded status of an executable.
        /// </summary>
        public bool Succesed { get;  set; }

        /// <summary>
        /// The exit code of an executable.
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// The standard output of an executable.
        /// </summary>
        public string StandardOutput { get; set; } = string.Empty;

        /// <summary>
        /// The standard error of an executable.
        /// </summary>
        public string StandardError { get; set; } = string.Empty;

        /// <summary>
        /// The start time of an executable.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The end time of an executable.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// The run time of an executable.
        /// </summary>
        public TimeSpan RunTime { get; set; }
    }

    public static class Extension
    {
        public static CommandExecuteResult ToCommandExecuteResult(this ExtendedCommandExecuteResult extendedResult)
        {
            var result = new CommandExecuteResult()
            {
                Succeeded = extendedResult.Succesed,
                ExitCode = extendedResult.ExitCode,

                StandardError = extendedResult.StandardError,
                StartTime = extendedResult.StartTime,
                EndTime = extendedResult.EndTime,
                RunTime = extendedResult.RunTime
            };

            return result;
        }
    }
}
