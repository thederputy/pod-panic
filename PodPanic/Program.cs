using System;

namespace PodPanic
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PodPanic game = new PodPanic())
            {
                game.Run();
            }
        }
    }
}

