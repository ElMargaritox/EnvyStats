using DataStorage;
using EnvyStats;
using EnvyStats.Models;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoLibre.Database
{
    public class DatabaseManager
    {
        private List<Stat> Data;



        private DataStorage<List<Stat>> DataStorage { get; set; }
        public DatabaseManager()
        {

            this.DataStorage = new DataStorage<List<Stat>>(EnvyStatsPlugin.Instance.Directory, "Database.json");
            Logger.Log("Conexion a la base de datos... OK");
        }

        public void Reload()
        {
            Data = DataStorage.Read();
            if (Data == null)
            {
                Data = new List<Stat>();
                DataStorage.Save(Data);
            }
        }

        private List<Stat> OrdenarPorKills()
        {
            return this.Data.OrderByDescending(x => x.KillS).ToList();
        }

        public List<Stat> GetTop()
        {
            var lista = OrdenarPorKills();
            List<Stat> newList = new List<Stat>();
            int num = 0;
            foreach (var item in lista)
            {
                if(num > 3)
                {
                    break;
                }
                num++;
                newList.Add(item);
            }
            return newList;
        }

        public Stat GetStats(CSteamID iD)
        {
            return this.Data.Find(X => X.Id == iD);
        }

        public int GetRankOnStats(CSteamID iD)
        {
            var lista = OrdenarPorKills();

            return lista.FindLastIndex(x => x.Id == iD);
        }
        public void SumarHeadshot(CSteamID iD)
        {
            var data = this.Data.Find(X => X.Id == iD); data.Headshots++; this.DataStorage.Save(this.Data);
        }
        public void SumarKill(CSteamID iD)
        {
            var data = this.Data.Find(X => X.Id == iD); data.KillS++; this.DataStorage.Save(this.Data);
        }

        public void SumarMuerte(CSteamID iD)
        {
            var data = this.Data.Find(X => X.Id == iD); data.Deaths++; this.DataStorage.Save(this.Data);
        }

        public void Create(CSteamID id)
        {
            try
            {
                var data = this.Data.Find(X => X.Id == id);
                if (data == null) throw new Exception();

                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(id);
                data.Name = player.CharacterName;
                DataStorage.Save(this.Data);
            }
            catch
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(id);
                Stat stat = new Stat
                {
                    Deaths = 0,
                    Headshots = 0,
                    Id = id,
                    KillS = 0,
                    Name = player.CharacterName
                };
                this.Data.Add(stat);
                DataStorage.Save(this.Data);
            }
        }


    }
}
