using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Net.ModbusTCP
{
    public class ModbusFunctionRegistration
    {
        public byte Code { get; private set; }
        public Type RequestType { get; private set; }
        public Type ResponseType { get; private set; }

        public ModbusFunctionRegistration(byte code, Type requestType, Type responseType)
        {
            this.Code = code;
            this.RequestType = requestType;
            this.ResponseType = responseType;
        }

    }

}
