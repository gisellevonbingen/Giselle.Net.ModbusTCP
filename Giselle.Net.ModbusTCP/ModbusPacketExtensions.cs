using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Net.ModbusTCP
{
    public static class ModbusPacketExtensions
    {
        public static byte[] WriteAsBytes(this ModbusPacket packet)
        {
            using (var stream = new MemoryStream())
            {
                packet.Write(stream);
                return stream.ToArray();
            }

        }

        public static long ReadAsBytes(this ModbusPacket packet, byte[] bytes, bool isRequest)
        {
            using (var stream = new MemoryStream(bytes))
            {
                packet.Read(stream, isRequest);
                return stream.Position;
            }

        }

        public static byte[] WriteAsBytes(this ModbusFunction function)
        {
            using (var stream = new MemoryStream())
            {
                var processor = ModbusPacket.CreateProcessor(stream);
                function.Write(processor);
                return stream.ToArray();
            }

        }

        public static long ReadAsBytes(this ModbusFunction function, byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var processor = ModbusPacket.CreateProcessor(stream);
                function.Read(processor);
                return stream.Position;
            }

        }

        public static byte GetFunctionCode(this ModbusFunction function)
        {
            return ModbusFunction.Find(function.GetType()).Code;
        }

    }

}
