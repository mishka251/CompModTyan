using System;
using System.Collections.Generic;
using System.Text;

namespace CompMod
{
    class Client
    {
        public double timeCome;
        public double timeWait;
        public double timeService;

        public double timeLeave;

        public Client()
        {

        }
        public Client(double timeCome)
        {
            this.timeCome = timeCome;
        }

        public override string ToString()
        {
            return $"Client come at = {timeCome} leave at {timeLeave} wait={timeWait} servcie={timeService}";
        }
    }
}
