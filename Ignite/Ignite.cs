using BlowFishCS;
using ENet;
using Flash.Riot.platform.game;
using Ignite.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;

namespace Ignite
{
    public class IgniteClient
    {
        private Host _enetHost;
        private Peer _peer;
        private BlowFish _blowfish;
        private bool _connected = false;
        private bool _gameStarted = false;
        private bool _isOdin = false;
        private bool _isOrder = false;
        private bool _toCenter = false;
        private string _myPlayer;
        private UInt32 _myNetId = 0;
        private UInt64 _mySummonerId;
        private DateTime _gameStartTime;
        private Timer _movementTimer;
        private PlayerCredentialsDto _credentials;

        // I mean it's C#, sigh.
        private bool _move;
        private List<uint> _capturePoints;
        private DateTime _lastCapTime;
        private int _captures;

        public bool Connected
        {
            get { return _connected; }
        }
        private delegate void OnPacket(byte[] packet, Channel channel);
        private Dictionary<PacketCommand, OnPacket> _callbacks;

        public IgniteClient(PlayerCredentialsDto credentials)
        {
            _credentials = credentials;
            _callbacks = new Dictionary<PacketCommand, OnPacket>()
            {
                {PacketCommand.Batch, OnBatchPacket},
                {PacketCommand.KeyCheck, OnKeyCheck},
                {PacketCommand.World_SendGameNumber, OnWorldSendGameNumber},
                {PacketCommand.S2C_QueryStatusAns, OnQueryStatusAns},
                {PacketCommand.S2C_SynchVersion, OnSyncVersionAns},
                {PacketCommand.S2C_HeroSpawn, OnHeroSpawn},
                {PacketCommand.S2C_MinionSpawn, OnMinionSpawn},
                {PacketCommand.S2C_RemoveVisionBuff, OnRemoveVisionBuff}
        //        {PacketCommand.S2C_EndGame, OnExitGame}
            };
            _capturePoints = new List<uint>();
        }
        public async void Start()
        {
            var ip = _credentials.serverIp;
            var port = Convert.ToUInt16(_credentials.serverPort);
            var blowfishKey = Convert.FromBase64String(_credentials.encryptionKey);
            _mySummonerId = Convert.ToUInt64(_credentials.summonerId);

            _blowfish = new BlowFish(blowfishKey);

            // Initialize ENetCS
            Library.Initialize();

            // Initialize Log Class.
            Log.Initialize();

            _enetHost = new Host();
            _enetHost.Create(null, 1);

            var address = new Address();
            address.SetHost(ip);
            address.Port = port;

            _peer = _enetHost.Connect(address, 8);

            _movementTimer = new Timer();
            _movementTimer.Interval = TimeSpan.FromSeconds(6).TotalMilliseconds;
            _movementTimer.Elapsed += (s, e) =>
            {
                if (_gameStarted && _myNetId != 0)
                {
                    if (!_isOdin)
                    {
  //                      Log.Write("[{0}] Attempting to move down mid.", _myPlayer);
                        Send(Deserialize<MovementRequest>(MovementRequest.CreateCoop(_myNetId)));
                    }
                    else
                    {
                        if (_isOrder && _captures == 0 && (DateTime.Now - _gameStartTime).TotalSeconds >= 90)
                        {
                            Log.Write("[{0}] Capturing point A", _myPlayer);
                            Send(Deserialize<CapturePoint>(CapturePoint.Create(_myNetId, _capturePoints[0])));
                        }
                        else if (_isOrder && _captures == 8)
                        {
                            if ((DateTime.Now - _gameStartTime).TotalMilliseconds >= 170000)
                            {
                                Send(Deserialize<CapturePoint>(CapturePoint.Create(_myNetId, _capturePoints[4])));
                            }
                            else
                            {
                                Send(Deserialize<MovementRequest>(MovementRequest.CreateOdin(_myNetId, _move)));
                                _move = !_move;
                            }
                        }
                        else if (!_isOrder)
                        {
                            Send(Deserialize<MovementRequest>(MovementRequest.CreateOdin(_myNetId, _move)));
                            _move = !_move;
                        }
                    }

                    if ((DateTime.Now - _gameStartTime).TotalMinutes >= 20)
                    {
//                        Log.Write("[{0}] Attempting to surrender.", _myPlayer);
                        Send(Deserialize<Surrender>(Surrender.Create(_myNetId, 1)));
                    }
                }
            };

            _movementTimer.Start();
            while (_enetHost.Service(1) >= 0)
            {
                Event enetEvent;
                try
                {
                    while (_enetHost.CheckEvents(out enetEvent) > 0)
                    {
                        switch (enetEvent.Type)
                        {
                            case EventType.Connect:
                                OnConnect(blowfishKey);
                                break;
                            case EventType.Receive:
                                OnRecieve(enetEvent);
                                break;
                            case EventType.Disconnect:
                                break;
                        }
                    }
                }catch (InvalidOperationException)
                {
                    break;
                }

            }
        }

        public void Exit()
        {
            try
            {
                Log.Write("[{0}] Exiting game.", _myPlayer);
                _movementTimer.Stop();
                _enetHost.Dispose();
            }
            catch (Exception e)
            {
            }
        }

        private void OnConnect(byte[] blowfishKey)
        {
            Log.Write("[{0}] Connected to server.", _credentials.summonerName);
            var keyCheck = KeyCheck.Create(_mySummonerId, _blowfish.Encrypt_ECB(_mySummonerId), blowfishKey);
            var keyBytes = Deserialize<KeyCheck>(keyCheck);
            Log.PacketLog(keyBytes, 0, keyBytes.Length);
            Send(keyBytes, Channel.Handshake);
        }

        private void OnRecieve(Event enetEvent)
        {
            var packet = enetEvent.Packet.GetBytes();
            var channel = (Channel)enetEvent.ChannelID;

            if (packet.Length >= 8 && channel != Channel.Handshake)
                packet = _blowfish.Decrypt_ECB(packet);

            if (packet.Length < 1)
                return;

            var cmd = (PacketCommand)BitConverter.ToUInt16(packet, 0);
            Log.Write("Got Cmd: {0}", cmd);
            Log.PacketLog(packet, 0, packet.Length);
            try
            {
                if (_callbacks.ContainsKey(cmd))
                    _callbacks[cmd](packet, channel);
            }
            catch { }
            // done!
            enetEvent.Packet.Dispose();
        }

        private void OnBatchPacket(byte[] packet, Channel channel)
        {
            var packetOffset = 3;
            var totalPackets = packet[1];
            var totalSize = packet[2];

            var previousCommand = packet[packetOffset];
            if (_callbacks.ContainsKey((PacketCommand)previousCommand))
            {
                var prevPacket = new byte[totalSize];
                Buffer.BlockCopy(packet, packetOffset, prevPacket, 0, totalSize);
                _callbacks[(PacketCommand)previousCommand](prevPacket, channel);
            }

            packetOffset += totalSize;

            for (var i = 2; i < totalPackets+1; ++i)
            {
                byte command;
                var flagsAndLen = packet[packetOffset];
                var size = (byte)(flagsAndLen >> 2);
                var additionalByte = packet[packetOffset + 1];
                var buffer = new byte[8192];

                packetOffset++;

                if ((flagsAndLen & 1) > 0)
                {
                    packetOffset++;
                    command = previousCommand;
                }
                else
                {
                    command = packet[packetOffset];
                    packetOffset++;

                    if ((flagsAndLen & 2) > 0)
                        packetOffset++;
                    else
                        packetOffset += 4;
                }

                if (size == 0x3F)
                {
                    size = packet[packetOffset];
                    packetOffset++;
                }

                Array.Resize<byte>(ref buffer, size+1);
                buffer[0] = command;
                Buffer.BlockCopy(packet, packetOffset-4, buffer, 1, size);

                if (_callbacks.ContainsKey((PacketCommand)command))
                    _callbacks[(PacketCommand)command](buffer, channel);

                packetOffset += size;
                previousCommand = command;
            }
        }
        private void OnKeyCheck(byte[] packet, Channel channel) 
        {
            if (channel != Channel.Handshake)
                return;

            var keyCheck = Serialize<KeyCheck>(packet);
            var checkBytes = BitConverter.GetBytes(keyCheck.checkId);

            Log.Write("UserId for client: ({0}). Player Id: {1}", Convert.ToUInt64(_blowfish.Decrypt_ECB(checkBytes)), keyCheck.playerNo);
        }

        private void OnWorldSendGameNumber(byte[] packet, Channel channel)
        {
            var game = Serialize<WorldSendGameNumber>(packet);

            _myPlayer = game.Name;
            Send(Deserialize<PacketHeader>(PacketHeader.Create(PacketCommand.C2S_QueryStatusReq, 0)));
        }

        private void OnQueryStatusAns(byte[] packet, Channel channel) 
        {
            var status = Serialize<QueryStatusAns>(packet);

            if (status.Ok == 1)
            {
                //Log.Write("Sendiong Version information.");
                var version = SynchVersion.Create();
                Send(Deserialize<SynchVersion>(version));
            }
        }

        private void OnSyncVersionAns(byte[] packet, Channel channel) 
        {
            if (!_gameStarted)
            {
                var answer = Serialize<SynchVersionAns>(packet);

                //Log.Write("Setting up for game mode: {0}", answer.GameMode);

                _isOdin = answer.GameMode.Equals("ODIN");
                
                // yay linq!
                var player = answer.players.ToList().Find(p => p.userId == _mySummonerId);
                _isOrder = player.teamId == 0x64;

                Send(Deserialize<PacketHeader>(PacketHeader.Create(PacketCommand.C2S_ClientReady, 0)), Channel.LoadingScreen);

                var ping = PingLoadInfo.Create(100, 2, _mySummonerId);
                Send(Deserialize<PingLoadInfo>(ping));
                Send(Deserialize<PacketHeader>(PacketHeader.Create(PacketCommand.C2S_CharLoaded, 0)));
                Send(Deserialize<StartGame>(StartGame.Create()));
            }
        }

        private void OnHeroSpawn(byte[] packet, Channel channel)
        {
            if (!_connected)
                _connected = true;
             
            var hero = Serialize<HeroSpawn>(packet);
            if (hero.Name == _myPlayer && _myNetId == 0)
            {
                _gameStarted = true;
                _gameStartTime = DateTime.Now;
                Log.Write("Hero:({0}) Player:({1}) Team:({2}) NetId:({3:X})", hero.Hero, hero.Name, _isOrder ? "ORDER" : "CHAOS", hero.NetId);
                _myNetId = hero.NetId;

                var purchase = BuyItem.Create(_myNetId, 0x3E9); // boots
                Send(Deserialize<BuyItem>(purchase));

                purchase.ItemId = 0x7D3; // hp pot
                for(var i = 0; i < 4; ++i)
                    Send(Deserialize<BuyItem>(purchase));

                purchase.ItemId = 0xD0C; // trinket
                Send(Deserialize<BuyItem>(purchase));
            }
        }

        private void OnGainVision(byte[] packet, Channel channel)
        {
            if (!_isOdin)
                Send(Deserialize<MovementRequest>(MovementRequest.CreateCoop(_myNetId)));
        }

        private void OnMinionSpawn(byte[] packet, Channel channel)
        {
            var minion = Serialize<MinionSpawn>(packet);

            if (minion.Name == "OdinNeutralGuardian")
            {
        //        Log.Write("[{0}] Located capture point at netId: {1:X}", _myPlayer, minion.NetId);
                _capturePoints.Add(minion.NetId);
            }
        }

        private void OnRemoveVisionBuff(byte[] packet, Channel channel)
        {
            if (!_isOdin || !_isOrder)
                return;

            switch(_captures)
            {
                case 0:
                case 1:
                    Log.Write("[{0}] Capturing point B", _myPlayer);
                    Send(Deserialize<CapturePoint>(CapturePoint.Create(_myNetId, _capturePoints[1])));
                    break;
                case 3:
                    Log.Write("[{0}] Capturing point C", _myPlayer);
                    Send(Deserialize<CapturePoint>(CapturePoint.Create(_myNetId, _capturePoints[2])));
                    break;
                case 5:
                    Log.Write("[{0}] Capturing point D", _myPlayer);
                    _gameStartTime = DateTime.Now;
                    Send(Deserialize<CapturePoint>(CapturePoint.Create(_myNetId, _capturePoints[3])));
                    break;
            }

            _captures++;

        }

        private void OnExitGame(byte[] packet, Channel channel)
        {
            Log.Write("[{0}] Exiting game.", _myPlayer);
            _movementTimer.Stop();
            _enetHost.Dispose();
        }

        
        private void Send(byte[] packet, Channel channel = Channel.C2S)
        {
            if (packet.Length >= 8 && channel != Channel.Handshake)
                packet = _blowfish.Encrypt_ECB(packet);

            _peer.Send((byte)channel, packet);
        }

        private byte[] Deserialize<T>(T packet) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var data = new byte[size];

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(packet, ptr, true);
            Marshal.Copy(ptr, data, 0, size);
            Marshal.FreeHGlobal(ptr);

            return data;
        }

        private T Serialize<T>(byte[] packet) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(packet, 0, ptr, size);
            var data = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            return data;
        }
    }
}
