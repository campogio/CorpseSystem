﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace WiredPlayers.data.temporary
{
    public class HitModel
    {
        public ulong Weaponhash;
        public ulong Boneidx;
        public int Damage;
        public string BoneString;
        public string WeaponString;
        public HitModel(ulong weaponhash, ulong boneidx, int damage)
        {
            // Initialize the required values
            Weaponhash = weaponhash;
            Boneidx = boneidx;
            Damage = damage;

            WeaponString = weaponhash switch
            {
                453432689 => "Pistol",
                3219281620 => "Pistol MK2",
                1593441988 => "Combat Pistol",
                584646201 => "AP Pistol",
                2578377531 => "Pistol .50",
                3218215474 => "SNS Pistol",
                2285322324 => "SNS Pistol MK2",
                727643628 => "Ceramic Pisol",
                324215364 => "Micro SMG",
                736523883 => "SMG",
                2024373456 => "SMG MK2",
                4024951519 => "Assault SMG",
                171789620 => "Combat PDW",
                3675956304 => "Machine Pistol",
                3173288789 => "Mini SMG",
                487013001 => "Pump Shotgun",
                1432025498 => "Pump Shotgun MK2",
                2017895192 => "Sawed-Off Shotgun",
                3800352039 => "Assault Shotgun",
                2640438543 => "Bullpup Shotgun",
                2828843422 => "Musket",
                984333226 => "Heavy Shotgun",
                4019527611 => "Double Barrel Shotgun",
                317205821 => "Sweeper Shotgun",
                94989220 => "Combat Shotgun",
                3220176749 => "Assault Rifle",
                961495388 => "Assault Rifle Mk 2",
                2210333304 => "Carbine Rifle",
                4208062921 => "Carbine Rifle Mk 2",
                2937143193 => "Advanced Rifle",
                3231910285 => "Special Carbine",
                2526821735 => "Special Carbine Mk 2",
                2132975508 => "Bullpup Rifle",
                2228681469 => "Bullpup Rifle Mk 2",
                1649403952 => "Compact Rifle",
                2636060646 => "Military Rifle",
                2634544996 => "MachineGun",
                2144741730 => "Combat MG",
                3686625920 => "Combat MG Mk 2",
                1627465347 => "Gusenberg Sweeper",
                100416529 => "Sniper Rifle",
                205991906 => "Heavy Sniper",
                177293209 => "Heavy Sniper Mk 2",
                3342088282 => "Marksman Rifle",
                1785463520 => "Marksman Rifle Mk 2",
                _ => "Unknown"
            };


            BoneString = boneidx switch
            {
                0=> "torso",


                //values for mp_f_freemode_01
                140698833649684 => "head",

                140698833649678 => "left-arm",
                140698833649676 => "left-arm",
                140698833649680 => "right-arm",
                140698833649682 => "right-arm",
                140698833649674 => "torso",
                140698833649666 => "left-leg",
                140698833649664 => "left-leg",
                140698833649670 => "right-leg",
                140698833649668 => "right-leg",



                //values for mp_m_freemode_01
                140694538682388 => "head",

                140694538682380 => "left-arm",
                140694538682382 => "left-arm",

                140694538682386 => "right-arm",
                140694538682384 => "right-arm",

                140694538682370 => "left-leg",
                140694538682368 => "left-leg",

                140694538682374 => "right-leg",
                140694538682372 => "right-leg",

                1356 => "head",
                2108 => "left-leg",
                2992 => "right-arm",
                4089 => "left-arm",
                4090 => "left-arm",
                4137 => "left-arm",
                4138 => "left-arm",
                4153 => "left-arm",
                4154 => "left-arm",
                4169 => "left-arm",
                4170 => "left-arm",
                4185 => "left-arm",
                4186 => "left-arm",
                5232 => "left-arm",
                6286 => "right-arm",
                6442 => "right-leg",
                10706 => "torso",
                11174 => "head",
                11816 => "torso",
                12844 => "head",
                14201 => "left-leg",
                16335 => "right-leg",
                17188 => "head",
                17719 => "head",
                18905 => "left-arm",
                19336 => "head",
                20178 => "head",
                20279 => "head",
                20623 => "head",
                20781 => "right-leg",
                21550 => "head",
                22711 => "left-arm",
                23553 => "torso",
                23639 => "left-leg",
                24806 => "right-leg",
                24816 => "torso",
                24817 => "torso",
                24818 => "torso",
                25260 => "head",
                26610 => "left-arm",
                26611 => "left-arm",
                26612 => "left-arm",
                26613 => "left-arm",
                26614 => "left-arm",
                27474 => "head",
                28252 => "right-arm",
                28422 => "right-arm",
                29868 => "head",
                31086 => "head",
                35502 => "right-leg",
                35731 => "torso",
                36029 => "left-arm",
                36864 => "right-leg",
                37119 => "right-arm",
                37193 => "head",
                39317 => "torso",
                40269 => "right-arm",
                43536 => "head",
                43810 => "right-arm",
                45509 => "left-arm",
                45750 => "head",
                46078 => "left-leg",
                46240 => "head",
                47419 => "head",
                47495 => "head",
                49979 => "head",
                51826 => "right-leg",
                52301 => "right-leg",
                56604 => "torso",
                57005 => "right-arm",
                57597 => "torso",
                57717 => "left-leg",
                58271 => "left-leg",
                58331 => "head",
                58866 => "right-arm",
                58867 => "right-arm",
                58868 => "right-arm",
                58869 => "right-arm",
                58870 => "right-arm",
                60309 => "left-arm",
                61007 => "left-arm",
                61163 => "left-arm",
                61839 => "head",
                63931 => "left-leg",
                64016 => "right-arm",
                64017 => "right-arm",
                64064 => "right-arm",
                64065 => "right-arm",
                64080 => "right-arm",
                64081 => "right-arm",
                64096 => "right-arm",
                64097 => "right-arm",
                64112 => "right-arm",
                64113 => "right-arm",
                64729 => "left-leg",
                65068 => "head",
                65245 => "left-leg",

                _ => "torso"
            };
        }
    }
}
