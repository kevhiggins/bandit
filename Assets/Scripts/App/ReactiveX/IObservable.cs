using System;
using UniRx;

namespace App.ReactiveX
{
    public interface IObservable
    {
        IDisposable Subscribe(IObserver<object> observer);
        IDisposable Subscribe(Action onNext);
        IObservable SkipN(int count);
    }
}