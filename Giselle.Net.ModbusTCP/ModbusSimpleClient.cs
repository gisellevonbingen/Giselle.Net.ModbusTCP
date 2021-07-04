using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons;
using Giselle.Commons.Net;

namespace Giselle.Net.ModbusTCP
{
    public class ModbusSimpleClient : SimpleLocalClient<ModbusPacket>
    {
        public ModbusSimpleClient()
            : base(ProcessMode.Exchange)
        {
            this.Hostname = string.Empty;
            this.Port = 502;

            this.PacketDeserialize += this.OnPacketDeserialize;
            this.PacketSerialize += this.OnPacketSerialize;
        }

        public override ModbusPacket ReadPacket(Stream stream)
        {
            return base.ReadPacket(stream);
        }

        protected virtual ModbusPacket OnPacketDeserialize(object sender, Stream stream)
        {
            var packet = new ModbusPacket();
            packet.Read(stream, false);
            return packet;
        }

        protected virtual void OnPacketSerialize(object sender, Stream stream, ModbusPacket packet)
        {
            packet.Write(stream);
        }

    }

}
