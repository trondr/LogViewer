using System;

namespace github.trondr.LogViewer.Library.Module.Common
{
    public interface IEventsHelper
    {
        void UnsafeFire(Delegate del, params object[] args);
        void UnsafeFireInParallel(Delegate del, params object[] args);
        void UnsafeFireAsync(Delegate del, params object[] args);
        void Fire(EventHandler del, object sender, EventArgs e);
        void Fire<T>(EventHandler<T> del, object sender, T t) where T : EventArgs;
        void Fire(GenericEventHandler del);
        void Fire<T>(GenericEventHandler<T> del, T t);
        void Fire<T, U>(GenericEventHandler<T, U> del, T t, U u);
        void Fire<T, U, V>(GenericEventHandler<T, U, V> del, T t, U u, V v);
        void Fire<T, U, V, W>(GenericEventHandler<T, U, V, W> del, T t, U u, V v, W w);
        void Fire<T, U, V, W, X>(GenericEventHandler<T, U, V, W, X> del, T t, U u, V v, W w, X x);
        void Fire<T, U, V, W, X, Y>(GenericEventHandler<T, U, V, W, X, Y> del, T t, U u, V v, W w, X x, Y y);
        void Fire<T, U, V, W, X, Y, Z>(GenericEventHandler<T, U, V, W, X, Y, Z> del, T t, U u, V v, W w, X x, Y y, Z z);
        void FireInParallel(EventHandler del, object sender, EventArgs e);
        void FireInParallel<T>(EventHandler<T> del, object sender, T t) where T : EventArgs;
        void FireInParallel(GenericEventHandler del);
        void FireInParallel<T>(GenericEventHandler<T> del, T t);
        void FireInParallel<T, U>(GenericEventHandler<T, U> del, T t, U u);
        void FireInParallel<T, U, V>(GenericEventHandler<T, U, V> del, T t, U u, V v);
        void FireInParallel<T, U, V, W>(GenericEventHandler<T, U, V, W> del, T t, U u, V v, W w);
        void FireInParallel<T, U, V, W, X>(GenericEventHandler<T, U, V, W, X> del, T t, U u, V v, W w, X x);
        void FireInParallel<T, U, V, W, X, Y>(GenericEventHandler<T, U, V, W, X, Y> del, T t, U u, V v, W w, X x, Y y);
        void FireInParallel<T, U, V, W, X, Y, Z>(GenericEventHandler<T, U, V, W, X, Y, Z> del, T t, U u, V v, W w, X x, Y y, Z z);
        void FireAsync(EventHandler del, object sender, EventArgs e);
        void FireAsync<T>(EventHandler<T> del, object sender, T t) where T : EventArgs;
        void FireAsync(GenericEventHandler del);
        void FireAsync<T>(GenericEventHandler<T> del, T t);
        void FireAsync<T, U>(GenericEventHandler<T, U> del, T t, U u);
        void FireAsync<T, U, V>(GenericEventHandler<T, U, V> del, T t, U u, V v);
        void FireAsync<T, U, V, W>(GenericEventHandler<T, U, V, W> del, T t, U u, V v, W w);
        void FireAsync<T, U, V, W, X>(GenericEventHandler<T, U, V, W, X> del, T t, U u, V v, W w, X x);
        void FireAsync<T, U, V, W, X, Y>(GenericEventHandler<T, U, V, W, X, Y> del, T t, U u, V v, W w, X x, Y y);
        void FireAsync<T, U, V, W, X, Y, Z>(GenericEventHandler<T, U, V, W, X, Y, Z> del, T t, U u, V v, W w, X x, Y y, Z z);
    }
}