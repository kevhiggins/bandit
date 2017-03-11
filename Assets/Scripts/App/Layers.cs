using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace App
{
    class Layers
    {
        public const int MouseClicks = 8;
        public const int BanditClicks = 12;

        public static int GetLayerBitMask(int layerId)
        {
            return 1 << layerId;
        }
    }
}
