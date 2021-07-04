using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons.IO;

namespace Giselle.Net.ModbusTCP
{
    public class ModbusFunctionErrorResponse : ModbusFunction
    {
        public byte ErrorCode { get; set; }

        public ModbusFunctionErrorResponse()
        {

        }

        public override void Read(DataProcessor processor)
        {
            this.ErrorCode = processor.ReadByte();
        }

        public override void Write(DataProcessor processor)
        {
            processor.WriteByte(this.ErrorCode);
        }

    }

}
