using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvyStats.Commands
{
    internal class CommandStats : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "stats";

        public string Help => "show your stats";

        public string Syntax => "/stats <Name>";

        public List<string> Aliases => new List<string> { "estadisticas"};

        public List<string> Permissions => new List<string> { "envy.stats" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var instance = EnvyStatsPlugin.Instance;
            try
            {
                UnturnedPlayer target = UnturnedPlayer.FromName(command[0]);
                if (target == null) throw new Exception();

                TaskDispatcher.QueueOnMainThread(() =>
                {
                    var data = instance.DatabaseManager.GetStats(target.CSteamID);

                    TaskDispatcher.QueueOnMainThread(() =>
                    {

                        if(caller is ConsolePlayer)
                        {
                            Logger.Log(instance.Translate("stats_of", data.Name, data.KillS, data.Deaths, data.Headshots, instance.DatabaseManager.GetRankOnStats(target.CSteamID)));
                        }
                        else
                        {
                            UnturnedPlayer player = (UnturnedPlayer)caller;
                            instance.SendMessage(player, instance.Translate("stats_of", data.Name, data.KillS, data.Deaths, data.Headshots, instance.DatabaseManager.GetRankOnStats(target.CSteamID)));

                        }

                        
                    });
                });
            }
            catch
            {
                UnturnedPlayer player = (UnturnedPlayer)caller;
                var data = instance.DatabaseManager.GetStats(player.CSteamID);

                TaskDispatcher.QueueOnMainThread(() =>
                {
                    instance.SendMessage(player, instance.Translate("your_stats", data.KillS, data.Deaths, data.Headshots, instance.DatabaseManager.GetRankOnStats(player.CSteamID)));
                });
               
            }
        }
    }
}
