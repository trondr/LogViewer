// © 2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

//Modified by trondr, 2016-01-03: Exctracted IEventsHelper interface, Added logging and exeception handling

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Common.Logging;
using github.trondr.LogViewer.Library.Infrastructure;

namespace github.trondr.LogViewer.Library.Module.Common
{
    [Singleton]
    public class EventsHelper : IEventsHelper
    {
        private readonly ILog _logger;

        public EventsHelper(ILog logger)
        {
            _logger = logger;
        }

        delegate void AsyncFire(Delegate del, object[] args);

        private void InvokeDelegate(Delegate del, object[] args)
        {
            try
            {
                ISynchronizeInvoke synchronizer = del.Target as ISynchronizeInvoke;
            if (synchronizer != null)//Requires thread affinity
            {
                if (synchronizer.InvokeRequired)
                {
                    synchronizer.Invoke(del, args);
                    return;
                }
            }
            //Not requiring thread afinity or invoke is not required
            del.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("InvokeDelegate() failed.{0}{1}", Environment.NewLine, ex.ToString());
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void UnsafeFire(Delegate del, params object[] args)
        {
            if (args.Length > 7)
            {
                _logger.WarnFormat("Too many parameters. Consider a structure to enable the use of the type-safe versions");                
            }
            if (del == null)
            {
                return;
            }
            Delegate[] delegates = del.GetInvocationList();

            foreach (Delegate sink in delegates)
            {
                try
                {
                    InvokeDelegate(sink, args);
                }
                catch(Exception ex)
                {
                    _logger.ErrorFormat("UnsafeFire() failed.{0}{1}", Environment.NewLine, ex.ToString());
                }
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void UnsafeFireInParallel(Delegate del, params object[] args)
        {
            if (args.Length > 7)
            {
                _logger.WarnFormat("Too many parameters. Consider a structure to enable the use of the type-safe versions");                
            }
            if (del == null)
            {
                return;
            }
            Delegate[] delegates = del.GetInvocationList();

            List<WaitHandle> calls = new List<WaitHandle>(delegates.Length);
            AsyncFire asyncFire = InvokeDelegate;

            foreach (Delegate sink in delegates)
            {
                IAsyncResult asyncResult = asyncFire.BeginInvoke(sink, args, null, null);
                calls.Add(asyncResult.AsyncWaitHandle);
            }
            WaitHandle[] handles = calls.ToArray();
            WaitHandle.WaitAll(handles);
            Action<WaitHandle> close = delegate (WaitHandle handle)
            {
                handle.Close();
            };
            Array.ForEach(handles, close);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void UnsafeFireAsync(Delegate del, params object[] args)
        {
            if (args.Length > 7)
            {
                _logger.WarnFormat("Too many parameters. Consider a structure to enable the use of the type-safe versions");                
            }
            if (del == null)
            {
                return;
            }
            Delegate[] delegates = del.GetInvocationList();
            AsyncFire asyncFire = InvokeDelegate;
            AsyncCallback cleanUp = delegate (IAsyncResult asyncResult)
            {
                asyncResult.AsyncWaitHandle.Close();
            };
            foreach (Delegate sink in delegates)
            {
                asyncFire.BeginInvoke(sink, args, cleanUp, null);
            }
        }
        public void Fire(EventHandler del, object sender, EventArgs e)
        {
            UnsafeFire(del, sender, e);
        }
        public void Fire<T>(EventHandler<T> del, object sender, T t) where T : EventArgs
        {
            UnsafeFire(del, sender, t);
        }
        public void Fire(GenericEventHandler del)
        {
            UnsafeFire(del);
        }
        public void Fire<T>(GenericEventHandler<T> del, T t)
        {
            UnsafeFire(del, t);
        }
        public void Fire<T, U>(GenericEventHandler<T, U> del, T t, U u)
        {
            UnsafeFire(del, t, u);
        }
        public void Fire<T, U, V>(GenericEventHandler<T, U, V> del, T t, U u, V v)
        {
            UnsafeFire(del, t, u, v);
        }
        public void Fire<T, U, V, W>(GenericEventHandler<T, U, V, W> del, T t, U u, V v, W w)
        {
            UnsafeFire(del, t, u, v, w);
        }
        public void Fire<T, U, V, W, X>(GenericEventHandler<T, U, V, W, X> del, T t, U u, V v, W w, X x)
        {
            UnsafeFire(del, t, u, v, w, x);
        }
        public void Fire<T, U, V, W, X, Y>(GenericEventHandler<T, U, V, W, X, Y> del, T t, U u, V v, W w, X x, Y y)
        {
            UnsafeFire(del, t, u, v, w, x, y);
        }
        public void Fire<T, U, V, W, X, Y, Z>(GenericEventHandler<T, U, V, W, X, Y, Z> del, T t, U u, V v, W w, X x, Y y, Z z)
        {
            UnsafeFire(del, t, u, v, w, x, y, z);
        }

        public void FireInParallel(EventHandler del, object sender, EventArgs e)
        {
            UnsafeFireInParallel(del, sender, e);
        }
        public void FireInParallel<T>(EventHandler<T> del, object sender, T t) where T : EventArgs
        {
            UnsafeFireInParallel(del, sender, t);
        }
        public void FireInParallel(GenericEventHandler del)
        {
            UnsafeFireInParallel(del);
        }
        public void FireInParallel<T>(GenericEventHandler<T> del, T t)
        {
            UnsafeFireInParallel(del, t);
        }
        public void FireInParallel<T, U>(GenericEventHandler<T, U> del, T t, U u)
        {
            UnsafeFireInParallel(del, t, u);
        }
        public void FireInParallel<T, U, V>(GenericEventHandler<T, U, V> del, T t, U u, V v)
        {
            UnsafeFireInParallel(del, t, u, v);
        }
        public void FireInParallel<T, U, V, W>(GenericEventHandler<T, U, V, W> del, T t, U u, V v, W w)
        {
            UnsafeFireInParallel(del, t, u, v, w);
        }
        public void FireInParallel<T, U, V, W, X>(GenericEventHandler<T, U, V, W, X> del, T t, U u, V v, W w, X x)
        {
            UnsafeFireInParallel(del, t, u, v, w, x);
        }
        public void FireInParallel<T, U, V, W, X, Y>(GenericEventHandler<T, U, V, W, X, Y> del, T t, U u, V v, W w, X x, Y y)
        {
            UnsafeFireInParallel(del, t, u, v, w, x, y);
        }
        public void FireInParallel<T, U, V, W, X, Y, Z>(GenericEventHandler<T, U, V, W, X, Y, Z> del, T t, U u, V v, W w, X x, Y y, Z z)
        {
            UnsafeFireInParallel(del, t, u, v, w, x, y, z);
        }

        public void FireAsync(EventHandler del, object sender, EventArgs e)
        {
            UnsafeFireAsync(del, sender, e);
        }
        public void FireAsync<T>(EventHandler<T> del, object sender, T t) where T : EventArgs
        {
            UnsafeFireAsync(del, sender, t);
        }
        public void FireAsync(GenericEventHandler del)
        {
            UnsafeFireAsync(del);
        }
        public void FireAsync<T>(GenericEventHandler<T> del, T t)
        {
            UnsafeFireAsync(del, t);
        }
        public void FireAsync<T, U>(GenericEventHandler<T, U> del, T t, U u)
        {
            UnsafeFireAsync(del, t, u);
        }
        public void FireAsync<T, U, V>(GenericEventHandler<T, U, V> del, T t, U u, V v)
        {
            UnsafeFireAsync(del, t, u, v);
        }
        public void FireAsync<T, U, V, W>(GenericEventHandler<T, U, V, W> del, T t, U u, V v, W w)
        {
            UnsafeFireAsync(del, t, u, v, w);
        }
        public void FireAsync<T, U, V, W, X>(GenericEventHandler<T, U, V, W, X> del, T t, U u, V v, W w, X x)
        {
            UnsafeFireAsync(del, t, u, v, w, x);
        }
        public void FireAsync<T, U, V, W, X, Y>(GenericEventHandler<T, U, V, W, X, Y> del, T t, U u, V v, W w, X x, Y y)
        {
            UnsafeFireAsync(del, t, u, v, w, x, y);
        }
        public void FireAsync<T, U, V, W, X, Y, Z>(GenericEventHandler<T, U, V, W, X, Y, Z> del, T t, U u, V v, W w, X x, Y y, Z z)
        {
            UnsafeFireAsync(del, t, u, v, w, x, y, z);
        }
    }
}