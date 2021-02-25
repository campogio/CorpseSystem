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
        private Timer FireTime { get; set; }

        public TextLabel ExamineLabel { get; set; }


        public CorpseModel(List<HitModel> hitList,Vector3 location,Vector3 rotation,String name) 
        {
            HitList = new List<HitModel>();
            foreach(HitModel hit in hitList)
            {
                HitList.Add(hit);
            }
            Location = location;
            Rotation = rotation;
            Name = name;
            Model = NAPI.Object.CreateObject(1165866977, location, rotation);
            DeathTime = DateTime.Now;
            DestroyTime = new Timer(DestroyCorpse, null, 6000000, 6000000);
            ExamineLabel= NAPI.TextLabel.CreateTextLabel("/esamina",new Vector3(this.Location.X,this.Location.Y,this.Location.Z+.5f), 5.0f, 0.75f, 4, new Color(255, 255, 255));
        }

        public void FireCorpse(Player player)
        {
            player.SendChatMessage("Questo cadavere verrà distrutto tra 10 minuti.");

            FireTime = new Timer(DestroyCorpse, null, 600000, 600000);
        }



        public void DestroyCorpse(object state)
        {
            try
            {
                NAPI.Task.Run(() =>
                {
                    this.Model.Delete();
                    this.ExamineLabel.Delete();
                });
                this.DestroyTime.Dispose();
                this.FireTime.Dispose();
                this.HitList.Clear();
                
                Emergency.CorpseList.Remove(this);
            }catch(Exception e)
            {
                NAPI.Util.ConsoleOutput(e.StackTrace);
            }
        }

        public void MovingCorpse()
        {

            // Delete all visual components related to the corpse while it'se being moved.
            try
            {
                this.Model.Delete();
                this.ExamineLabel.Text = "";
                this.Location = null;
            }catch(Exception e)
            {
                NAPI.Util.ConsoleOutput(e.StackTrace);
            }
        }

        public void MoveCorpse(Vector3 position,Player player)
        {
            try
            {
                if (this.DeathTime.AddMinutes(1).CompareTo(DateTime.Now) > 0)
                {
                    this.Location = position;
                    
                    this.ExamineLabel.Position = new Vector3(this.Location.X, this.Location.Y, this.Location.Z + 1f);
                    this.ExamineLabel.Text = "/esamina";
                    this.Model = NAPI.Object.CreateObject(1165866977, new Vector3(this.Location.X, this.Location.Y, this.Location.Z), this.Rotation); ;
                }
                else
                {
                    player.SendChatMessage("The corpse has decomposed.");
                }
            }
            catch(Exception e)
            {
                NAPI.Util.ConsoleOutput(e.StackTrace);
            }
            
        }
    }
}
