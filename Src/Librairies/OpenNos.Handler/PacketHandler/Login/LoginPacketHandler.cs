using System;
using System.Linq;
using NosTale.Configuration;
using NosTale.Configuration.Utilities;
using NosTale.Extension.GameExtension.Packet;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.DAL;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.Master.Library.Client;

namespace OpenNos.Handler.BasicPacket.Login
{
    public class LoginPacketHandler : IPacketHandler
    {
        #region Members

        private readonly ClientSession _session;

        #endregion

        #region Instantiation

        public LoginPacketHandler(ClientSession session)
        {
            _session = session;
        }

        #endregion

        #region Methods

        public void VerifyLogin(LoginPacket loginPacket)
        {
            if (loginPacket == null || loginPacket.Name == null || loginPacket.Password == null) return;

            var a = DependencyContainer.Instance.GetInstance<JsonGameConfiguration>().Server;

            var user = new UserDTO
            {
                Name = loginPacket.Name,
                Password = a.UseOldCrypto
                    ? CryptographyBase.Sha512(LoginCryptography.GetPassword(loginPacket.Password)).ToUpper()
                    : loginPacket.Password
            };
            if (user == null || user.Name == null || user.Password == null) return;
            var loadedAccount = DAOFactory.AccountDAO.LoadByName(user.Name);
            if (loadedAccount != null && loadedAccount.Name != user.Name)
            {
                _session.SendPacket($"failc {(byte) LoginFailType.WrongCaps}");
                return;
            }

            if (loadedAccount?.Password.ToUpper().Equals(user.Password) == true)
            {
                var ipAddress = _session.IpAddress;
                DAOFactory.AccountDAO.WriteGeneralLog(loadedAccount.AccountId, ipAddress, null,
                    GeneralLogType.Connection, "LoginServer");

                if (DAOFactory.PenaltyLogDAO.LoadByIp(ipAddress).Count() > 0)
                {
                    _session.SendPacket($"failc {(byte) LoginFailType.CantConnect}");
                    return;
                }

                //check if the account is connected
                if (!CommunicationServiceClient.Instance.IsAccountConnected(loadedAccount.AccountId))
                {
                    var cleanIp = ipAddress.Replace("tcp://", "");
                    cleanIp = cleanIp.Substring(0, cleanIp.LastIndexOf(":") > 0 ? cleanIp.LastIndexOf(":") : cleanIp.Length);

                    var type = loadedAccount.Authority;
                    var penalty = DAOFactory.PenaltyLogDAO.LoadByAccount(loadedAccount.AccountId)
                        .FirstOrDefault(s => s.DateEnd > DateTime.Now && s.Penalty == PenaltyType.Banned);

                    if(penalty == null)
                    {
                        penalty = DAOFactory.PenaltyLogDAO.LoadByIp(cleanIp)
                        .FirstOrDefault(s => s.DateEnd > DateTime.Now && s.Penalty == PenaltyType.IPBanned);
                    }

                    if (penalty != null)
                        _session.SendPacket($"failc {(byte) LoginFailType.Banned}");
                    else
                        switch (type)
                        {
                            case AuthorityType.Unconfirmed:
                            {
                                _session.SendPacket($"failc {(byte) LoginFailType.CantConnect}");
                            }
                                break;

                            case AuthorityType.Banned:
                            {
                                _session.SendPacket($"failc {(byte) LoginFailType.Banned}");
                            }
                                break;

                            case AuthorityType.Closed:
                            {
                                _session.SendPacket($"failc {(byte) LoginFailType.CantConnect}");
                            }
                                break;

                            default:
                            {
                                if (loadedAccount.Authority < AuthorityType.SMOD)
                                {
                                    var maintenanceLog = DAOFactory.MaintenanceLogDAO.LoadFirst();
                                    if (maintenanceLog != null && maintenanceLog.DateStart < DateTime.Now)
                                    {
                                        _session.SendPacket($"failc {(byte) LoginFailType.Maintenance}");
                                        return;
                                    }
                                }

                                var newSessionId = SessionFactory.Instance.GenerateSessionId();
                                Logger.Debug(string.Format(Language.Instance.GetMessageFromKey("CONNECTION"), user.Name,   newSessionId));
                                 
                                try
                                {
                                    ipAddress = ipAddress.Substring(6, ipAddress.LastIndexOf(':') - 6);
                                    CommunicationServiceClient.Instance.RegisterAccountLogin(loadedAccount.AccountId, newSessionId, ipAddress);

                                }
                                catch (Exception ex)
                                {
                                    Logger.Error("General Error SessionId: " + newSessionId, ex);
                                }

                                    var clientData = loginPacket.ClientData.Split('.');                                 
                                    if (clientData.Length < 2)
                                    {
                                        clientData = loginPacket.ClientDataOld.Split('.');
                                    }                                

                                    var ignoreUserName = short.TryParse(clientData[3], out var clientVersion) && (clientVersion < 3075 || a.UseOldCrypto);
                                    _session.SendPacket(
                                    _session.BuildServersPacket(user.Name, newSessionId, ignoreUserName));
                            }
                            break;
                        }
                }
                else
                {
                    _session.SendPacket($"failc {(byte) LoginFailType.AlreadyConnected}");
                }
            }
            else
            {
                _session.SendPacket($"failc {(byte) LoginFailType.AccountOrPasswordWrong}");
            }
        }

        #endregion
    }
}