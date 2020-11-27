using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using OpenNos.Master.Library.Client;
using OpenNos.Master.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenNos.GameObject.Event.WORLDBOSS
{

    public static class WorldBoss
    {
        #region Properties

        public static int AngelDamage { get; set; }

        public static MapInstance WorldMapinstance { get; set; }

        public static int DemonDamage { get; set; }

        public static bool IsLocked { get; set; }

        public static bool IsRunning { get; set; }

        public static int RemainingTime { get; set; }

        public static MapInstance UnknownLandMapInstance { get; set; }

        #endregion

        #region Methods

        public static void Run()
        {
            WolrdBoss raidThread = new WolrdBoss();
            Observable.Timer(TimeSpan.FromMinutes(0)).Subscribe(X => raidThread.Run());
        }

        #endregion
    }

    public class WolrdBoss
    {
        #region Methods

        public void Run()
        {
            CommunicationServiceClient.Instance.SendMessageToCharacter(new SCSCharacterMessage
            {
                DestinationCharacterId = null,
                SourceCharacterId = 0,
                SourceWorldId = ServerManager.Instance.WorldId,
                Message = "The fearsome boss Fafnir has appeared in Nosville, get his precious treasure!",
                Type = MessageType.Shout
            });

            WorldBoss.RemainingTime = 3600;
            const int interval = 1;

            WorldBoss.WorldMapinstance = ServerManager.GenerateMapInstance(2610, MapInstanceType.WorldBossInstance, new InstanceBag());
            WorldBoss.UnknownLandMapInstance = ServerManager.GetMapInstance(ServerManager.GetBaseMapInstanceIdByMapId(1));



            WorldBoss.WorldMapinstance.CreatePortal(new Portal
            {
                SourceMapInstanceId = WorldBoss.WorldMapinstance.MapInstanceId,
                SourceX = 103,
                SourceY = 82,
                DestinationMapId = 0,
                DestinationX = 79,
                DestinationY = 104,
                DestinationMapInstanceId = WorldBoss.UnknownLandMapInstance.MapInstanceId,
                Type = 6
            });

            WorldBoss.UnknownLandMapInstance.CreatePortal(new Portal
            {
                SourceMapId = 1,
                SourceX = 79,
                SourceY = 104,
                DestinationMapId = 0,
                DestinationX = 103,
                DestinationY = 82,
                DestinationMapInstanceId = WorldBoss.WorldMapinstance.MapInstanceId,
                Type = 6
            });

            List<EventContainer> onDeathEvents = new List<EventContainer>
            {
               new EventContainer(WorldBoss.WorldMapinstance, EventActionType.SCRIPTEND, (byte)1)
            };

            #region Fafnir

            MapMonster FafnirMonster = new MapMonster
            {
                MonsterVNum = 2619,
                MapY = 71,
                MapX = 87,
                MapId = WorldBoss.WorldMapinstance.Map.MapId,
                Position = 2,
                IsMoving = true,
                MapMonsterId = WorldBoss.WorldMapinstance.GetNextMonsterId(),
                ShouldRespawn = false
            };
            FafnirMonster.Initialize(WorldBoss.WorldMapinstance);
            WorldBoss.WorldMapinstance.AddMonster(FafnirMonster);
            MapMonster Fafnir = WorldBoss.WorldMapinstance.Monsters.Find(s => s.Monster.NpcMonsterVNum == 2619);
            if (Fafnir != null)
            {
                Fafnir.BattleEntity.OnDeathEvents = onDeathEvents;
                Fafnir.IsBoss = true;
            }
            #endregion
            try
            {
                Observable.Timer(TimeSpan.FromMinutes(15)).Subscribe(X => LockRaid());
                Observable.Timer(TimeSpan.FromMinutes(60)).Subscribe(X => EndRaid());
            }
            catch (Exception ex)
            {

            }



        }

        public void EndRaid()
        {
            try
            {
                ServerManager.Shout(Language.Instance.GetMessageFromKey("WORDLBOSS_END"), true);

                foreach (ClientSession sess in WorldBoss.WorldMapinstance.Sessions.ToList())
                {
                    ServerManager.Instance.ChangeMapInstance(sess.Character.CharacterId, WorldBoss.UnknownLandMapInstance.MapInstanceId, sess.Character.MapX, sess.Character.MapY);
                    Thread.Sleep(100);
                }
                WorldBoss.IsRunning = false;
                WorldBoss.AngelDamage = 0;
                WorldBoss.DemonDamage = 0;
                ServerManager.Instance.StartedEvents.Remove(EventType.WORLDBOSS);
            }
            catch (Exception ex)
            {

            }

        }

        private void LockRaid()
        {
            foreach (Portal p in WorldBoss.UnknownLandMapInstance.Portals.Where(s => s.DestinationMapInstanceId == WorldBoss.WorldMapinstance.MapInstanceId).ToList())
            {
                WorldBoss.UnknownLandMapInstance.Portals.Remove(p);
                WorldBoss.UnknownLandMapInstance.Broadcast(p.GenerateGp());
            }
            ServerManager.Shout(Language.Instance.GetMessageFromKey("WORLDBOSS_LOCKED"), true);
            WorldBoss.IsLocked = true;
        }

        #endregion
    }
}
