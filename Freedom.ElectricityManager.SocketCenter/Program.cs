﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Freedom.ElectricityManager.SocketCenter.Models;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Freedom.ElectricityManager.SocketCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Press any key to start the server!");

            //Console.ReadKey();
            //Console.WriteLine();

            Console.Title = "武穴电力数据管理中心";

            var appServer = new AppServer(new DefaultReceiveFilterFactory<MyReceiveFilter, StringRequestInfo>());

            appServer.NewSessionConnected += session =>
            {
                session.Send("Welcome to WuXue Electricity Manager Server！");
            };

            appServer.NewRequestReceived += (session, requestInfo) =>
            {
                switch (requestInfo.Key.ToUpper())
                {
                    case ("ECHO"):
                        session.Send(requestInfo.Body);
                        break;

                    case ("ADD"):
                        session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
                        break;

                    case ("MULT"):

                        var result = 1;

                        foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
                        {
                            result *= factor;
                        }

                        session.Send(result.ToString());
                        break;
                    case "RECEVIE":
                        try
                        {
                            using (var httpClient = new HttpClient())
                            {
                                var addPacketTaskInput = new AddPacketTaskInput
                                {
                                    PacketData = requestInfo.Body,
                                    AreaOne = requestInfo.Parameters[0],
                                    AreaTwo = requestInfo.Parameters[1],
                                    DataContent = requestInfo.Parameters[2],
                                    DeviceCode = requestInfo.Parameters[3]
                                };
                                httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseUrl"]);
                                var response = httpClient.PostAsync("/api/services/app/PacketTask/AddPacketTask",
                                    new StringContent(JsonConvert.SerializeObject(addPacketTaskInput), Encoding.UTF8, "application/json")).Result;
                                if (response.IsSuccessStatusCode)
                                {
                                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}:{addPacketTaskInput.PacketData} 上传成功。");
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            session.Send($"areaCode：{requestInfo.Parameters[0]} detailAreaCode:{requestInfo.Parameters[1]} equipmentCode:{requestInfo.Parameters[2]} commandContent:{requestInfo.Parameters[3]}");
                        }

                        break;
                }
            };

            //Setup the appServer
            if (!appServer.Setup(ConfigurationManager.AppSettings["SocketPort"].ToInt32OrDefault(2001))) //Setup with listening port
            {
                Console.WriteLine("启动失败!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("启动失败!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("服务启动成功，按“q”退出!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("服务已经停止!");
            Console.ReadKey();
        }
    }
}
