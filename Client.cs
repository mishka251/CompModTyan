using System;

namespace CompMod
{
    class Client
    {
        /// <summary>
        /// Время прихода клиента
        /// </summary>
        public double timeCome;
        /// <summary>
        /// Время ожидания клиента
        /// </summary>
        public double timeWait;
        /// <summary>
        /// Время обслуживания клиента
        /// </summary>
        public double timeService;
        /// <summary>
        /// Время ухода клиента
        /// </summary>
        public double timeLeave;
        /// <summary>
        /// Время в системе
        /// </summary>
        /// <param name="maxT">конец обслуживания</param>
        /// <returns></returns>
        public double GetTimeIsSystem (double maxT)
        {
            return Math.Min(timeLeave, maxT) - timeCome;
        }
        /// <summary>
        /// время ухода
        /// </summary>
        /// <param name="maxT">максвремя</param>
        /// <returns></returns>
        public double GetTimeLeave(double maxT)
        {
            return  Math.Min(timeLeave, maxT);
        }
        /// <summary>
        /// Время обслуживания с учетом закрытия
        /// </summary>
        /// <param name="maxT"></param>
        /// <returns></returns>
        public double GetTimeInDevice(double maxT)
        {
            return Math.Max(0, GetTimeLeave(maxT) - timeWait - timeCome);
        }

        public Client(double timeCome)
        {
            this.timeCome = timeCome;
        }

        public override string ToString()
        {
            return $"Клиент пришел в {timeCome:f2} ушел в {timeLeave:f2} ждал {timeWait:f2} обслуживался {timeService:f2}";
        }
    }
}
