using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons.IO;

namespace Giselle.Net.ModbusTCP
{
    public class ModbusFunction03ReadHoldingRegistersRequest : ModbusFunction
    {
        public ushort StartingAddress { get; set; }
        public ushort QuantityOfRegisters { get; set; }

        public ModbusFunction03ReadHoldingRegistersRequest()
        {

        }

        public override void Read(DataProcessor processor)
        {
            this.StartingAddress = processor.ReadUShort();
            this.QuantityOfRegisters = processor.ReadUShort();
        }

        public override void Write(DataProcessor processor)
        {
            processor.WriteUShort(this.StartingAddress);
            processor.WriteUShort(this.QuantityOfRegisters);
        }

    }

    public class ModbusFunction03ReadHoldingRegistersResponse : ModbusFunction
    {
        public byte[] Values { get; set; }

        public ModbusFunction03ReadHoldingRegistersResponse()
        {

        }

        public override void Read(DataProcessor processor)
        {
            var byteLength = processor.ReadByte();
            var values = new byte[byteLength];

            for (var i = 0; i < values.Length; i++)
            {
                values[i] = processor.ReadByte();
            }

            this.Values = values;
        }

        public override void Write(DataProcessor processor)
        {
            var values = this.Values;
            processor.WriteByte((byte)values.Length);

            for (var i = 0; i < values.Length; i++)
            {
                processor.WriteByte(values[i]);
            }

        }

    }

}
