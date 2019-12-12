using System;
using System.Collections.Generic;
using System.Text;

namespace CompMod
{
    class ServiceState
    {
        /// <summary>
        /// Кол-во клиентов в системе
        /// </summary>
        public int clinetInSystem;
        /// <summary>
        /// Какой клиент на каком устройстве
        /// </summary>
        public int[] clientInDevice;
        /// <summary>
        /// текущее время
        /// </summary>
        public double time;
        /// <summary>
        /// Тип события
        /// </summary>
        public string type;

        public int ClientsInQueue { get => Math.Max(0, clinetInSystem - devicesCnt); }

        public int devicesCnt;

     public static  readonly  int NoClientValue = 0;

        public ServiceState(string type, int devices)
        {
            this.type = type;
            devicesCnt = devices;
            this.clientInDevice = new int[devices];
            for (int i = 0; i < devices; i++)
            {
                clientInDevice[i] = NoClientValue;
            }
        }

        public ServiceState(double time, int devices)
        {
            this.time = time;
            devicesCnt = devices;
            this.clientInDevice = new int[devices];
            for (int i = 0; i < devices; i++)
            {
                clientInDevice[i] = -1;
            }
        }
        public ServiceState(ServiceState s)
        {
            this.clinetInSystem = s.clinetInSystem;
            devicesCnt = s.devicesCnt;
            this.clientInDevice = new int[s.clientInDevice.Length];
            for (int i = 0; i < s.clientInDevice.Length; i++)
            {
                this.clientInDevice[i] = s.clientInDevice[i];
            }
        }

        public ServiceState(ServiceState s, string type, double time)
        {
            this.type = type;
            this.clinetInSystem = s.clinetInSystem;
            devicesCnt = s.devicesCnt;

            this.clientInDevice = new int[s.clientInDevice.Length];
            for (int i = 0; i < s.clientInDevice.Length; i++)
            {
                this.clientInDevice[i] = s.clientInDevice[i];
            }
            this.time = time;
        }

        public override string ToString()
        {
            string res = $"Состояние {type} в {time:f2}:\n\tКлиентов в системе: {clinetInSystem} ";
            for (int i = 0; i < clientInDevice.Length; i++)
            {
                res += $"\n\tклиент на устройстве {i + 1} - {clientInDevice[i]}";
            }
            return res;
        }
    }

}
