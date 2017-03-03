namespace Bandit
{
    class Layers
    {
        public const int MouseClicks = 8;

        public static int GetLayerBitMask(int layerId)
        {
            return 1 << layerId;
        }
    }
}
