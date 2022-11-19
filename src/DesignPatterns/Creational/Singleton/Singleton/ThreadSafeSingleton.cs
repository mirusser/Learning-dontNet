﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton;

// This Singleton implementation is called "double check lock". It is safe
// in multithreaded environment and provides lazy initialization for the
// Singleton object.
public sealed class ThreadSafeSingleton
{
    private ThreadSafeSingleton() { }

    private static ThreadSafeSingleton instance = null!;

    // We now have a lock object that will be used to synchronize threads
    // during first access to the Singleton.
    private static readonly object @lock = new();

    public static ThreadSafeSingleton GetInstance(string value)
    {
        // This conditional is needed to prevent threads stumbling over the
        // lock once the instance is ready.
        if (instance is null)
        {
            // Now, imagine that the program has just been launched. Since
            // there's no Singleton instance yet, multiple threads can
            // simultaneously pass the previous conditional and reach this
            // point almost at the same time. The first of them will acquire
            // lock and will proceed further, while the rest will wait here.
            lock (@lock)
            {
                // The first thread to acquire the lock, reaches this
                // conditional, goes inside and creates the Singleton
                // instance. Once it leaves the lock block, a thread that
                // might have been waiting for the lock release may then
                // enter this section. But since the Singleton field is
                // already initialized, the thread won't create a new
                // object.
                instance ??= new ThreadSafeSingleton
                {
                    Value = value
                };
            }
        }
        return instance;
    }

    // We'll use this property to prove that our Singleton really works.
    public string Value { get; set; }
}
