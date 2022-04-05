using Rocket.API;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvyStats.Commands
{
    internal class CommandRanking : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "ranking";

        public string Help => "show top 3";

        public string Syntax => String.Empty;

        public List<string> Aliases => new List<string> { "top" };

        public List<string> Permissions => new List<string> { "envy.ranking" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var instance = EnvyStatsPlugin.Instance;
            UnturnedPlayer player = (UnturnedPlayer)caller;
            TaskDispatcher.QueueOnMainThread(() =>
            {


                var data = instance.DatabaseManager.GetTop();
                
               for (int i = 0; i < data.Count; i++)
                {
                    int j = i;
                    instance.SendMessage(player, instance.Translate("ranking", j++, data[i].Name, data[i].KillS, data[i].Deaths));
                }
            });
        }
    }
}
