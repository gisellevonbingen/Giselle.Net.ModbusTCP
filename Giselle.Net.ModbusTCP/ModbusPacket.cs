using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons.IO;

namespace Giselle.Net.ModbusTCP
{
    public class ModbusPacket
    {
        public ModbusHeader Header { get; set; }
        public ModbusFunction Function { get; set; }

        public static DataProcessor CreateProcessor(Stream stream)
        {
            return new DataProcessor(stream) { IsLittleEndian = false };
        }

        public ModbusPacket()
        {
            this.Header = new ModbusHeader();
            this.Function = null;
        }

        public void Read(Stream stream, bool isRequest)
        {
            var processor = CreateProcessor(stream);
            var header = this.Header = new ModbusHeader();
            header.TransactionIdentifier = processor.ReadUShort();
            header.ProtocolIdentifier = processor.ReadUShort();

            var readLength = processor.ReadUShort();
            header.UnitIdentifier = processor.ReadByte();

            var functionCode = processor.ReadByte();
            var functionBytes = processor.ReadBytes(readLength - 2);
            var function = this.Function = ModbusFunction.Create(functionCode, isRequest);
            function.ReadAsBytes(functionBytes);
        }

        public void Write(Stream stream)
        {
            var header = this.Header;
            var processor = CreateProcessor(stream);
            processor.WriteUShort(header.TransactionIdentifier);
            processor.WriteUShort(header.ProtocolIdentifier);

            var function = this.Function;
            var functionBytes = function.WriteAsBytes();
            processor.WriteUShort((ushort)(functionBytes.Length + 2));
            processor.WriteByte(header.UnitIdentifier);
            processor.WriteByte(function.GetFunctionCode());
            processor.Write(functionBytes, 0, functionBytes.Length);
        }

    }

}
