using Newtonsoft.Json.Linq;
using System;

namespace Linker
{
    internal class LinkInfo
    {
        internal readonly string From;
        internal readonly string To;
        internal readonly DateTime Date;

        /// <summary>
        /// An class that holds information about an link but does not contain the link itself.
        /// </summary>
        /// <param name="jobject">An json object with the fields string from, string to, datetime date</param>
        internal LinkInfo(JObject jobject)
        {
            From = jobject.Value<string>("from");
            To = jobject.Value<string>("to");
            
            try
            {
                Date = jobject.Value<DateTime>("date");
            }
            catch
            {
                Date = DateTime.MinValue;
            }
        }

        /// <summary>
        /// An class that holds information about an link but does not contain the link itself.
        /// </summary>
        /// <param name="from">Where the link is placed</param>
        /// <param name="to">The destination of the link</param>
        internal LinkInfo(string from, string to)
        {
            From = from;
            To = to;
            Date = DateTime.Now;
        }

        internal string[] ToArray()
        {
            return new string[] { From, To, Date.ToString() };
        }
    }
}