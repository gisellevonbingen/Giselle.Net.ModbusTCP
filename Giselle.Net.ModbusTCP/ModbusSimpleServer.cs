using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons.Net;

namespace Giselle.Net.ModbusTCP
{
    public class ModbusSimpleServer : SimpleServer<ModbusPacket>
    {
        public ModbusSimpleServer()
            : base(ProcessMode.Exchange)
        {
            this.Accepting += this.OnAccepting;
        }

        protected virtual SimpleRemoteClient<ModbusPacket> OnAccepting(object sender, ProcessMode mode, TcpClient tcpClient)
        {
            var client = new SimpleRemoteClient<ModbusPacket>(mode);
            client.Closed += this.OnClientClosed;
            client.PacketDeserialize += this.OnPacketDeserialize;
            client.PacketSerialize += this.OnPacketSerialize;

            return client;
        }

        protected virtual void OnClientClosed(object sender, EventArgs e)
        {
            var client = sender as SimpleRemoteClient<ModbusPacket>;
            client.Closed -= this.OnClientClosed;
            client.PacketDeserialize -= this.OnPacketDeserialize;
            client.PacketSerialize -= this.OnPacketSerialize;
        }

        protected virtual ModbusPacket OnPacketDeserialize(object sender, Stream stream)
        {
            var request = new ModbusPacket();
            request.Read(stream, true);
            return request;
        }

        protected virtual void OnPacketSerialize(object sender, Stream stream, ModbusPacket packet)
        {
            packet.Write(stream);
        }

    }

}
