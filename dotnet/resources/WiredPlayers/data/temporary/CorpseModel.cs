using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WiredPlayers.factions;

namespace WiredPlayers.data.temporary
{
    public class CorpseModel
    {
        public List<HitModel> HitList { get; set; }
        public Vector3 Location { get; set; }
        public Vector3 Rotation { get; set; }
        public String Name { get; set; }
        public GTANetworkAPI.Object Model { get; set; }
        public DateTime DeathTime { get; set; }
        private Timer DestroyTime { get; set; }


        public CorpseModel(List<HitModel> hitList,Vector3 location,Vector3 rotation,String name) 
        {
            HitList = hitList;
            Location = location;
            Rotation = rotation;
            Name = name;
            Model = NAPI.Object.CreateObject(1165866977, location, rotation);
            DeathTime = DateTime.Now;
            DestroyTime = new Timer(DestroyCorpse, null, 30000, 30000);
            Emergency.CorpseList.Add(this);
        }

        private void DestroyCorpse(object state)
        {
            this.DestroyTime.Dispose();
            this.Model.Delete();
            Emergency.CorpseList.Remove(this);
        }
    }
}
