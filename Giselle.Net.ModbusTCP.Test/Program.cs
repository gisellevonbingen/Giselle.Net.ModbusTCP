using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons.Users;

namespace Giselle.Net.ModbusTCP.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var client = new ModbusSimpleClient())
            {
                var user = new UserConsole();
                client.Hostname = "192.168.1.100";
                client.Opened += (sender, e) => user.SendMessage("Opened");
                client.Closed += (sender, e) => user.SendMessage("Closed");
                client.Start();

                var read = "read";
                var write = "write";
                var inputs = new[] { read, write };

                try
                {
                    while (true)
                    {
                        var input = user.QueryInput("Enter", inputs, t => t, false).Value;

                        if (input == read)
                        {
                            var packet = new ModbusPacket();
                            packet.Header.UnitIdentifier = (byte)(user.ReadInput("unit id as int").AsInt ?? 0);
                            packet.Function = new ModbusFunction03ReadHoldingRegistersRequest()
                            {
                                StartingAddress = (ushort)(user.ReadInput("address as int").AsInt ?? 0),
                                QuantityOfRegisters = (ushort)(user.ReadInput("registers as int").AsInt ?? 0)
                            };

                            var _response = client.Enqueue(packet).Wait().Function;

                            if (_response is ModbusFunction03ReadHoldingRegistersResponse response)
                            {
                                user.SendMessage($"bytes : {BitConverter.ToString(response.Values)}");
                            }
                            else if (_response is ModbusFunctionErrorResponse error)
                            {
                                user.SendMessage($"error : {error.ErrorCode}");
                            }
                            else
                            {
                                user.SendMessage($"unknown response : {_response}");
                            }

                        }
                        else if (input == write)
                        {
                            var packet = new ModbusPacket();
                            packet.Header.UnitIdentifier = (byte)(user.ReadInput("unit id as int").AsInt ?? 0);
                            packet.Function = new ModbusFunction16WriteMultipleRegistersRequest()
                            {
                                StartingAddress = (ushort)(user.ReadInput("address as int").AsInt ?? 0),
                                QuantityOfRegisters = (ushort)(user.ReadInput("registers as int").AsInt ?? 0),
                                Values = user.ReadInputWhileBreak("bytes as byte until break", (u, i) => (byte)(i.AsInt ?? 0)).ToArray()
                            };

                            var _response = client.Enqueue(packet).Wait().Function;

                            if (_response is ModbusFunction16WriteMultipleRegistersResponse response)
                            {
                                user.SendMessage($"startingAddress : {response.StartingAddress}");
                                user.SendMessage($"quantityOfRegisters : {response.QuantityOfRegisters}");
                            }
                            else if (_response is ModbusFunctionErrorResponse error)
                            {
                                user.SendMessage($"error : {error.ErrorCode}");
                            }
                            else
                            {
                                user.SendMessage($"unknown response : {_response}");
                            }

                        }

                    }

                }
                catch (UserInputReturnException)
                {

                }

            }

        }

    }

}
