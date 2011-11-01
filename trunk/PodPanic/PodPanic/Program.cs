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
            ////Authorization code for Intel AppUp(TM) software
            //com.intel.adp.AdpApplication app;
            //try
            //{
            //    //TODO: Right click and select Get Application GUID to replace the debug GUID before submit it
            //    //app = new com.intel.adp.AdpApplication(new com.intel.adp.AdpApplicationId(0x11111111, 0x11111111, 0x11111111, 0x11111111)); //debug GUID
            //    app = new com.intel.adp.AdpApplication(new com.intel.adp.AdpApplicationId(0x07998DB6, 0x1FEA41B5, 0x9D6A8AE7, 0x56949529)); //Pod Panic GUID
            //}
            //catch (com.intel.adp.AdpException e)
            //{
            //    if (e is com.intel.adp.AdpErrorException)
            //    {
            //        // TO DO: add your logic to handle the errors during initialization
            //        System.Environment.Exit(1);
            //    }
            //    else if (e is com.intel.adp.AdpWarningException)
            //    {
            //        // TO DO: add your logic to handle the warnings
            //    }
            //}
            using (PodPanic game = new PodPanic())
            {
                game.Run();
            }
        }
    }
}

