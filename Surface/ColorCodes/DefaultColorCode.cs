using System.Windows.Media;
using NITHdmis.Music;

namespace Netytar.Surface.ColorCodes
{
    class DefaultColorCode : IColorCode
    {
        public List<SolidColorBrush> KeysColorCode { get; } = new List<SolidColorBrush>()
        {
            new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.Orange),
            new SolidColorBrush(Colors.Yellow),
            new SolidColorBrush(Colors.LightGreen),
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Purple),
            new SolidColorBrush(Colors.Coral)
        };
        public SolidColorBrush NotInScaleBrush { get; } = new SolidColorBrush(Color.FromRgb(25, 25, 25));
        public SolidColorBrush MinorBrush { get; } = new SolidColorBrush(Colors.Blue);
        public SolidColorBrush MajorBrush { get; } = new SolidColorBrush(Colors.Red);
        public SolidColorBrush HighlightBrush { get; } = new SolidColorBrush(Colors.White);
        public SolidColorBrush TransparentBrush { get; } = new SolidColorBrush(Colors.Transparent);

        public SolidColorBrush FromAbsNote(AbsNotes absNote)
        {
            switch (absNote)
            {
                case AbsNotes.C:
                    return KeysColorCode[0];
                case AbsNotes.sC:
                    return NotInScaleBrush;
                case AbsNotes.D:
                    return KeysColorCode[1];
                case AbsNotes.sD:
                    return NotInScaleBrush;
                case AbsNotes.E:
                    return KeysColorCode[2];
                case AbsNotes.F:
                    return KeysColorCode[3];
                case AbsNotes.sF:
                    return NotInScaleBrush;
                case AbsNotes.G:
                    return KeysColorCode[4];
                case AbsNotes.sG:
                    return NotInScaleBrush;
                case AbsNotes.A:
                    return KeysColorCode[5];
                case AbsNotes.sA:
                    return NotInScaleBrush;
                case AbsNotes.B:
                    return KeysColorCode[6];
                case AbsNotes.NaN:
                    return NotInScaleBrush;
            }
            return NotInScaleBrush;
        }
    }
}
