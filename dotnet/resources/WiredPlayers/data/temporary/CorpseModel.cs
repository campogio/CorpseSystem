using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace WiredPlayers.data.temporary
{
    class CorpseModel
    {
        public List<HitModel> HitList { get; set; }
        public Vector3 Location { get; set; }
        public Vector3 Rotation { get; set; }
        public String Name { get; set; }
        public GTANetworkAPI.Object Model { get; set; }
        public DateTime deathTime { get; set; }


        public CorpseModel(List<HitModel> hitList,Vector3 location,Vector3 rotation,String name) 
        {
            HitList = hitList;
            Location = location;
            Rotation = rotation;
            Name = name;
            Model = NAPI.Object.CreateObject(1165866977, location, rotation);
            deathTime = DateTime.Now;

        }
    }
}
