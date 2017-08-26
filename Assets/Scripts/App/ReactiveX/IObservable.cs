using System;
using UniRx;

namespace App.ReactiveX
{
    public interface IObservable
    {
        IDisposable Subscribe(IObserver<object> observer);
    }
}