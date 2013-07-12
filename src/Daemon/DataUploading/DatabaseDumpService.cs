﻿using Common.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Daemon.DataUploading {
    class DatabaseDumpService {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void DumpDatabase(string database, string outputFolder, TimeSpan timeAgo) {
            var filename = Path.Combine(Environment.CurrentDirectory, "Lib", "mongodump.exe");

            foreach (var arg in BuildMongodumpArguments(timeAgo)) {
                var command = string.Format("-db {0} -o {1} {2}", database, outputFolder, arg);
                var info = new ProcessStartInfo(filename, command);
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;

                var process = Process.Start(info);

                var reader = process.StandardOutput;
                while (!reader.EndOfStream) {
                    Logger.Debug(reader.ReadLine());
                }
            }
        }

        private IEnumerable<string> BuildMongodumpArguments(TimeSpan timeAgo) {
            var epoch = new DateTime(1970, 1, 1).ToLocalTime();
            var startTime = (long)DateTime.Now.Add(timeAgo.Negate()).Subtract(epoch).TotalMilliseconds;
            yield return string.Format("-c products -q \"{{UpdateTime: {{$gte: new Date({0})}}}}\"", startTime);

            yield return "-c categories";

            yield return "-c ratio_rankings";
        }
    }
}
