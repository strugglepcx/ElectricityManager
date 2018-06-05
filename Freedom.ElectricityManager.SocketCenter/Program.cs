using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Freedom.ElectricityManager.SocketCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();

            var appServer = new AppServer(new DefaultReceiveFilterFactory<MyReceiveFilter, StringRequestInfo>());

            appServer.NewSessionConnected += session =>
            {
                session.Send("Welcome to SuperSocket Telnet Server");
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
                        session.Send($"areaCode:{requestInfo.Parameters[0]} detailAreaCode:{requestInfo.Parameters[1]} equipmentCode:{requestInfo.Parameters[2]} commandContent:{requestInfo.Parameters[3]}");
                        break;
                }
            };

            //Setup the appServer
            if (!appServer.Setup(2012)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }
    }
}
