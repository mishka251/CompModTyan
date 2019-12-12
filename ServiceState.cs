using System;
using System.Collections.Generic;
using System.Text;

namespace CompMod
{
    class ServiceState
    {
        public int clinetInSystem;
        public int[] clientInDevice;
        public double time;
        public string type;

        public ServiceState(string type)
        {
            this.type = type;
        }

        public ServiceState(double time)
        {
            this.time = time;
        }
        public ServiceState(ServiceState s)
        {
            this.clinetInSystem = s.clinetInSystem;
            var arr = new int[] { 1, 2 };

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
            var arr = new int[] { 1, 2 };

            this.clientInDevice = new int[s.clientInDevice.Length];
            for (int i = 0; i < s.clientInDevice.Length; i++)
            {
                this.clientInDevice[i] = s.clientInDevice[i];
            }
            this.time = time;
        }

        public override string ToString()
        {
            string res = $"State {type} at {time}: clients in system: {clinetInSystem} ";
            for (int i = 0; i < clientInDevice.Length; i++)
            {
                res += $" in device{i + 1} = {clientInDevice[i]}";
            }
            return res;
        }
    }

}
