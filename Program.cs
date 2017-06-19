using System;
using System.Collections.Generic;

namespace GitBranchCleaner
{
    class Program
    {
        /// <summary>
        /// Simple little utility to create script files for deleting git branches remote and local.
        /// </summary>
        static void Main(string[] args)
        {
            var logger = new LogService();

            if (args.Length != 1)
            {
                logger.Info("usage: gitbranchcleaner [<git folder path>]");
                logger.Info("");
                return;
            }

            var gitPath = args[0];

            var scriptCreator = new GitScriptCreator(logger, gitPath, new List<string>());

            try
            {
                logger.Info("Creating script files from local git folder: " + gitPath);
                scriptCreator.CreateScriptFiles(false);
                logger.Info("Script files has been created");
            }
            catch (ArgumentException e)
            {
                logger.Error("Failed to create git script files. " + e.Message);
            }
            catch (Exception e)
            {
                logger.Error("Failed to create git script file. Unexpected error occured: " + e.Message);
            }
        }
    }
}