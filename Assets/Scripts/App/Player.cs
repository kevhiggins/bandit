namespace App
{
    public class Player
    {
        public int Gold { get; private set; }
        public int Infamy { get; private set; }

        public Player()
        {
            Gold = 0;
            Infamy = 0;
        }
    }
}