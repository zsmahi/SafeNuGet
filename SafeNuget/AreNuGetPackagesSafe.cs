﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using Owasp.SafeNuGet.NuGet;
using Owasp.SafeNuGet.Unsafe;


namespace Owasp.SafeNuGet
{
    public class AreNuGetPackagesSafe : Task
    {
        public string ProjectPath { get; set; }
        public string CacheTimeInMinutes { get; set; }
        private const String _id = "OWASP SafeNuGet";

        public override bool Execute()
        {
            
            var nugetFile = Path.Combine(ProjectPath, "packages.config");
            int cacheTime = 0;
            if (!String.IsNullOrEmpty(CacheTimeInMinutes) && !int.TryParse(CacheTimeInMinutes, out cacheTime))
            {
                BuildEngine.LogErrorEvent(new BuildErrorEventArgs("Configuration error", "CacheTimeInMinutes", BuildEngine.ProjectFileOfTaskNode, 0, 0, 0, 0, "Invalid value for CacheTimeInMinutes: " + CacheTimeInMinutes, "", "SafeNuGet"));
                return false;
            }

            BuildEngine.LogMessageEvent(new BuildMessageEventArgs("Checking " + nugetFile + " ...", "", _id, MessageImportance.High));
            if (File.Exists(nugetFile))
            {
                var packages = new NuGetPackageLoader().LoadPackages(nugetFile);
                UnsafePackages unsafePackages;
                if (cacheTime > 0)
                {
                    bool cacheHit = false;
                    var cacheFolder = Path.Combine(new FileInfo(BuildEngine.ProjectFileOfTaskNode).Directory.FullName, "cache");
                    unsafePackages = new PackageListLoader().GetCachedUnsafePackages(cacheFolder, cacheTime, out cacheHit);
                    if (cacheHit)
                    {
                        BuildEngine.LogMessageEvent(new BuildMessageEventArgs("Using cached list of unsafe packages", "", _id, MessageImportance.High));
                    }
                }
                else
                {
                    unsafePackages = new PackageListLoader().GetUnsafePackages();
                }
                var failures = new DecisionMaker().Evaluate(packages, unsafePackages);
                if (!failures.Any()) {
                    BuildEngine.LogMessageEvent(new BuildMessageEventArgs("No vulnerable packages found", "", _id, MessageImportance.High));
                } else {
                    foreach(var k in failures) {
                        var s = k.Key.Id + " " + k.Key.Version;
                        BuildEngine.LogWarningEvent(new BuildWarningEventArgs("SECURITY WARNING", s, nugetFile, 0, 0, 0, 0, "Library is vulnerable: " + s + " " + k.Value.InfoUri, "", _id));
                    }
                    return false;
                }

            } else {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("No packages.config found", "", "SafeNuGet", MessageImportance.High));
            }
            return true;
        }

    }
}
