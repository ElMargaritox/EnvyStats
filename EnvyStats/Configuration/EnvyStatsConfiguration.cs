using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvyStats.Configuration
{
    public class EnvyStatsConfiguration : IRocketPluginConfiguration
    {
        public string Image { get; set; }
        public void LoadDefaults()
        {
            Image = "url here";
        }
    }
}
