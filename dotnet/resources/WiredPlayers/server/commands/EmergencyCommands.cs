using GTANetworkAPI;
using WiredPlayers.Data;
using WiredPlayers.Data.Persistent;
using WiredPlayers.factions;
using WiredPlayers.Utility;
using WiredPlayers.messages.error;
using WiredPlayers.messages.information;
using WiredPlayers.messages.success;
using WiredPlayers.chat;
using static WiredPlayers.Utility.Enumerators;
using WiredPlayers.Data.Temporary;
using System.Threading.Tasks;
using System.Collections.Generic;
using WiredPlayers.data.temporary;
using System;

namespace WiredPlayers.Server.Commands
{
    public static class EmergencyCommands
    {


        [Command]
        public static void HealCommand(Player player, string targetString)
        {
            Player target = UtilityFunctions.GetPlayer(targetString);

            if (target == null || player.Position.DistanceTo(target.Position) > 2.5f)
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_too_far);
                return;
            }

            if (player.GetExternalData<CharacterModel>((int)ExternalDataSlot.Database).Faction != PlayerFactions.Emergency)
            {
                // The player is not a medic
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_not_emergency_faction);
                return;
            }

            if (target.Health >= 100)
            {
                // The target player is not injured
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_not_hurt);
                return;
            }

            // We heal the character
            target.Health = 100;

            // Send the message to the players close
            string message = string.Format(InfoRes.medic_reanimated, player.Name, target.Name);
            Chat.SendMessageToNearbyPlayers(player, message, ChatTypes.Me, player.Dimension > 0 ? 7.5f : 20.0f);

            // Send the confirmation message to both players
            player.SendChatMessage(Constants.COLOR_INFO + string.Format(InfoRes.medic_healed_player, target.Name));
            target.SendChatMessage(Constants.COLOR_INFO + string.Format(InfoRes.player_healed_medic, player.Name));
        }

        [Command]
        public static async Task ReanimateCommand(Player player, string targetString)
        {
            // Get the character model for the player
            CharacterModel characterModel = player.GetExternalData<CharacterModel>((int)ExternalDataSlot.Database);

            if (characterModel.Faction != PlayerFactions.Emergency)
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_not_emergency_faction);
                return;
            }

            if (!characterModel.OnDuty)
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_not_on_duty);
                return;
            }

            if (Emergency.IsPlayerDead(player))
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_is_dead);
                return;
            }

            if (Emergency.GetRemainingBlood() == 0)
            {
                // There's no blood left
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.no_blood_left);
                return;
            }

            // Get the target client
            Player target = UtilityFunctions.GetPlayer(targetString);

            if (target == null || player.Position.DistanceTo(target.Position) > 2.5f)
            {
                // Need to be closer to the patient
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_too_far);
                return;
            }

            if (!Emergency.IsPlayerDead(target))
            {
                // The player is not dead
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_not_dead);
                return;
            }

            // We create blood model
            BloodModel bloodModel = new BloodModel();
            {
                bloodModel.Doctor = player.GetExternalData<CharacterModel>((int)ExternalDataSlot.Database).Id;
                bloodModel.Patient = target.GetExternalData<CharacterModel>((int)ExternalDataSlot.Database).Id;
                bloodModel.Type = string.Empty;
                bloodModel.Used = true;
            }

            // Cancel the player's death
            Emergency.CancelPlayerDeath(target);

            // Add the blood consumption to the database
            bloodModel.Id = await DatabaseOperations.AddBloodTransaction(bloodModel).ConfigureAwait(false);
            Emergency.BloodList.Add(bloodModel);

            // Send the confirmation message to both players
            player.SendChatMessage(Constants.COLOR_ADMIN_INFO + string.Format(InfoRes.player_reanimated, target.Name));
            target.SendChatMessage(Constants.COLOR_SUCCESS + string.Format(SuccRes.target_reanimated, player.Name));
        }

        [Command]
        public static async Task ExtractCommand(Player player, string targetString)
        {
            // Get the character model for the player
            CharacterModel characterModel = player.GetExternalData<CharacterModel>((int)ExternalDataSlot.Database);

            if (Emergency.IsPlayerDead(player))
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_is_dead);
                return;
            }

            if (!characterModel.OnDuty)
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_not_on_duty);
                return;
            }

            if (characterModel.Faction != PlayerFactions.Emergency)
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_not_emergency_faction);
                return;
            }

            // Get the target client
            Player target = UtilityFunctions.GetPlayer(targetString);

            if (target == null || player.Position.DistanceTo(target.Position) > 5.0f)
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_too_far);
                return;
            }

            if (target.Health <= 15)
            {
                player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.low_blood);
                return;
            }

            // Substract the blood from the player
            target.Health -= 15;

            // We create the blood model
            BloodModel blood = new BloodModel();
            {
                blood.Doctor = characterModel.Id;
                blood.Patient = target.GetExternalData<CharacterModel>((int)ExternalDataSlot.Database).Id;
                blood.Type = string.Empty;
                blood.Used = false;
            }

            // We add the blood unit to the database
            blood.Id = await DatabaseOperations.AddBloodTransaction(blood).ConfigureAwait(false);
            Emergency.BloodList.Add(blood);

            player.SendChatMessage(Constants.COLOR_INFO + string.Format(InfoRes.blood_extracted, target.Name));
            target.SendChatMessage(Constants.COLOR_INFO + string.Format(InfoRes.blood_given, player.Name));
        }
        

            [Command]
        public static void DieCommand(Player player)
        {
            try
            {


                // Check if the player is dead
                if (!player.HasData(EntityData.TimeHospitalRespawn))
                {
                    player.SendChatMessage(Constants.COLOR_ERROR + ErrRes.player_not_dead);
                    return;
                }

                int totalSeconds = UtilityFunctions.GetTotalSeconds();


                /*
                if (player.GetData<int>(EntityData.TimeHospitalRespawn) > totalSeconds)
                {
                    player.SendChatMessage(Constants.COLOR_INFO + InfoRes.death_time_not_passed);
                    return;
                }
                */

                // Get player position to place corpse

                Vector3 corpsePos = player.Position;
                Vector3 corpseRot = player.Rotation;
                // Get player hitlist

                List<HitModel> hitList = player.GetExternalData<PlayerTemporaryModel>((int)ExternalDataSlot.Ingame).HitList;

                // Generate corpse with player's info

                CorpseModel corpse = new CorpseModel(player.GetExternalData<PlayerTemporaryModel>((int)ExternalDataSlot.Ingame).HitList, corpsePos, corpseRot, player.Name);

                // Add new corpse to list of corpses

                Emergency.CorpseList.Add(corpse);

                // clear the player's hitlist
                player.GetExternalData<PlayerTemporaryModel>((int)ExternalDataSlot.Ingame).HitList.Clear();

                // Move player to the hospital
                Emergency.TeleportPlayerToHospital(player);

                // Get the report generated with the death
                FactionWarningModel factionWarn = Faction.GetFactionWarnByTarget(player.Value, PlayerFactions.Emergency);

                if (factionWarn != null)
                {
                    if (factionWarn.TakenBy >= 0)
                    {
                        // Tell the player who attended the report it's been canceled
                        Player doctor = UtilityFunctions.GetPlayer(factionWarn.TakenBy);
                        doctor.SendChatMessage(Constants.COLOR_INFO + InfoRes.faction_warn_canceled);
                    }

                    // Remove the report from the list
                    Faction.factionWarningList.Remove(factionWarn);
                }
                //TODO: Getplayerpos, add corpse w/ attached HitList,empty player Hitlist

            }catch(Exception e)
            {
                NAPI.Util.ConsoleOutput(e.StackTrace);
            }
            
        }

        [Command]
        public static void ExamineCommand(Player player)
        {
            foreach(CorpseModel corpse in Emergency.CorpseList)
            {
                if (player.Position.DistanceToSquared2D(corpse.Location)<5)
                {
                    player.SendChatMessage(corpse.Name);
                    player.SendChatMessage($"{corpse.HitList.Count}");

                    foreach (HitModel hit in corpse.HitList)
                    {
                        player.SendChatMessage($"Danno : {hit.Damage} |Arma : {hit.WeaponString} |BoneID : {hit.Boneidx} | BoneZone : {hit.BoneString}");
                    }

                    //Pass info to corpseInspection.html

                    player.TriggerEvent("showCorpse", NAPI.Util.ToJson(corpse));


                }
            }
        }

        [Command]
        public static void CarryCorpseCommand(Player player)
        {

            try
            {
                //check if player is already carrying corpse,drop current corpse, if not, take closest corpse and carry it
                if (player.GetExternalData<PlayerTemporaryModel>((int)ExternalDataSlot.Ingame).CarriedCorpse != null)
                {
                    player.SendChatMessage($"You stopped carrying {player.GetExternalData<PlayerTemporaryModel>((int)ExternalDataSlot.Ingame).CarriedCorpse.Name}'s corpse.");

                    player.TriggerEvent("returnGroundPos",player.Position.X, player.Position.Y,player.Position.Z);
                    
                }
                else
                {

                    //get corpse closest to player, and assign it for carrying
                    CorpseModel closestCorpse = null;
                    float closestDistance = 5;
                    float newDistance;

                    foreach (CorpseModel corpse in Emergency.CorpseList)
                    {

                        newDistance = player.Position.DistanceToSquared2D(corpse.Location);
                        if (newDistance < closestDistance)
                        {
                            closestDistance = newDistance;
                            closestCorpse = corpse;
                        }

                    }

                    //get corpse in temporary variable and off the ground if it exists, else say there's no corpses in range
                    if (closestCorpse != null)
                    {
                        player.GetExternalData<PlayerTemporaryModel>((int)ExternalDataSlot.Ingame).CarriedCorpse = closestCorpse;
                        closestCorpse.MovingCorpse();
                        player.SendChatMessage($"You are now Carrying {player.GetExternalData<PlayerTemporaryModel>((int)ExternalDataSlot.Ingame).CarriedCorpse.Name}'s corpse.");
                    }
                    else
                    {
                        player.SendChatMessage("There are no corpses in range.");
                    }
                }

            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.StackTrace);
            }
            


        }

    }
    }

