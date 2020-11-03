using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace OpenNos.GameObject.Event.RTR
{
    public static class RoundToRound
    {
        #region Properties
        static MapInstance map = ServerManager.GenerateMapInstance(2509, MapInstanceType.RoundToRound, new InstanceBag());

        static MapInstance nosvillemap = ServerManager.GetMapInstance(ServerManager.GetBaseMapInstanceIdByMapId(1));

        static int Round = 1;

        static short BossId = 0;
        #endregion

        #region Methods
        public static void Run()
        {
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ROUNDTOROUND_STARTED"), 1));
            ServerManager.Instance.Sessions.Where(s => s.Character?.MapInstance.MapInstanceType == MapInstanceType.BaseMapInstance).ToList().ForEach(s => s.SendPacket($"qnaml 7 #guri^506 {Language.Instance.GetMessageFromKey("ROUNDTOROUND_QUESTION")}"));
            ServerManager.Instance.EventInWaiting = true;

            Thread.Sleep(30 * 1000);

            IEnumerable<ClientSession> sessions = ServerManager.Instance.Sessions.Where(s => s.Character?.IsWaitingForEvent == true);

            foreach (ClientSession s in sessions)
            {
                ServerManager.Instance.ChangeMapInstance(s.Character.CharacterId, map.MapInstanceId, 64, 81);
                s.Character.IsWaitingForEvent = false;
                Thread.Sleep(100);
            }
            ServerManager.Instance.EventInWaiting = false;
            GenerateRound();
        }

        static void GenerateRound(MapMonster Boss = null)
        {
            switch (Round)
            {
                case 1:
                    BossId = 556;
                    break;
                case 2:
                    BossId = 2678;
                    break;
                case 3:
                    BossId = 2504;
                    break;
                case 4:
                    BossId = 2514;
                    break;
                case 5:
                    BossId = 2574;
                    break;
                case 6:
                    BossId = 3027;
                    break;
                case 7:
                    BossId = 3028;
                    break;
                case 8:
                    BossId = 3029;
                    break;
            }
            List<MonsterToSummon> SummonParameters = new List<MonsterToSummon>();
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ROUNDTOROUND_BOSSINCOMING"), 1));
            Thread.Sleep(30 * 1000);
            switch (Round)
            {
                case 1:
                    SummonParameters.AddRange(map.Map.GenerateMonsters(620, 30, true, new List<EventContainer>()));
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(1), new EventContainer(map, EventActionType.SPAWNMONSTERS, SummonParameters));
                    break;
                case 2:
                    SummonParameters.AddRange(map.Map.GenerateMonsters(444, 45, true, new List<EventContainer>()));
                    SummonParameters.AddRange(map.Map.GenerateMonsters(2662, 1, true, new List<EventContainer>()));
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(1), new EventContainer(map, EventActionType.SPAWNMONSTERS, SummonParameters));
                    break;
                case 3:
                    SummonParameters.AddRange(map.Map.GenerateMonsters(1903, 50, true, new List<EventContainer>()));
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(1), new EventContainer(map, EventActionType.SPAWNMONSTERS, SummonParameters));
                    break;
                case 4:
                    SummonParameters.AddRange(map.Map.GenerateMonsters(1925, 30, true, new List<EventContainer>()));
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(1), new EventContainer(map, EventActionType.SPAWNMONSTERS, SummonParameters));
                    break;
                case 5:
                    SummonParameters.AddRange(map.Map.GenerateMonsters(2560, 30, true, new List<EventContainer>()));
                    SummonParameters.AddRange(map.Map.GenerateMonsters(2559, 30, true, new List<EventContainer>()));
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(1), new EventContainer(map, EventActionType.SPAWNMONSTERS, SummonParameters));
                    break;
                case 6:
                    SummonParameters.AddRange(map.Map.GenerateMonsters(3040, 40, true, new List<EventContainer>()));
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(1), new EventContainer(map, EventActionType.SPAWNMONSTERS, SummonParameters));
                    break;
                case 7:
                    SummonParameters.AddRange(map.Map.GenerateMonsters(3015, 35, true, new List<EventContainer>()));
                    SummonParameters.AddRange(map.Map.GenerateMonsters(3103, 35, true, new List<EventContainer>()));
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(1), new EventContainer(map, EventActionType.SPAWNMONSTERS, SummonParameters));
                    break;
                case 8:
                    SummonParameters.AddRange(map.Map.GenerateMonsters(3108, 40, true, new List<EventContainer>()));
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(1), new EventContainer(map, EventActionType.SPAWNMONSTERS, SummonParameters));
                    break;
            }
            List<EventContainer> onDeathEvents = new List<EventContainer>
            {
               new EventContainer(map, EventActionType.NEXTROUND, (byte)1)
            };
            MapMonster BossMonster = new MapMonster
            {
                MonsterVNum = BossId,
                MapY = 68,
                MapX = 71,
                MapId = map.Map.MapId,
                Position = 2,
                IsMoving = true,
                MapMonsterId = map.GetNextMonsterId(),
                ShouldRespawn = false
            };
            BossMonster.Initialize(map);
            map.AddMonster(BossMonster);
            MapMonster Exist = map.Monsters.Find(s => s.Monster.NpcMonsterVNum == BossId);
            if (Exist != null)
            {
                Exist.BattleEntity.OnDeathEvents = onDeathEvents;
                Exist.IsBoss = true;
                map.LoadMonsters();
            }
            IEnumerable<ClientSession> sessions = ServerManager.Instance.Sessions.Where(s => s.CurrentMapInstance.MapInstanceType == map.MapInstanceType);
            foreach (ClientSession Session in sessions)
            {
                Session.SendPacket(Exist.GenerateIn());
                Session.SendPacket(Exist.GenerateBoss());
            }
        }

        public static void GenerateGiftAdd()
        {
            IEnumerable<ClientSession> sess = ServerManager.Instance.Sessions.Where(s => s.Character?.IsWaitingForEvent == false && s.Character.MapInstance.MapInstanceType == MapInstanceType.RoundToRound);
            switch (Round)
            {
                case 1:
                    foreach (ClientSession s in sess)
                    {
                        s.Character.GiftAdd(2330, 5);
                        s.Character.GiftAdd(5746, 2);
                        s.Character.GiftAdd(2143, 1);
                    }
                    break;
                case 2:
                    foreach (ClientSession s in sess)
                    {
                        s.Character.GiftAdd(2330, 10);
                        s.Character.GiftAdd(5746, 5);
                        s.Character.GiftAdd(2143, 1);
                    }
                    break;
                case 3:
                    foreach (ClientSession s in sess)
                    {
                        s.Character.GiftAdd(2331, 2);
                        s.Character.GiftAdd(1669, 1);
                        s.Character.GiftAdd(5985, 1);
                        s.Character.GiftAdd(2143, 1);
                    }
                    break;
                case 4:
                    foreach (ClientSession s in sess)
                    {
                        s.Character.GiftAdd(2331, 3);
                        s.Character.GiftAdd(1669, 1);
                        s.Character.GiftAdd(5985, 2);
                        s.Character.GiftAdd(2143, 1);
                    }
                    break;
                case 5:
                    foreach (ClientSession s in sess)
                    {
                        s.Character.GiftAdd(2331, 5);
                        s.Character.GiftAdd(1669, 3);
                        s.Character.GiftAdd(1651, 1);
                        s.Character.GiftAdd(2143, 1);
                    }
                    break;
                case 6:
                    foreach (ClientSession s in sess)
                    {
                        s.Character.GiftAdd(2331, 6);
                        s.Character.GiftAdd(1669, 1);
                        s.Character.GiftAdd(1652, 1);
                        s.Character.GiftAdd(2143, 1);
                    }
                    break;
                case 7:
                    foreach (ClientSession s in sess)
                    {
                        s.Character.GiftAdd(2332, 2);
                        s.Character.GiftAdd(1669, 1);
                        s.Character.GiftAdd(1655, 1);
                        s.Character.GiftAdd(2143, 1);
                    }
                    break;
                case 8:
                    foreach (ClientSession s in sess)
                    {
                        s.Character.GiftAdd(2332, 4);
                        s.Character.GiftAdd(1669, 1);
                        s.Character.GiftAdd(1655, 1);
                        s.Character.GiftAdd(2143, 1);
                    }
                    EndEvent();
                    return;
            }
            Round += 1;
            GenerateRound();
        }

        static void EndEvent()
        {
            if (Round >= 8)
            {
                Round = 1;
                ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ROUNDTOROUND_SUCCESS"), 1));
                Thread.Sleep(10 * 1000);
                map.Sessions.ToList().ForEach(Session =>
                {
                    ServerManager.Instance.ChangeMapInstance(Session.Character.CharacterId, nosvillemap.MapInstanceId, 79, 117);
                });
            }
        }
        #endregion
    }
}
