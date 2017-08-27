using System;
using UniRx;

namespace App.ReactiveX
{
    public class ReactiveProperty<T> : UniRx.ReactiveProperty<T>, IObservable
    {
        public ReactiveProperty() : this(default(T))
        {
        }

        public ReactiveProperty(T initialValue) : base(initialValue)
        {
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return Subscribe((IObserver<T>)observer);
        }

        public IDisposable Subscribe(Action onNext)
        {
            return Subscribe(Observer.Create<T>(v => { onNext(); }));
        }

        public IObservable SkipN(int count)
        {
            return new ObservableAdapter<T>(this.Skip(count));
        }
    }
}