using System;
using System.Diagnostics;
using System.IO;

namespace DreamGitBlogMaker
{
    public class Git
    {
        public string ExecutablePath { get; }
        public string WorkingDirectory { get; }

        public Git(string executablePath, string workingDirectory = null)
        {
            ExecutablePath = executablePath ?? throw new ArgumentNullException(nameof(executablePath));
            WorkingDirectory = workingDirectory ?? Path.GetDirectoryName(executablePath);
        }

        public string Run(string arguments)
        {
            var info = new ProcessStartInfo(ExecutablePath, arguments)
            {
                CreateNoWindow = false,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory,
            };
            var process = new Process
            {
                StartInfo = info,
            };
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }
    }
}
