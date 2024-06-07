namespace Netytar.Controllers.Color
{
    class ColorManager
    {
        private const int span = -30;

        public System.Windows.Media.Color GetHighLightedVersion(System.Windows.Media.Color color)
        {
            int R = color.R + span;
            int G = color.G + span;
            int B = color.B + span;

            return System.Windows.Media.Color.FromRgb((byte)R, (byte)G, (byte)B);
        }
    }
}