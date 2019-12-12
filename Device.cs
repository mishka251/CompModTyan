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

        public int clientIndex;
        public double timeFree;
        public int usedClient;
        public Device()
        {
            this.clientIndex = -1;
            this.timeFree = double.MaxValue;
            usedClient = 0;

            clients = new List<Client>();

        }
        public override string ToString()
        {
            return $"Device usedClients = {usedClient}";
        }

        public double GetWorkTime(double maxT) {
            return clients.Select(client => client.GetTimeInDevice(maxT)).Sum();
        }

    }
}
