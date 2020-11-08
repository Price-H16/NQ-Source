using NosTale.Extension.GameExtension.Character;
using NosTale.Packets.Packets.CommandPackets;
using OpenNos.Core;
using OpenNos.GameObject;
using OpenNos.GameObject.Extension;
using OpenNos.GameObject.Helpers;

namespace OpenNos.Handler.PacketHandler.Command
{
    public class BankHandler : IPacketHandler
    {
        #region Instantiation

        public BankHandler(ClientSession session) => Session = session;

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods

        public void BankManagement(BankPacket bankPacket)
        {
            Session.AddLogsCmd(bankPacket);
            Session.OpenBank();
        }

        #endregion
    }
}