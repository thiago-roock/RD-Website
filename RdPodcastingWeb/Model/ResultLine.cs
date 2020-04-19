using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RdPodcastingWeb.Model
{
    public class ResultLine
    {
        public ResultLine() { }

        public string Estado { get; set; }
        public int Casos { get; set; }
    }

    public class ResultLine2
    {
        public ResultLine2() { }

        public string Estado { get; set; }
        public string Mortos { get; set; }
    }
}