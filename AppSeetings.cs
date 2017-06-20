using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace GitBranchCleaner
{
    public class AppSeetings
    {
        public List<string> ExcludedBranches => Properties.Settings.Default.ExcludedBranches.Split( new[]{';'}, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}