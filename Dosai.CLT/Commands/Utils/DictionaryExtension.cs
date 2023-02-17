using Dosai.CLT.Exceptions;
using Dosai.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosai.CLT.Commands.Utils
{
    public static class DictionaryExtension
    {
        public static void Must(this Dictionary<string, string> kvs, out int res, params string[] keyOptions)
            => kvs.must(out res, keyOptions, StringParser.ToInt);
        public static void Must(this Dictionary<string, string> kvs, out uint res, params string[] keyOptions)
            => kvs.must(out res, keyOptions, StringParser.ToUInt);
        public static void Must(this Dictionary<string, string> kvs, out string res, params string[] keyOptions)
            => kvs.must(out res, keyOptions, StringParser.ToString);
        public static void Must(this Dictionary<string, string> kvs, out TimeSpan res, params string[] keyOptions)
            => kvs.must(out res, keyOptions, StringParser.ToTimeSpan);
        private static void must<T>(this Dictionary<string, string> kvs, out T res, string[] keyOptions, Func<string, T> parser)
        {
            foreach (var key in keyOptions)
            {
                if (kvs.ContainsKey(key))
                {
                    try
                    {
                        res = parser(kvs[key]);
                    }
                    catch (FormatException ex)
                    {
                        throw new InvalidCommandException();
                    }
                    return;
                }
            }
            throw new InvalidCommandException();
        }

        public static void Any(this Dictionary<string, string> kvs, out int? res, params string[] keyOptions)
            => kvs.any(out res, keyOptions, (s) => StringParser.ToInt(s));
        public static void Any(this Dictionary<string, string> kvs, out uint? res, params string[] keyOptions)
            => kvs.any(out res, keyOptions, (s) => StringParser.ToUInt(s));
        public static void Any(this Dictionary<string, string> kvs, out string? res, params string[] keyOptions)
            => kvs.any(out res, keyOptions, (s) => StringParser.ToString(s));
        public static void Any(this Dictionary<string, string> kvs, out TimeSpan? res, params string[] keyOptions)
            => kvs.any(out res, keyOptions, (s) => StringParser.ToTimeSpan(s));
        private static void any<T>(this Dictionary<string, string> kvs, out T? res, string[] keyOptions, Func<string, T> parser)
        {
            foreach (var key in keyOptions)
            {
                if (kvs.ContainsKey(key))
                {
                    res = parser(kvs[key]);
                    return;
                }
            }
            res = default;
        }
    }
}
