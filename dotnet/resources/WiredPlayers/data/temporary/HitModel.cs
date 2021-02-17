using System;
using System.Collections.Generic;
using System.Text;

namespace WiredPlayers.data.temporary
{
    class HitModel
    {
        public ulong Weaponhash;
        public ulong Boneidx;
        public int Damage;
        public string BoneString;
        public HitModel(ulong weaponhash, ulong boneidx, int damage, string bonestring)
        {
            // Initialize the required values
            Weaponhash = weaponhash;
            Boneidx = boneidx;
            Damage = damage;
            BoneString = bonestring;
        }
    }
}
