namespace ProductManagementSystem.DisplayServices
{
    /// <summary>
    /// Shared console helpers used by all MenuHandler classes.
    /// </summary>
    public static class MenuHelpers
    {
        public static string Prompt(string label)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"\n  {label}: ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n  Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey(intercept: true);
        }
    }
}
