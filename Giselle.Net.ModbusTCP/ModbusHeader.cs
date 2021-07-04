using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Net.ModbusTCP
{
    public class ModbusHeader
    {
        public ushort TransactionIdentifier { get; set; }
        public ushort ProtocolIdentifier { get; set; }
        public byte UnitIdentifier { get; set; }

        public ModbusHeader()
        {

        }

        public ModbusHeader(ushort transactionIdentifier, ushort protocolIdentifier, byte unitIdentifier)
        {
            this.TransactionIdentifier = transactionIdentifier;
            this.ProtocolIdentifier = protocolIdentifier;
            this.UnitIdentifier = unitIdentifier;
        }

    }

}
