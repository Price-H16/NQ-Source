﻿using System;
using System.Linq;
using System.Threading.Tasks;
using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject._ItemUsage;
using OpenNos.GameObject._ItemUsage.Event;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;

namespace Plugins.BasicImplementations.ItemUsage.Handler.NoFunction
{
   public class DefaultNoFunction : IUseItemRequestHandlerAsync
    {
        public ItemPluginType Type => ItemPluginType.NoFunction;
        public long EffectId => default;

        public async Task HandleAsync(ClientSession session, InventoryUseItemEvent e)
        {
            if (session.Character.IsVehicled)
            {
                session.SendPacket(
                    session.Character.GenerateSay(Language.Instance.GetMessageFromKey("CANT_DO_VEHICLED"), 10));
                return;
            }

            if (session.CurrentMapInstance.MapInstanceType == MapInstanceType.TalentArenaMapInstance)
            {
                return;
            }

            switch (e.Item.Item.Effect)
            {
                case 10:
                {
                    switch (e.Item.Item.EffectValue)
                    {
                        case 1:
                            if (session.Character.Inventory.CountItem(1036) < 1 ||
                                session.Character.Inventory.CountItem(1013) < 1)
                            {
                                return;
                            }

                            session.Character.Inventory.RemoveItemAmount(1036);
                            session.Character.Inventory.RemoveItemAmount(1013);
                            if (ServerManager.RandomNumber() < 25)
                            {
                                switch (ServerManager.RandomNumber(0, 2))
                                {
                                    case 0:
                                        session.Character.GiftAdd(1015, 1);
                                        break;

                                    case 1:
                                        session.Character.GiftAdd(1016, 1);
                                        break;
                                }
                            }

                            break;

                        case 2:
                            if (session.Character.Inventory.CountItem(1038) < 1 ||
                                session.Character.Inventory.CountItem(1013) < 1)
                            {
                                return;
                            }

                            session.Character.Inventory.RemoveItemAmount(1038);
                            session.Character.Inventory.RemoveItemAmount(1013);
                            if (ServerManager.RandomNumber() < 25)
                            {
                                switch (ServerManager.RandomNumber(0, 4))
                                {
                                    case 0:
                                        session.Character.GiftAdd(1031, 1);
                                        break;

                                    case 1:
                                        session.Character.GiftAdd(1032, 1);
                                        break;

                                    case 2:
                                        session.Character.GiftAdd(1033, 1);
                                        break;

                                    case 3:
                                        session.Character.GiftAdd(1034, 1);
                                        break;
                                }
                            }

                            break;

                        case 3:
                            if (session.Character.Inventory.CountItem(1037) < 1 ||
                                session.Character.Inventory.CountItem(1013) < 1)
                            {
                                return;
                            }

                            session.Character.Inventory.RemoveItemAmount(1037);
                            session.Character.Inventory.RemoveItemAmount(1013);
                            if (ServerManager.RandomNumber() < 25)
                            {
                                switch (ServerManager.RandomNumber(0, 17))
                                {
                                    case 0:
                                    case 1:
                                    case 2:
                                    case 3:
                                    case 4:
                                        session.Character.GiftAdd(1017, 1);
                                        break;

                                    case 5:
                                    case 6:
                                    case 7:
                                    case 8:
                                        session.Character.GiftAdd(1018, 1);
                                        break;

                                    case 9:
                                    case 10:
                                    case 11:
                                        session.Character.GiftAdd(1019, 1);
                                        break;

                                    case 12:
                                    case 13:
                                        session.Character.GiftAdd(1020, 1);
                                        break;

                                    case 14:
                                        session.Character.GiftAdd(1021, 1);
                                        break;

                                    case 15:
                                        session.Character.GiftAdd(1022, 1);
                                        break;

                                    case 16:
                                        session.Character.GiftAdd(1023, 1);
                                        break;
                                }
                            }

                            break;
                    }

                    session.Character.GiftAdd(1014, (byte) ServerManager.RandomNumber(5, 11));
                }
                    break;

                default:
                    Logger.Warn(string.Format(Language.Instance.GetMessageFromKey("NO_HANDLER_ITEM"), GetType(), e.Item.Item.VNum,
                        e.Item.Item.Effect, e.Item.Item.EffectValue));
                    break;
            }
        }
    }
}