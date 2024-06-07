using System.Windows.Media;
using NITHdmis.Music;

namespace Netytar.Surface.ColorCodes
{
    public interface IColorCode
    {
        List<SolidColorBrush> KeysColorCode { get; }
        SolidColorBrush NotInScaleBrush { get; }
        SolidColorBrush MinorBrush { get; }
        SolidColorBrush MajorBrush { get; }
        SolidColorBrush HighlightBrush { get; }
        SolidColorBrush TransparentBrush { get; }
        SolidColorBrush FromAbsNote(AbsNotes absNote);
    }
}
