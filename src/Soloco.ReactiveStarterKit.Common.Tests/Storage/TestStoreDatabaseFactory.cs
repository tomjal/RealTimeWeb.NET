﻿using System;
using System.Diagnostics;
using System.IO;

namespace Soloco.ReactiveStarterKit.Common.Tests.Storage
{
    public static class TestStoreDatabaseFactory
    {
        //todo add alternative paths or move to config if necessary
        private static readonly string[] _versions = { "9.5", "9.4" };
        private const string _psqlPath = @"C:\Program Files\PostgreSQL\{version}\bin\psql.exe";
        private const string _psqlPathLinux = "psql";

        public static void CreateCleanStoreDatabase()
        {
            Debug.WriteLine("Test Store Database Creating");

            var tempScriptFileName = WriteTempSqlScript();
            var command = $"-f {tempScriptFileName} -U postgres -v ON_ERROR_STOP=1";

            var path = Environment.IsRunningOnMono && Environment.IsLinux ? _psqlPathLinux : GetWindowsPath();

            Debug.WriteLine($"Executing script {tempScriptFileName} with exe {path} and command {command}.");

            StartAndOutputProcess(path, command);

            Debug.WriteLine("Test Store Database Created");

            File.Delete(tempScriptFileName);
        }

        private static string GetWindowsPath()
        {
            foreach (var version in _versions)
            {
                var path = _psqlPath.Replace("{version}", version);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            var versionList = string.Join(", ", _versions);
            throw new InvalidOperationException($"Could not find psql.exe. Supported version: {versionList}");
        }

        private static void StartAndOutputProcess(string path, string command)
        {
            using (var process = new Process { StartInfo = ProcessInfo(path, command) })
            {
                process.Start();
                WriteOutputToDebug(process);
                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException(
                        $"{$"Could not initilaize database. exit code: {process.ExitCode}"}{System.Environment.NewLine}" +
                        "The database will be dropped. Are you connected you the database?");
                }
            }
        }

        private static void WriteOutputToDebug(Process process)
        {
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                if (line != null)
                {
                    Debug.WriteLine(line);
                }
            }
        }

        private static ProcessStartInfo ProcessInfo(string path, string command)
        {
            return new ProcessStartInfo
            {
                FileName = path,
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
        }

        private static string WriteTempSqlScript()
        {
            var script = typeof(TestStoreDatabaseFactory).ReadResourceString("CreateStore.sql");

            var tempScriptFileName = Path.GetTempFileName();
            File.WriteAllText(tempScriptFileName, script);
            return tempScriptFileName;
        }
    }
}