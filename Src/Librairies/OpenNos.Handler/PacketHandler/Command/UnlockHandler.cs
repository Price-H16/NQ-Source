//using NosTale.Packets.Packets.CommandPackets;
//using OpenNos.Core;
//using OpenNos.GameObject;
//using OpenNos.GameObject.Extension;
//using OpenNos.GameObject.Networking;

//namespace OpenNos.Handler.PacketHandler.Command
//{
//    public class UnlockHandler : IPacketHandler
//    {
//        #region Instantiation

//        public UnlockHandler(ClientSession session) => Session = session;

//        #endregion

//        #region Properties

//        public ClientSession Session { get; }

//        #endregion

//        #region Methods

//        public void UnlockCharacter(UnlockPacket packet)
//        {
//            if (packet != null)
//            {
//                if (Session.Character.LockCode == CryptographyBase.Sha512(packet.Code))
//                {
//                    Session.SendPacket(Session.Character.GenerateSay("Unlocked", 10));
//                    Session.Character.IsLocked = false;
//                }
//                else
//                {
//                    Session.SendPacket(Session.Character.GenerateSay("Wrong password", 10));
//                }
//            }
//        }

//        #endregion
//    }
//}
