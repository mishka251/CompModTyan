using System;
using System.Collections.Generic;
using System.Linq;

namespace CompMod
{
    class Device
    {
        /// <summary>
        /// Клиенты обслуживавшиеся этим устройством
        /// </summary>
        public List<Client> clients;
        public double timeFree;
        public Device()
        {
            this.timeFree = double.MaxValue;

            clients = new List<Client>();
        }
        public override string ToString()
        {
            return $"Клиентов обслужено = {clients.Count}";
        }

        public double GetWorkTime(double maxT) {
            return clients.Select(client => client.GetTimeInDevice(maxT)).Sum();
        }

    }
}
