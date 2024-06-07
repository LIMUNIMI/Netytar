namespace Netytar.Surface.ButtonsSettings
{
    public interface IButtonsSettings
    {
        int NCols { get; }
        int NRows { get; }
        int Spacing { get; }
        int GenerativeNote { get; }
        int StartPositionX { get; }
        int StartPositionY { get; }
        int OccluderAlpha { get; }
    }
}
