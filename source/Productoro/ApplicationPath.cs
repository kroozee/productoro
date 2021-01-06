using System;
using System.IO;
using Productoro.Extensions;
using Optional.Unsafe;

namespace Productoro
{
    public static class ApplicationPath
    {
        private static Lazy<string> _applicationFile { get; } = new Lazy<string>(() =>
        {
            using var process = System.Diagnostics.Process.GetCurrentProcess();
            return process.MainModule.FileName;
        });

        private static Lazy<string> _applicationDirectory { get; } =
           _applicationFile.Select(f => Path.GetDirectoryName(f).AsOption().ValueOrFailure("Directory does not exist for file."));

        public static string File => _applicationFile.Value;

        public static string Directory => _applicationDirectory.Value;
    }
}
