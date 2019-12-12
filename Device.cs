using System;
using System.Collections.Generic;
using System.Text;

namespace CompMod
{
    class Device
    {
        public int clientIndex;
        public double timeFree;
        public int usedClient;
        public Device()
        {
            this.clientIndex = -1;
            this.timeFree = double.MaxValue;
            usedClient = 0;
        }
        public override string ToString()
        {
            return $"Device usedClients = {usedClient}";
        }

    }
}
