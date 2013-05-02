using System;

namespace GameEngine
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GunbondGame game = new GunbondGame())
            {
                game.Run();
            }
        }
    }
#endif
}

