using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VTreeTest
{
    public class ProducerConsumer
    {
        readonly object listLock = new object();
        Queue queue = new Queue();

        public void Produce(object o)
        {
            lock (listLock)
            {
                queue.Enqueue(o);

                // We always need to pulse, even if the queue wasn't
                // empty before. Otherwise, if we add several items
                // in quick succession, we may only pulse once, waking
                // a single thread up, even if there are multiple threads
                // waiting for items.            
                Monitor.Pulse(listLock);
            }
        }

        public object Consume()
        {
            lock (listLock)
            {
                // If the queue is empty, wait for an item to be added
                // Note that this is a while loop, as we may be pulsed
                // but not wake up before another thread has come in and
                // consumed the newly added object. In that case, we'll
                // have to wait for another pulse.
                while (queue.Count == 0)
                {
                    // This releases listLock, only reacquiring it
                    // after being woken up by a call to Pulse
                    Monitor.Wait(listLock);
                }
                return queue.Dequeue();
            }
        }
    }
}
