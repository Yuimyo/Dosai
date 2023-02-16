namespace Dosai.Common.Utils
{
    public static class UriHelper
    {
        public static Dictionary<string, string> ParseUrlQueries(Uri u)
        {
            var res = new Dictionary<string, string>();
            var q = u.Query.Substring(1, u.Query.Length - 1).Split('&');
            for (int i = 0; i < q.Length; i++)
            {
                var kv = q[i].Split('=');
                var key = kv[0];
                var value = kv[1];
                res[key] = value;
            }
            return res; 
        }
    }
}
