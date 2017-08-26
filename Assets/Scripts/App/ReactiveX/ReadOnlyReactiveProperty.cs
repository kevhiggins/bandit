using System;
using UniRx;

namespace App.ReactiveX
{
    public class ReadOnlyReactiveProperty<T> : UniRx.ReadOnlyReactiveProperty<T>, IObservable
    {
        public ReadOnlyReactiveProperty(IObservable<T> source) : base(source)
        {
        }

        public ReadOnlyReactiveProperty(IObservable<T> source, bool distinctUntilChanged) : base(source, distinctUntilChanged)
        {
        }

        public ReadOnlyReactiveProperty(IObservable<T> source, T initialValue) : base(source, initialValue)
        {
        }

        public ReadOnlyReactiveProperty(IObservable<T> source, T initialValue, bool distinctUntilChanged) : base(source, initialValue, distinctUntilChanged)
        {
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return Subscribe((IObserver<T>)observer);
        }
    }
}