using System;
using System.Collections.Generic;
using PuassonGeneration;
using System.Linq;

namespace CompMod
{
    class Program
    {
        static Random r = new Random(0);
        const string new_client = "пришел клиент";
        const string client_leave = "клиент ушел";

        static List<ServiceState> GenerateStates(double maxT, double lambda, lambda_f lambda_t)
        {
            List<ServiceState> states = new List<ServiceState>();

            ServiceState now = new ServiceState("Init", 3);
            now.clinetInSystem = 0;

            var r = new Random(DateTime.Now.Millisecond);
            double t = 0;
            nextClient = GenerateNextClientCome(t, lambda, lambda_t);
            while (t < maxT || now.clinetInSystem > 0)
            {

                double min = devices.Select(device => device.timeFree).Min();

                if (t < maxT)
                {
                    min = Math.Min(min, nextClient);
                }

                t = min;
                if (min == nextClient)
                {

                    clients.Add(new Client(t));
                    int index = clients.Count;
                    nextClient = GenerateNextClientCome(t, lambda, lambda_t);

                    int device = GetDeviceForClent(now);

                    now = new ServiceState(now, new_client, t);
                    now.clinetInSystem += 1;


                    if (device != -1)
                    {
                        double y = GenerateY(lambda);
                        clients[index - 1].timeService = y;
                        devices[device].timeFree = t + y;
                        devices[device].clients.Add(clients[index - 1]);
                        now.clientInDevice[device] = index;
                    }


                }
                else
                {
                    Device nowStop = devices.Where(device => (device.timeFree == min)).First();
                    int deveci_index = devices.IndexOf(nowStop);
                    Client freeClient = clients[now.clientInDevice[deveci_index] - 1];

                    freeClient.timeLeave = t;



                    now = new ServiceState(now, client_leave, t);
                    now.clinetInSystem--;
                    if (now.clinetInSystem >= 3)
                    {
                        int nextClient = now.clientInDevice.Max() + 1;
                        now.clientInDevice[devices.IndexOf(nowStop)] = nextClient;

                        double y = GenerateY(lambda);
                        nowStop.timeFree = t + y;

                        clients[nextClient - 1].timeService = y;
                        clients[nextClient - 1].timeWait = t - clients[nextClient - 1].timeCome;
                    }
                    else
                    {
                        nowStop.timeFree = double.MaxValue;

                        now.clientInDevice[devices.IndexOf(nowStop)] = ServiceState.NoClientValue;
                    }

                }
                states.Add(now);
            }

            return states;
        }

        public static double GenerateY(double lambda)
        {
            var U = r.NextDouble();
            return -1 / (lambda * Math.Log(U, 2));
        }

        static List<Device> devices;
        static double nextClient;

        static List<Client> clients;
        static double GenerateNextClientCome(double t_now, double lambda, lambda_f lambda_t)
        {
            double t = t_now;
            while (true)
            {

                var U1 = r.NextDouble();
                t -= 1 / lambda * Math.Log(U1, 2);

                Random r2 = new Random();
                var U2 = r2.NextDouble();
                if (U2 < lambda_t(t) / lambda)
                    return t;
            }

        }




        static int GetDeviceForClent(ServiceState now)
        {
            for (int i = 0; i < now.clientInDevice.Length; i++)
            {
                if (now.clientInDevice[i] == ServiceState.NoClientValue)
                {
                    return i;
                }
            }
            return -1;
        }


        static double ClientsMidTimeInSystem(List<Client> clients, double maxT)
        {
            double sum = clients.Select(client => client.GetTimeIsSystem(maxT)).Sum();
            return sum / clients.Count;
        }

        static double ClientsMidWaitTime(List<Client> clients)
        {
            double sum = clients.Select(client => client.timeWait).Sum();
            return sum / clients.Count;
        }

        static double ClientsInQueue(List<ServiceState> states)
        {
            double sum = 0;
            for (int i = 0; i < states.Count - 1; i++)
            {
                sum += states[i].ClientsInQueue * (states[i + 1].time - states[i].time);
            }
            return sum;
        }


        static double WorkCoefficient(Device device, double T)
        {
            return device.GetWorkTime(T) / T;
        }

        static void Main(string[] args)
        {
            devices = new List<Device>
            {
                new Device(),
                new Device(),
                new Device()
            };
            Console.Write("T=");
            double T = double.Parse(Console.ReadLine());
            double lambda = 1.0;
            lambda_f lambda_func = (x) => x > T / 2 ? 0.5 : 1;


            clients = new List<Client>();

            List<ServiceState> states = GenerateStates(T, lambda, lambda_func);


            foreach (var state in states)
            {
                Console.WriteLine(state.ToString());
            }
            Console.WriteLine();
            Console.WriteLine();

            foreach (var client in clients)
            {
                Console.WriteLine(client.ToString());
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Клиентов в системе {clients.Count}");
            double midTime = ClientsMidTimeInSystem(clients, T);
            Console.WriteLine($"Среднее ремя клиента в системе {midTime:f2}");
            double clientsInQueue = ClientsMidWaitTime(clients);
            Console.WriteLine($"Среднее ремя клиента в очереди {clientsInQueue:f2}");

            for (int i = 0; i < devices.Count; i++)
            {
                double coeff = WorkCoefficient(devices[i], T);
                Console.WriteLine($"Занятость устройства {i} = {coeff:f2}");
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

        }
    }
}
