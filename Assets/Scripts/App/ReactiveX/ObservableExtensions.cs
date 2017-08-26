using System;
using Observer = UniRx.Observer;

namespace App.ReactiveX
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe(this IObservable source, Action<object> onNext)
        {
            return source.Subscribe(Observer.Create(onNext));
        }
    }
}