using System;

namespace VoronoiDemo
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new VoronoiDemo())
                game.Run();
        }
    }
}
