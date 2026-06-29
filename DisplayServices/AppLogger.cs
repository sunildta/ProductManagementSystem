using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.IOptions
{
   // Demonstrates SINGLETON lifetime — one instance for the entire app lifetime.
    public class AppLogger
    {
        private readonly List<string> _log = new();
        public IReadOnlyList<string> Entries => _log.AsReadOnly();

        public void Log(string message)
        {
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            _log.Add(entry);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"LOG -> {entry}");
            Console.ResetColor();
        }
    }
}
