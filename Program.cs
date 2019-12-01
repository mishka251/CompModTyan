using System;
using System.Collections.Generic;

namespace CompMod
{
    class Program
    {
        //struct Client
        //{
        //    double timeCome;
        //    double timeWait;
        //    double timeService;

        //    double timeLeave { get => timeCome + timeWait + timeService; }

        //}

        struct ServiceEvent
        {
            public string name;
            public double time;
            public ServiceEvent(string n, double t)
            {
                this.name = n;
                this.time = t;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("T=");
            double T = double.Parse(Console.ReadLine());
            double lambda = 1.0;
            PuassonGeneration.lambda_f lambda_func = (x) => x > T / 2 ? 0.5 : 1;

            var A = PuassonGeneration.Generator.CreateNotUniform(T, lambda, lambda_func);
            double Vmin = 0.5;
            double Vmax = 1.5;

            List<double> V = new List<double>();
            List<double> D = new List<double>();
            List<double> W = new List<double>();

            Random r = new Random();
            for (int i = 0; i < A.Count; i++)
            {
                V.Add(Vmin + (r.NextDouble() * (Vmax - Vmin)));
            }
            W.Add(0);
            W.Add(0);
            D.Add(A[0] + V[0]);
            D.Add(A[1] + V[1]);

            for (int i = 2; i < A.Count; i++)
            {
                D.Add(Math.Max(A[i], Math.Min(D[i - 1], D[i - 2])) + V[i]);
                W.Add(D[i] - A[i] - V[i]);
            }

            List<ServiceEvent> events = new List<ServiceEvent>();

            for (int i = 0; i < A.Count; i++)
            {
                events.Add(new ServiceEvent("A" + (i + 1), A[i]));
                events.Add(new ServiceEvent("D" + (i + 1), D[i]));
                //events.Add(new ServiceEvent("A" + (i + 1), A[i]));
            }

            events.Sort((ServiceEvent e1, ServiceEvent e2) => (e1.time > e2.time ? 1 : e1.time == e2.time ? 0 : -1));

            for (int i = 0; i < events.Count - 1; i++)
            {
                Console.WriteLine(events[i].time + "    " + events[i].name + "     " + events[i + 1].name + " " + events[i + 1].time);
            }

            Console.WriteLine(events[events.Count - 1].time + "    " + events[events.Count - 1].name);
            Console.WriteLine("Клиентов было - " + A.Count);
            double Wmid = 0;
            foreach(double w in W)
            {
                Wmid += w;
            }
            Wmid /= W.Count;
            Console.WriteLine("Средняя задержка - "+Wmid);

        }
    }
}
