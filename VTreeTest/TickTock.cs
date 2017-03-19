using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VTreeTest
{
    class TickTock
    {
        private object lockOn = new object();

        public void Tick(bool running)
        {
            lock (lockOn)
            {
                if (!running)
                {
                    // Остановить часы
                    Monitor.Pulse(lockOn);
                    return;
                }

                Console.Write("Тик ");
                // Разрешить выполнение метода Tock()
                Monitor.Pulse(lockOn);

                // Ожидать завершение Tock()
                Monitor.Wait(lockOn);
            }
        }

        public void Tock(bool running)
        {
            lock (lockOn)
            {
                if (!running)
                {
                    Monitor.Pulse(lockOn);
                    return;
                }

                Console.WriteLine("так");
                Monitor.Pulse(lockOn);
                Monitor.Wait(lockOn);
            }
        }
    }
}
