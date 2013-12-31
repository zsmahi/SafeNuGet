﻿using Owasp.SafeNuGet.NuGet;
using Owasp.SafeNuGet.Unsafe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owasp.SafeNuGet
{
    public class DecisionMaker
    {
        public List<KeyValuePair<NuGetPackage, UnsafePackage>> Evaluate(NuGetPackages packages, UnsafePackages unsafePackages) 
        {
            var result = new List<KeyValuePair<NuGetPackage, UnsafePackage>>();
            foreach(var package in packages) 
            {
                result.AddRange(unsafePackages.Where(u => u.Is(package)).Select(unsafePackage => new KeyValuePair<NuGetPackage, UnsafePackage>(package, unsafePackage)));
            }
            return result;
        }
    }
}
