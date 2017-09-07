using App.ReactiveX;

namespace App
{
    public class Player
    {
        public ReactiveProperty<int> Gold { get; private set; }
        public ReactiveProperty<int> Infamy { get; private set; }

        public Player()
        {
            Gold = new ReactiveProperty<int>(5);
            Infamy = new ReactiveProperty<int>(2);
        }
    }
}