using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    class Log
    {
        private readonly string path = "logs";
        public string Text { get; set; }

        public Log(string text, string name = "latest.log")
        {
            Text = text;
            path = Path.Combine(path, name);
        }

        public void WriteLog(string path = null, bool isAppend = false)
        {
            if (path is null)
                path = this.path;
            Text = "[LP][" + DateTime.Now.ToString() + "]" + Text;
            var sw = new StreamWriter(path, isAppend, Encoding.UTF8);
            sw.WriteLine(Text);
            sw.Close();
        }
    }
}
