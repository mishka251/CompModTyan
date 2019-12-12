using System;
using System.Collections.Generic;
using PuassonGeneration;
using System.Linq;

namespace CompMod
{
    class Program
    {
        //class Client
        //{
        //    public double timeCome;
        //    public double timeWait;
        //    public double timeService;

        //    public double timeLeave;

        //    public Client()
        //    {

        //    }
        //    public Client(double timeCome)
        //    {
        //        this.timeCome = timeCome;
        //    }

        //    public override string ToString()
        //    {
        //        return $"Client come at = {timeCome} leave at {timeLeave} wait={timeWait} servcie={timeService}";
        //    }
        //}

        ////struct ServiceEvent
        ////{
        ////    public string name;
        ////    public double time;
        ////    public ServiceEvent(string n, double t)
        ////    {
        ////        this.name = n;
        ////        this.time = t;
        ////    }
        ////}

        //class Device
        //{
        //    public int clientIndex;
        //    public double timeFree;
        //    public int usedClient;
        //    public Device()
        //    {
        //        this.clientIndex = -1;
        //        this.timeFree = double.MaxValue;
        //        usedClient = 0;
        //    }
        //    public override string ToString()
        //    {
        //        return $"Device usedClients = {usedClient}";
        //    }

        //}
        //class ServiceState
        //{
        //    public int clinetInSystem;
        //    public int[] clientInDevice;
        //    public double time;
        //    public string type;

        //    public ServiceState(string type)
        //    {
        //        this.type = type;
        //    }

        //    public ServiceState(double time)
        //    {
        //        this.time = time;
        //    }
        //    public ServiceState(ServiceState s)
        //    {
        //        this.clinetInSystem = s.clinetInSystem;
        //        var arr = new int[] { 1, 2 };

        //        this.clientInDevice = new int[s.clientInDevice.Length];
        //        for (int i = 0; i < s.clientInDevice.Length; i++)
        //        {
        //            this.clientInDevice[i] = s.clientInDevice[i];
        //        }
        //    }

        //    public ServiceState(ServiceState s, string type, double time)
        //    {
        //        this.type = type;
        //        this.clinetInSystem = s.clinetInSystem;
        //        var arr = new int[] { 1, 2 };

        //        this.clientInDevice = new int[s.clientInDevice.Length];
        //        for (int i = 0; i < s.clientInDevice.Length; i++)
        //        {
        //            this.clientInDevice[i] = s.clientInDevice[i];
        //        }
        //        this.time = time;
        //    }

        //    public override string ToString()
        //    {
        //        string res = $"State {type} at {time}: clients in system: {clinetInSystem} ";
        //        for (int i = 0; i < clientInDevice.Length; i++)
        //        {
        //            res += $" in device{i + 1} = {clientInDevice[i]}";
        //        }
        //        return res;
        //    }
        //}

        static Random r = new Random(DateTime.Now.Millisecond);

        static List<ServiceState> GenerateStates(double maxT, double lambda, lambda_f lambda_t)
        {
            List<ServiceState> states = new List<ServiceState>();

            ServiceState now = new ServiceState("Init");
            now.clientInDevice = new int[] { 0, 0, 0 };
            now.clinetInSystem = 0;

            //ServiceState prev = now;
            var r = new Random(DateTime.Now.Millisecond);
            double t = 0;
            //List<double> S = new List<double>();
            nextClient = GenerateNextClientCome(t, lambda, lambda_t);
            while (t < maxT || now.clinetInSystem > 0)
            {

                double min = devices.Select(device => device.timeFree).Min();

                if (t < maxT)
                {
                    min = Math.Min(min, nextClient);
                }


                if (t < maxT && min == nextClient)
                {
                    t = nextClient;
                    clients.Add(new Client(t)); //A.Add(t);
                    int index = clients.Count;
                    nextClient = GenerateNextClientCome(t, lambda, lambda_t);
                    //states.Add(now);
                    int device = GetDeviceForClent();

                    now = new ServiceState(now, "new client", t);
                    now.clinetInSystem += 1;
                    //(now, device) = NewClient(now, index);

                    if (device != -1)
                    {
                        double y = GenerateY(lambda);
                        clients[index - 1].timeService = y;
                        devices[device].timeFree = t + y;
                        devices[device].clientIndex = index;
                        now.clientInDevice[device] = index;
                    }


                }
                else
                {
                    Device nowStop = devices.Where(device => (device.timeFree == min)).First();
                    Client freeClient = clients[nowStop.clientIndex - 1];
                    t = nowStop.timeFree;
                    freeClient.timeLeave = t;


                    nowStop.usedClient++;


                    now = new ServiceState(now, "client leave", t);
                    now.clinetInSystem--;
                    if (now.clinetInSystem >= 3)
                    {
                        int nextClient = devices.Select(device => device.clientIndex).Max() + 1;
                        now.clientInDevice[devices.IndexOf(nowStop)] = nextClient;
                        nowStop.clientIndex = nextClient;
                        double y = GenerateY(lambda);
                        nowStop.timeFree = t + y;
                        clients[nextClient - 1].timeService = y;
                        clients[nextClient - 1].timeWait = t - clients[nextClient - 1].timeCome;
                    }
                    else
                    {
                        nowStop.timeFree = double.MaxValue;
                        nowStop.clientIndex = -1;
                        now.clientInDevice[devices.IndexOf(nowStop)] = 0;
                    }

                }
                states.Add(now);
            }
            //return S;



            return states;
        }

        public static double GenerateY(double lambda)
        {
            var U = r.NextDouble();


            return -1 / (lambda * Math.Log(U, 2));
        }

        static List<Device> devices;
        static double nextClient;
        //static List<double> A, D;
        static List<Client> clients;
        static double GenerateNextClientCome(double t_now, double lambda, lambda_f lambda_t)
        {
            double t = t_now;
            while (true)
            {

                var U1 = r.NextDouble();
                t -= 1 / lambda * Math.Log(U1, 2);

                Random r2 = new Random(DateTime.Now.Millisecond);
                var U2 = r2.NextDouble();
                if (U2 < lambda_t(t) / lambda)
                    return t;//S.Add(t);
            }

        }

        //static (ServiceState state, int device) NewClient(ServiceState prev, int ind_now)
        //{
        //    ServiceState next = new ServiceState(prev);
        //    next.clinetInSystem += 1;

        //    int device_ind = -1;
        //    for (int i = 0; i < devices.Count; i++)
        //    {
        //        if (devices[i].clientIndex == -1)
        //        {

        //            next.clientInDevice[i] = ind_now;
        //            device_ind = i;
        //            break;
        //        }
        //    }



        //    return (next, device_ind);
        // }


        static int GetDeviceForClent()
        {
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].clientIndex == -1)
                {

                    return i;
                }
            }
            return -1;
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
            // A = new List<double>();
            //D = new List<double>();

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


            Console.ReadKey();


            //var A = PuassonGeneration.Generator.CreateNotUniform(T, lambda, lambda_func);
            //double Vmin = 0.5;
            //double Vmax = 1.5;

            //List<double> V = new List<double>();
            //List<double> D = new List<double>();
            //List<double> W = new List<double>();

            //Random r = new Random();
            //for (int i = 0; i < A.Count; i++)
            //{
            //    V.Add(Vmin + (r.NextDouble() * (Vmax - Vmin)));
            //}
            //W.Add(0);
            //W.Add(0);
            //D.Add(A[0] + V[0]);
            //D.Add(A[1] + V[1]);

            //for (int i = 2; i < A.Count; i++)
            //{
            //    D.Add(Math.Max(A[i], Math.Min(D[i - 1], D[i - 2])) + V[i]);
            //    W.Add(D[i] - A[i] - V[i]);
            //}

            //List<ServiceEvent> events = new List<ServiceEvent>();

            //for (int i = 0; i < A.Count; i++)
            //{
            //    events.Add(new ServiceEvent("A" + (i + 1), A[i]));
            //    events.Add(new ServiceEvent("D" + (i + 1), D[i]));
            //    //events.Add(new ServiceEvent("A" + (i + 1), A[i]));
            //}

            //events.Sort((ServiceEvent e1, ServiceEvent e2) => (e1.time > e2.time ? 1 : e1.time == e2.time ? 0 : -1));

            //for (int i = 0; i < events.Count - 1; i++)
            //{
            //    Console.WriteLine(events[i].time + "    " + events[i].name + "     " + events[i + 1].name + " " + events[i + 1].time);
            //}

            //Console.WriteLine(events[events.Count - 1].time + "    " + events[events.Count - 1].name);
        }
    }
}
