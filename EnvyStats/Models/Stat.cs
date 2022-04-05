using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvyStats.Models
{
    public class Stat
    {
        public CSteamID Id { get; set; }
        public string Name { get; set; }
        public int KillS;
        public int Deaths;
        public decimal Headshots;
    }
}
