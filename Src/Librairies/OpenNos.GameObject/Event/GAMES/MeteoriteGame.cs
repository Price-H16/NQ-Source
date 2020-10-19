using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;

namespace OpenNos.GameObject.Event.GAMES
{
    public static class MeteoriteGame
    {
        #region Methods

        public static void GenerateMeteoriteGame()
        {
            //ServerManager.Instance.Broadcast(
            //    UserInterfaceHelper.GenerateMsg(
            //        string.Format(Language.Instance.GetMessageFromKey("METEORITE_MINUTES"), 5), 0));
            //ServerManager.Instance.Broadcast(
            //    UserInterfaceHelper.GenerateMsg(
            //        string.Format(Language.Instance.GetMessageFromKey("METEORITE_MINUTES"), 5), 1));
            //Thread.Sleep(4 * 60 * 1000);
            //ServerManager.Instance.Broadcast(
            //    UserInterfaceHelper.GenerateMsg(
            //        string.Format(Language.Instance.GetMessageFromKey("METEORITE_MINUTES"), 1), 0));
            //ServerManager.Instance.Broadcast(
            //    UserInterfaceHelper.GenerateMsg(
            //        string.Format(Language.Instance.GetMessageFromKey("METEORITE_MINUTES"), 1), 1));
            //Thread.Sleep(30 * 1000);
            //ServerManager.Instance.Broadcast(
            //    UserInterfaceHelper.GenerateMsg(
            //        string.Format(Language.Instance.GetMessageFromKey("METEORITE_SECONDS"), 30), 0));
            //ServerManager.Instance.Broadcast(
            //    UserInterfaceHelper.GenerateMsg(
            //        string.Format(Language.Instance.GetMessageFromKey("METEORITE_SECONDS"), 30), 1));
            //Thread.Sleep(20 * 1000);
            //ServerManager.Instance.Broadcast(
            //    UserInterfaceHelper.GenerateMsg(
            //        string.Format(Language.Instance.GetMessageFromKey("METEORITE_SECONDS"), 10), 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_SECONDS"), 10), 1));
            Thread.Sleep(10 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("METEORITE_STARTED"), 1));
            ServerManager.Instance.Broadcast("qnaml 100 #guri^506 The Meteorite Game is starting! Join now!");
            ServerManager.Instance.EventInWaiting = true;
            Thread.Sleep(30 * 1000);
            ServerManager.Instance.Sessions.Where(s => s.Character?.IsWaitingForEvent == false).ToList().ForEach(s => s.SendPacket("esf"));
            ServerManager.Instance.EventInWaiting = false;
            var sessions = ServerManager.Instance.Sessions.Where(s => s.Character?.IsWaitingForEvent == true && s.Character.MapInstance.MapInstanceType == MapInstanceType.BaseMapInstance);

            var map = ServerManager.GenerateMapInstance(2004, MapInstanceType.EventGameInstance, new InstanceBag());
            if (map != null)
            {
                foreach (var sess in sessions) ServerManager.Instance.TeleportOnRandomPlaceInMap(sess, map.MapInstanceId);
                ServerManager.Instance.Sessions.Where(s => s.Character != null).ToList().ForEach(s => s.Character.IsWaitingForEvent = false);
                ServerManager.Instance.StartedEvents.Remove(EventType.METEORITEGAME);

                var task = new MeteoriteGameThread();
                Observable.Timer(TimeSpan.FromSeconds(10)).Subscribe(X => task.Run(map));
            }
        }

        #endregion

        #region Classes

        public class MeteoriteGameThread
        {
            #region Members

            private MapInstance _map;

            #endregion

            #region Methods

            public static void RemoveAllPetInTeam(ClientSession session)
            {
                foreach (var mate in session.Character.Mates.Where(s => s.IsTeamMember)) mate.RemoveTeamMember();
            }

            public void Run(MapInstance map)
            {
                _map = map;

                foreach (var session in _map.Sessions)
                {
                    RemoveAllPetInTeam(session);
                    ServerManager.Instance.TeleportOnRandomPlaceInMap(session, map.MapInstanceId);
                    if (session.Character.IsVehicled) session.Character.RemoveVehicle();
                    if (session.Character.UseSp)
                    {
                        session.Character.LastSp = (DateTime.Now - Process.GetCurrentProcess().StartTime.AddSeconds(-50)).TotalSeconds;
                        var specialist = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);
                        if (specialist != null) session?.Character.RemoveSp(specialist.ItemVNum, true);
                    }

                    session.Character.Speed = 12;
                    session.Character.IsVehicled = true;
                    session.Character.IsCustomSpeed = true;
                    session.Character.Morph = 1156;
                    session.Character.ArenaWinner = 0;
                    session.Character.MorphUpgrade = 0;
                    session.Character.MorphUpgrade2 = 0;
                    session.SendPacket(session.Character.GenerateCond());
                    session.Character.LastSpeedChange = DateTime.Now;
                    session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                }

                var i = 0;
                var shouldEnd = false;
                while (_map?.Sessions?.Any() == true && !shouldEnd) runRound(i++, ref shouldEnd);

                map.Sessions.Where(s => s.Character != null).ToList().ForEach(s => {
                    s.Character.RemoveBuffByBCardTypeSubType(new List<KeyValuePair<byte, byte>>()
                    {
                        new KeyValuePair<byte, byte>((byte)BCardType.CardType.SpecialActions, (byte)AdditionalTypes.SpecialActions.Hide),
                        new KeyValuePair<byte, byte>((byte)BCardType.CardType.FalconSkill, (byte)AdditionalTypes.FalconSkill.Hide),
                        new KeyValuePair<byte, byte>((byte)BCardType.CardType.FalconSkill, (byte)AdditionalTypes.FalconSkill.Ambush)
                    });
                    ServerManager.Instance.ChangeMap(s.Character.CharacterId, s.Character.MapId, s.Character.MapX, s.Character.MapY);
                });
            }

            private static IEnumerable<Tuple<short, int, short, short>> generateDrop(Map map, short vnum,
                int amountofdrop, int amount)
            {
                var dropParameters = new List<Tuple<short, int, short, short>>();
                for (var i = 0; i < amountofdrop; i++)
                {
                    var cell = map.GetRandomPosition();
                    dropParameters.Add(new Tuple<short, int, short, short>(vnum, amount, cell.X, cell.Y));
                }

                return dropParameters;
            }

            private void runRound(int number, ref bool shouldEnd)
            {
                var amount = 120 + 60 * number * 5;

                var i = amount;
                while (i != 0)
                {
                    spawnCircle(number);
                    Thread.Sleep(60000 / amount);
                    i--;
                }

                Thread.Sleep(5000);
                if (number < 5)
                {
                    _map.Broadcast(UserInterfaceHelper.GenerateMsg(
                        string.Format(Language.Instance.GetMessageFromKey("METEORITE_ROUND"), number + 1), 0));
                }
                else
                {
                    _map.Broadcast(UserInterfaceHelper.GenerateMsg("You won the meteorite game, congratulations!", 0));
                }

                Thread.Sleep(5000);

                // Your dropped reward
                _map.DropItems(generateDrop(_map.Map, 1046, 10, 50000 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 1030, 10, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2282, 10, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2514, 2, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2515, 2, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2516, 2, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2517, 2, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2518, 2, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2519, 2, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2520, 2, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 2521, 2, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                _map.DropItems(generateDrop(_map.Map, 5947, 25, 1 * (number + 1 > 1 ? 1 : number + 1)).ToList());
                foreach (var session in _map.Sessions)
                {
                    // Your reward that every player should get
                }

                Thread.Sleep(30000);

                if (number == 5)
                {
                    shouldEnd = true;
                }
            }

            private void spawnCircle(int round)
            {
                if (_map != null)
                {
                    var cell = _map.Map.GetRandomPosition();

                    var circleId = _map.GetNextMonsterId();

                    var circle = new MapMonster
                    {
                        MonsterVNum = 2018, MapX = cell.X, MapY = cell.Y, MapMonsterId = circleId, IsHostile = false,
                        IsMoving = false, ShouldRespawn = false
                    };
                    circle.Initialize(_map);
                    circle.NoAggresiveIcon = true;
                    _map.AddMonster(circle);
                    _map.Broadcast(circle.GenerateIn());
                    _map.Broadcast(StaticPacketHelper.GenerateEff(UserType.Monster, circleId, 4660));
                    Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe(observer =>
                    {
                        if (_map != null)
                        {
                            _map.Broadcast(StaticPacketHelper.SkillUsed(UserType.Monster, circleId, 3, circleId, 1220,
                                220, 0, 4983, cell.X, cell.Y, true, 0, 65535, 0, 0));
                            foreach (var character in _map.GetCharactersInRange(cell.X, cell.Y, 2))
                            {
                                if (!_map.Sessions.Skip(3).Any())
                                {
                                    // Your reward for the last three living players
                                }

                                character.IsCustomSpeed = false;
                                character.RemoveVehicle();
                                character.GetDamage(655350, character.BattleEntity);
                                Observable.Timer(TimeSpan.FromMilliseconds(1000)).Subscribe(o =>
                                    ServerManager.Instance.AskRevive(character.CharacterId));
                            }

                            _map.RemoveMonster(circle);
                            _map.Broadcast(StaticPacketHelper.Out(UserType.Monster, circle.MapMonsterId));
                        }
                    });
                }
            }

            #endregion
        }

        #endregion
    }
}