using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace GitBranchCleaner
{
    public class GitScriptCreator
    {
        private readonly LogService _logService;
        private readonly string _gitFolder;
        private readonly List<string> _excludeBranches;

        public GitScriptCreator(LogService logService,  string gitFolder, List<string> excludeBranches)
        {
            _logService = logService;
            _gitFolder = gitFolder;
            _excludeBranches = excludeBranches;
        }

        /// <summary>
        /// Create script for deleting git branches. Will create for both local and remote branches.
        /// </summary>
        /// <param name="localOnly">if false will also create script file for remote branches.</param>
        public void CreateScriptFiles(bool localOnly)
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory;
            var gitLocalFilePath = Path.Combine(filePath, "localGitCleaning.bat");
            var gitRemoteFilePath = Path.Combine(filePath, "remoteGitCleaning.bat");

            if (File.Exists(gitLocalFilePath))
            {
                _logService.Info("Deleting old script file: " + gitLocalFilePath);
                File.Delete(gitLocalFilePath);
            }
            if (File.Exists(gitRemoteFilePath))
            {
                _logService.Info("Deleting old script file: " + gitRemoteFilePath);
                File.Delete(gitRemoteFilePath);
            }

            if (!Directory.Exists(_gitFolder))
                throw new ArgumentException($"The folder \"{_gitFolder}\" does not exist");

            try
            {
                using (var repo = new Repository(_gitFolder))
                {
                    var lines = repo.Branches.Where(p => !p.IsRemote && _excludeBranches.All(x => p.FriendlyName != x)).ToList().
                        Select(x => $"git branch -d { x.FriendlyName}").ToList();
                    CreateScriptFile(gitLocalFilePath, lines);

                    if (localOnly) return;

                    lines = repo.Branches.Where(p => p.IsRemote && _excludeBranches.All(x => p.FriendlyName.Replace(@"origin/", "") != x)).ToList().
                        Select(x => $"git push origin --delete {x.FriendlyName.Replace(@"origin/", "")}").ToList();
                    CreateScriptFile(gitRemoteFilePath, lines);
                }
            }
            catch (Exception e)
            {
                _logService.Error($"An error occured when fetching git branches from folder: \"{_gitFolder}\"");
                _logService.Error("Error message: " + e.Message);

                throw;
            }
        }

        private static void CreateScriptFile(string gitLocalFilePath, List<string> lines)
        {
            using (var writer = File.AppendText(gitLocalFilePath))
            {
                lines.ForEach(line => writer.WriteLine(line));
            }
        }
    }
}