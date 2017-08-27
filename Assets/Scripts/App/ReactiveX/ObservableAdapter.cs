using System;
using UniRx;

namespace App.ReactiveX
{
    public class ObservableAdapter<T> : IObservable
    {
        private IObservable<T> observable;

        public ObservableAdapter(IObservable<T> observable)
        {
            this.observable = observable;
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return observable.Subscribe((IObserver<T>)observer);
        }

        public IDisposable Subscribe(Action onNext)
        {
            return observable.Subscribe(Observer.Create<T>(v => { onNext(); }));
        }

        public IObservable SkipN(int count)
        {
            return new ObservableAdapter<T>(observable.Skip(count));
        }
    }
}