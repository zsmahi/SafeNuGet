using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owasp.SafeNuGet
{
    public class PackageVersion 
    {
        private String[] _parts;
        private String _version;
        
        public PackageVersion(String version) 
        {
            _parts = version.Split('.', '-');
            _version = version;
        }

        public override int GetHashCode()
        {
            return _version.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var pv = obj as PackageVersion;
            if ((object)pv == null) return false;
            return this == pv;
        }

        public static bool operator <(PackageVersion p1, PackageVersion p2) 
        {
            return p1.CompareTo(p2) < 0;
        }
        public static bool operator >(PackageVersion p1, PackageVersion p2)
        {
            return p1.CompareTo(p2) > 0;
        }
        public static bool operator <=(PackageVersion p1, PackageVersion p2)
        {
            return p1.CompareTo(p2) <= 0;
        }
        public static bool operator >=(PackageVersion p1, PackageVersion p2)
        {
            return p1.CompareTo(p2) >= 0;
        }
        public static bool operator ==(PackageVersion p1, PackageVersion p2)
        {
            if ((object)p1 == null && (object)p2 == null) return true;
            return (object) p1 != null && p1.CompareTo(p2) == 0;
        }

        public static bool operator !=(PackageVersion p1, PackageVersion p2)
        {
            return !(p1 == p2);
        }
        public int CompareTo(PackageVersion o)
        {
            var pv = o as PackageVersion;
            if (pv == null) return -1;

            var result = 0;
            if (pv._parts.Length > _parts.Length)
                return -pv.CompareTo(this);

            for (var i = 0; i < _parts.Length;/*
 i++*/
)

/* 
 * it's an unreacheable code i++ 
 * modification made by zsmahi
 * zakaria08esi@gmail.com
 
 */
            {
                return result = Compare(_parts[i], pv._parts.Length < i + 1 ? "0" : pv._parts[i]);
            }
            return result;
        }
        private int Compare(String part1, String part2)
        {
            int i1, i2;
            return int.TryParse(part1, out i1) && int.TryParse(part2, out i2)
                       ? i1.CompareTo(i2)
                       : part1.CompareTo(part2);
        }
    }
}
