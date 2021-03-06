﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VTree.Events
{
    public class AsyncEventSubscriber<T>
        where T : EventArgs
    {
        private Queue<T> query = new Queue<T>();

        public event EventHandler<T> OnEvent;

        public EventHandler<T> Handler
        {
            get
            {
                if (this.handler == null)
                {
                    this.handler = (object sender, T e) =>
                    {
                        lock (this.query)
                        {
                            this.query.Enqueue(e);
                            Monitor.Pulse(this.query);
                        }
                    };
                }

                return this.handler;
            }
        }
        private EventHandler<T> handler;

        public Thread Thread
        {
            get
            {
                if (this.thread == null)
                {
                    this.thread = new Thread(() =>
                    {
                        while (true)
                        {
                            lock (this.query)
                            {
                                while (this.query.Count == 0)
                                {
                                    Monitor.Wait(this.query);
                                }
                                T e = this.query.Dequeue();

                            // this??
                            this.OnEvent?.Invoke(this, e);
                            }
                        }
                    });
                }

                return this.thread;
            }
        }
        private Thread thread;
    }
}
