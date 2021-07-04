using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons.IO;

namespace Giselle.Net.ModbusTCP
{
    public class ModbusFunction16WriteMultipleRegistersRequest : ModbusFunction
    {
        public ushort StartingAddress { get; set; }
        public ushort QuantityOfRegisters { get; set; }
        public byte[] Values { get; set; }

        public ModbusFunction16WriteMultipleRegistersRequest()
        {

        }

        public override void Read(DataProcessor processor)
        {
            this.StartingAddress = processor.ReadUShort();
            this.QuantityOfRegisters = processor.ReadUShort();
            var byteLength = processor.ReadByte();

            var values = new byte[byteLength];
            this.Values = values;

            for (var i = 0; i < values.Length; i++)
            {
                values[i] = processor.ReadByte();
            }

        }

        public override void Write(DataProcessor processor)
        {
            processor.WriteUShort(this.StartingAddress);
            processor.WriteUShort(this.QuantityOfRegisters);

            var values = this.Values;
            processor.WriteByte((byte)values.Length);

            for (var i = 0; i < values.Length; i++)
            {
                processor.WriteByte(values[i]);
            }

        }

    }

    public class ModbusFunction16WriteMultipleRegistersResponse : ModbusFunction
    {
        public ushort StartingAddress { get; set; }
        public ushort QuantityOfRegisters { get; set; }

        public ModbusFunction16WriteMultipleRegistersResponse()
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

}
