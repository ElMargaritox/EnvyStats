using EnvyStats.Configuration;
using MercadoLibre.Database;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvyStats
{
    public class EnvyStatsPlugin : RocketPlugin<EnvyStatsConfiguration>
    {
        public static EnvyStatsPlugin Instance { get; set; }
        public DatabaseManager DatabaseManager { get; set; }
        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEvents_OnPlayerDeath;
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            DatabaseManager = new DatabaseManager(); DatabaseManager.Reload();
        }
        public override TranslationList DefaultTranslations
        {
            get
            {
                TranslationList list = new TranslationList();
                list.Add("your_stats", "(color=red)Kills: {0} Deaths: {1} Headshots: {2} Rank: {3}(/color)");
                list.Add("stats_of", "(color=red)Stat's {0} - Kills: {1} Deaths: {2} Headshots: {3} Rank: {4}(/color)");
                list.Add("ranking", "(color=red){0} - [{1}] [{2} Kills] [{3} Deaths](/color)");
                return list;
            }
        }

        public void SendMessage(UnturnedPlayer player, string message)
        {
            ChatManager.serverSendMessage(message.Replace('<', '(').Replace('>', ')'), UnityEngine.Color.white, null, player.SteamPlayer(), EChatMode.GLOBAL, Configuration.Instance.Image, true);
        }
        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            DatabaseManager.Create(player.CSteamID);
        }

        private void UnturnedPlayerEvents_OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, Steamworks.CSteamID murderer)
        {
            UnturnedPlayer killer = UnturnedPlayer.FromCSteamID(murderer);

            if(killer == null || killer.Equals(player.CSteamID))
            {
                DatabaseManager.SumarMuerte(player.CSteamID);
                return;
            }
            else
            {
                if(limb == ELimb.SKULL)
                {
                    DatabaseManager.SumarHeadshot(killer.CSteamID);
                }
                DatabaseManager.SumarKill(killer.CSteamID);
                DatabaseManager.SumarMuerte(player.CSteamID);
            }
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerDeath -= UnturnedPlayerEvents_OnPlayerDeath;
            U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
        }
    }
}
