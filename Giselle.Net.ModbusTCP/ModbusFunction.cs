using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons.IO;

namespace Giselle.Net.ModbusTCP
{
    public abstract class ModbusFunction
    {
        public const byte ErrorMask = 0x80;
        private static readonly Dictionary<byte, ModbusFunctionRegistration> FunctionCodeTypeMap = new Dictionary<byte, ModbusFunctionRegistration>();

        static ModbusFunction()
        {
            Register<ModbusFunction03ReadHoldingRegistersRequest, ModbusFunction03ReadHoldingRegistersResponse>(03);
            Register<ModbusFunction16WriteMultipleRegistersRequest, ModbusFunction16WriteMultipleRegistersResponse>(16);
        }

        public static ModbusFunctionRegistration Register<TQ, TS>(byte code)
            where TQ : ModbusFunction
            where TS : ModbusFunction
        {
            var registraion = new ModbusFunctionRegistration(code, typeof(TQ), typeof(TS));
            FunctionCodeTypeMap[code] = registraion;
            return registraion;
        }

        public static ModbusFunctionRegistration Find(byte code)
        {
            return FunctionCodeTypeMap.TryGetValue(code, out var registration) ? registration : null;
        }

        public static ModbusFunctionRegistration Find(Type type)
        {
            foreach (var pair in FunctionCodeTypeMap)
            {
                var registration = pair.Value;

                if (registration.RequestType == type)
                {
                    return registration;
                }

                if (registration.ResponseType == type)
                {
                    return registration;
                }

            }

            return null;
        }

        public static ModbusFunction Create(byte functionCode, bool isRequest)
        {
            if ((functionCode & ErrorMask) == ErrorMask)
            {
                return new ModbusFunctionErrorResponse();
            }

            if (FunctionCodeTypeMap.ContainsKey(functionCode) == false)
            {
                throw new ArgumentException(string.Format("FunctionCode{0} is not exist", functionCode));
            }

            var registration = FunctionCodeTypeMap[functionCode];
            var type = isRequest ? registration.RequestType : registration.ResponseType;

            return Activator.CreateInstance(type) as ModbusFunction;
        }

        public abstract void Read(DataProcessor processor);

        public abstract void Write(DataProcessor processor);
    }

}
