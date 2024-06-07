using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Netytar.Modules;
using NITHdmis.Music;
using WpfAnimatedGif;

namespace Netytar.Surface
{
    public enum NetytarSurfaceLineModes
    {
        AllLines,
        OnlyScaleLines,
        NoLines
    }

    public enum NetytarSurfaceHighlightModes
    {
        CurrentNote,
        None
    }

    
    public class NetytarSurface
    {
        private Ellipse highlighter;

        private NetytarButton lastCheckedButton;
        private Scale scale = ScalesFactory.Cmaj;
        private List<TextBlock> NoteNames { get; set; } = new List<TextBlock>();


        public NetytarSurface(Canvas canvas, NetytarSurfaceLineModes drawMode)
        {
            this.DrawMode = drawMode;

            this.canvas = canvas;
            InitializeHighlighter();

            ResetCanvasDimensions();
        }

        private void InitializeHighlighter()
        {
            highlighter = new Ellipse();
            highlighter.Height = highlighter.Width = Rack.UserSettings.HighlightRadius;
            highlighter.Stroke = new SolidColorBrush(Colors.White);
            highlighter.StrokeThickness = Rack.UserSettings.HighlightStrokeDim;
        }

        public NetytarButton CheckedButton { get; private set; }
        private bool noteNamesVisualized = false;
        public NetytarSurfaceLineModes DrawMode { get; set; } = NetytarSurfaceLineModes.OnlyScaleLines;
        public NetytarSurfaceHighlightModes HighLightMode { get; set; }

        public Scale Scale
        {
            get { return scale; }
            set { scale = value; DrawButtons(); }
        }

        public bool NoteNamesVisualized
        {
            get { return noteNamesVisualized; }
            set
            {
                noteNamesVisualized = value;
                if (value)
                {
                    DrawNoteNames();
                }
                else
                {
                    ClearNoteNames();
                }
            }
        }

        #region Surface components

        private Canvas canvas;

        private List<Ellipse> drawnEllipses = new List<Ellipse>();
        private List<Line> drawnLines = new List<Line>();
        private NetytarButton[,] netytarButtons;

        #endregion Surface components

        public void ClearEllipses()
        {
            for (int i = 0; i < drawnEllipses.Count; i++)
            {
                Ellipse ellipse = drawnEllipses[i];
                canvas.Children.Remove(ellipse);
            }

            drawnEllipses = new List<Ellipse>();
        }

        public void ClearLines()
        {
            for (int i = 0; i < drawnLines.Count; i++)
            {
                Line line = drawnLines[i];
                canvas.Children.Remove(line);
            }

            drawnLines = new List<Line>();
        }
        public void ClearNoteNames()
        {
            if(NoteNames.Count > 0)
            {
                foreach(TextBlock textBlock in NoteNames)
                {
                    canvas.Children.Remove(textBlock);
                }
            }
            NoteNames.Clear();
        }

        public void DrawNoteNames()
        {
            ClearNoteNames();
            foreach(NetytarButton button in netytarButtons)
            {
                TextBlock text = new TextBlock();
                text.Text = button.Note.ToStandardString();
                text.FontSize = 16;
                text.Foreground = new SolidColorBrush(Colors.White);
                text.Height = 20;
                text.Width = button.Ellipse.Width;
                text.TextAlignment = TextAlignment.Center;
                Canvas.SetLeft(text, Canvas.GetLeft(button.Ellipse));
                Canvas.SetTop(text, Canvas.GetTop(button.Ellipse) + button.Ellipse.Height + 5);
                NoteNames.Add(text);
                canvas.Children.Add(text);
            }
        }

        /// <summary>
        /// Logica di disegno dei pulsanti a schermo
        /// </summary>
        public void DrawButtons()
        {
            ClearButtons();
            ResetCanvasDimensions();

            if (canvas.Children.Contains(highlighter))
            {
                canvas.Children.Remove(highlighter);
            }
            canvas.Children.Add(highlighter);

            // Mi segno un po' di misure
            int halfSpacer = Rack.UserSettings.HorizontalSpacer / 2;
            int spacer = Rack.UserSettings.HorizontalSpacer;

            int firstSpacer = 0;

            bool isPairRow;

            for (int row = 0; row < Rack.ButtonsSettings.NRows; row++)
            {
                for (int col = 0; col < Rack.ButtonsSettings.NCols; col++)
                {
                    #region Is row pair?

                    if (row % 2 != 0)
                    {
                        isPairRow = false;
                    }
                    else
                    {
                        isPairRow = true;
                    }

                    #endregion Is row pair?

                    #region Draw the button on canvas

                    if (isPairRow)
                    {
                        firstSpacer = halfSpacer;
                    }
                    else
                    {
                        firstSpacer = 0;
                    }

                    netytarButtons[row, col] = new NetytarButton(this);

                    #region Define note

                    int calcPitch = Rack.ButtonsSettings.GenerativeNote;
                    calcPitch += col * 2 + row * 2;
                    if (isPairRow)
                    {
                        calcPitch += 1;
                    }
                    netytarButtons[row, col].Note = PitchUtils.PitchToMidiNote(calcPitch);

                    #endregion Define note

                    // Algoritmo che trova la posizione del pulsante
                    int X = Rack.ButtonsSettings.StartPositionX + firstSpacer + col * Rack.UserSettings.HorizontalSpacer;
                    int Y = Rack.ButtonsSettings.StartPositionY + Rack.UserSettings.VerticalSpacer * row;
                    Canvas.SetLeft(netytarButtons[row, col], X);
                    Canvas.SetTop(netytarButtons[row, col], Y);

                    // Algoritmo che crea l'occluder e ne definisce le dimensioni
                    netytarButtons[row, col].Occluder.Width = Rack.UserSettings.OccluderOffset * 2;
                    netytarButtons[row, col].Occluder.Height = Rack.UserSettings.OccluderOffset * 2;
                    netytarButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb((byte)Rack.ButtonsSettings.OccluderAlpha, 0xFF, 0xFF, 0xFF));
                    netytarButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)Rack.ButtonsSettings.OccluderAlpha, 0, 0, 0));
                    Canvas.SetLeft(netytarButtons[row, col].Occluder, X - Rack.UserSettings.OccluderOffset);
                    Canvas.SetTop(netytarButtons[row, col].Occluder, Y - Rack.UserSettings.OccluderOffset);

                    // Indica le posizioni sull'asse Z
                    Panel.SetZIndex(netytarButtons[row, col], 3);
                    Panel.SetZIndex(netytarButtons[row, col].Occluder, 2000);

                    // Aggiunge gli oggetti al canvas di Netytar
                    canvas.Children.Add(netytarButtons[row, col]);
                    canvas.Children.Add(netytarButtons[row, col].Occluder);
                    if (Rack.UserSettings.SharpNotesMode == _SharpNotesModes.Off && !scale.IsInScale(netytarButtons[row, col].Note))
                    {
                        netytarButtons[row, col].Visibility = Visibility.Hidden;
                        netytarButtons[row, col].Occluder.Visibility = Visibility.Hidden;
                    }

                    #endregion Draw the button on canvas

                    // ===========================================================================
                }
            }
            DrawScale();
        }

        public void DrawEllipses(Scale scale)
        {
            if (netytarButtons != null)
            {
                ClearEllipses();

                List<AbsNotes> scaleNotes = scale.NotesInScale;

                for (int j = 0; j < Rack.ButtonsSettings.NCols; j++)
                {
                    for (int k = 0; k < Rack.ButtonsSettings.NCols; k++)
                    {
                        AbsNotes note = netytarButtons[j, k].Note.ToAbsNote();

                        Ellipse ellipse = new Ellipse();

                        ellipse.Width = Rack.UserSettings.EllipseRadius * 2;
                        ellipse.Height = Rack.UserSettings.EllipseRadius * 2;

                        Canvas.SetLeft(ellipse, Canvas.GetLeft(netytarButtons[j, k]) - Rack.UserSettings.EllipseRadius);
                        Canvas.SetTop(ellipse, Canvas.GetTop(netytarButtons[j, k]) - Rack.UserSettings.EllipseRadius);
                        Canvas.SetZIndex(ellipse, 2);

                        if (!scaleNotes.Contains(note))
                        {
                            if (Rack.UserSettings.SharpNotesMode == _SharpNotesModes.On)
                            {
                                ellipse.Stroke = Rack.ColorCode.NotInScaleBrush;
                                ellipse.Fill = Rack.ColorCode.NotInScaleBrush;
                            }
                            else if (Rack.UserSettings.SharpNotesMode == _SharpNotesModes.Off)
                            {
                                ellipse.Stroke = new SolidColorBrush(Colors.Transparent);
                                ellipse.Fill = new SolidColorBrush(Colors.Transparent);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < scaleNotes.Count; i++)
                            {
                                if (note == scaleNotes[i])
                                {
                                    ellipse.Stroke = Rack.ColorCode.KeysColorCode[i];
                                    ellipse.Fill = Rack.ColorCode.KeysColorCode[i];
                                }
                            }
                        }

                        drawnEllipses.Add(ellipse);
                        netytarButtons[j, k].Ellipse = ellipse;
                    }
                }

                foreach (Ellipse ellipse in drawnEllipses)
                {
                    canvas.Children.Add(ellipse);
                }
            }
        }

        public void DrawScale()
        {
            DrawEllipses(scale);
            DrawLines(scale);

            NoteNamesVisualized = Rack.UserSettings.NoteNamesVisualized;
        }

        public void FlashMovementLine()
        {
            if (lastCheckedButton != null)
            {
                Point point1 = new Point(Canvas.GetLeft(CheckedButton), Canvas.GetTop(CheckedButton));
                Point point2 = new Point(Canvas.GetLeft(lastCheckedButton), Canvas.GetTop(lastCheckedButton));
                LineFlasherTimer timer = new LineFlasherTimer(point1, point2, canvas, Colors.NavajoWhite);
            }
        }

        public void FlashSpark()
        {
            if (CheckedButton != null)
            {
                Image image = new Image();
                image.Height = 40;
                image.Width = 40;
                Canvas.SetLeft(image, Canvas.GetLeft(CheckedButton) - 10);
                Canvas.SetTop(image, Canvas.GetTop(CheckedButton) - 15);
                Canvas.SetZIndex(image, 100);
                canvas.Children.Add(image);

                BitmapImage bitImage = new BitmapImage();
                bitImage.BeginInit();
                bitImage.UriSource = new Uri("pack://application:,,,/Images/Sparks/Spark3.gif");
                bitImage.EndInit();
                ImageBehavior.SetAnimatedSource(image, bitImage);
                ImageBehavior.SetRepeatBehavior(image, new RepeatBehavior(1));
                ImageBehavior.AddAnimationCompletedHandler(image, DisposeImage);
            }
        }

        public void NetytarButton_OccluderMouseEnter(NetytarButton sender)
        {
            if (sender != CheckedButton)
            {
                Rack.NetytarDmiBox.SelectedNote = sender.Note;

                lastCheckedButton = CheckedButton;
                CheckedButton = sender;

                FlashMovementLine();

                if (HighLightMode == NetytarSurfaceHighlightModes.CurrentNote)
                {
                    MoveHighlighter(CheckedButton);
                }
            }
        }

        private void ResetCanvasDimensions()
        {
            canvas.Width = Rack.ButtonsSettings.StartPositionX * 2 + (Rack.UserSettings.HorizontalSpacer + 13) * (Rack.ButtonsSettings.NCols - 1);
            canvas.Height = Rack.ButtonsSettings.StartPositionY * 2 + (Rack.UserSettings.VerticalSpacer + 13) * (Rack.ButtonsSettings.NRows - 1);
        }

        private void ClearButtons()
        {
            if (netytarButtons != null)
            {
                foreach (NetytarButton button in netytarButtons)
                {
                    canvas.Children.Remove(button);
                    canvas.Children.Remove(button.Occluder);
                }

                netytarButtons = new NetytarButton[Rack.ButtonsSettings.NRows, Rack.ButtonsSettings.NCols];
            }
            netytarButtons = new NetytarButton[Rack.ButtonsSettings.NRows, Rack.ButtonsSettings.NCols];
        }

        private void DisposeImage(object sender, RoutedEventArgs e)
        {
            canvas.Children.Remove(((Image)sender));
        }

        private void DrawLines(Scale scale)
        {
            if (netytarButtons != null)
            {
                ClearLines();
                if (DrawMode != NetytarSurfaceLineModes.NoLines)
                {
                    Brush inScaleBrush = new SolidColorBrush();

                    #region Determine inScale brush

                    switch (scale.ScaleCode)
                    {
                        case ScaleCodes.maj:
                            inScaleBrush = Rack.ColorCode.MajorBrush;
                            break;

                        case ScaleCodes.min:
                            inScaleBrush = Rack.ColorCode.MinorBrush;
                            break;

                        default:
                            inScaleBrush = Rack.ColorCode.MajorBrush;
                            break;
                    }

                    #endregion Determine inScale brush

                    bool isPairRow;

                    Point realCenter1;
                    Point realCenter2;

                    for (int row = 0; row < Rack.ButtonsSettings.NRows; row++)
                    {
                        for (int col = 0; col < Rack.ButtonsSettings.NCols; col++)
                        {
                            #region Is row pair?

                            if (row % 2 != 0)
                            {
                                isPairRow = false;
                            }
                            else
                            {
                                isPairRow = true;
                            }

                            #endregion Is row pair?

                            #region Draw horizontal lines

                            if (col != 0)
                            {
                                Brush brush;
                                if (scale.AreConsequent(netytarButtons[row, col].Note, netytarButtons[row, col - 1].Note))
                                {
                                    brush = inScaleBrush;
                                }
                                else
                                {
                                    if (DrawMode == NetytarSurfaceLineModes.OnlyScaleLines)
                                    {
                                        brush = Rack.ColorCode.TransparentBrush;
                                    }
                                    else
                                    {
                                        brush = Rack.ColorCode.NotInScaleBrush;
                                    }
                                }
                                realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col].Ellipse) + netytarButtons[row, col].Ellipse.Width / 2, Canvas.GetTop(netytarButtons[row, col].Ellipse) + netytarButtons[row, col].Ellipse.Height / 2);
                                realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row, col - 1].Ellipse) + netytarButtons[row, col - 1].Ellipse.Width / 2, Canvas.GetTop(netytarButtons[row, col - 1].Ellipse) + netytarButtons[row, col - 1].Ellipse.Height / 2);
                                Line myLine = new Line();

                                myLine.Stroke = brush;
                                myLine.X1 = realCenter1.X;
                                myLine.X2 = realCenter2.X;
                                myLine.Y1 = realCenter1.Y;
                                myLine.Y2 = realCenter2.Y;
                                myLine.HorizontalAlignment = HorizontalAlignment.Center;
                                myLine.VerticalAlignment = VerticalAlignment.Center;
                                myLine.StrokeThickness = Rack.UserSettings.LineThickness;
                                Panel.SetZIndex(myLine, 1);
                                drawnLines.Add(myLine);

                                netytarButtons[row, col - 1].L_p2 = myLine;
                                netytarButtons[row, col].L_m2 = myLine;
                            }

                            #endregion Draw horizontal lines

                            #region Draw diagonal lines

                            // Diagonale A: se riga pari p+1, se dispari p+3
                            if (row != 0)
                            {
                                Brush brush;
                                if (scale.AreConsequent(netytarButtons[row, col].Note, netytarButtons[row - 1, col].Note))
                                {
                                    brush = inScaleBrush;
                                }
                                else
                                {
                                    if (DrawMode == NetytarSurfaceLineModes.OnlyScaleLines)
                                    {
                                        brush = Rack.ColorCode.TransparentBrush;
                                    }
                                    else
                                    {
                                        brush = Rack.ColorCode.NotInScaleBrush;
                                    }
                                }

                                if (isPairRow)
                                {
                                    realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col].Ellipse) + netytarButtons[row, col].Ellipse.Width / 2, Canvas.GetTop(netytarButtons[row, col].Ellipse) + netytarButtons[row, col].Ellipse.Height / 2);
                                    realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row - 1, col].Ellipse) + netytarButtons[row - 1, col].Ellipse.Width / 2, Canvas.GetTop(netytarButtons[row - 1, col].Ellipse) + netytarButtons[row - 1, col].Ellipse.Height / 2);
                                }
                                else
                                {
                                    realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col].Ellipse) + netytarButtons[row, col].Ellipse.Width / 2, Canvas.GetTop(netytarButtons[row, col].Ellipse) + netytarButtons[row, col].Ellipse.Width / 2);
                                    realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row - 1, col].Ellipse) + netytarButtons[row - 1, col].Ellipse.Width / 2, Canvas.GetTop(netytarButtons[row - 1, col].Ellipse) + netytarButtons[row - 1, col].Ellipse.Width / 2);
                                }

                                Line myLine = new Line();

                                myLine.Stroke = brush;
                                myLine.X1 = realCenter1.X;
                                myLine.X2 = realCenter2.X;
                                myLine.Y1 = realCenter1.Y;
                                myLine.Y2 = realCenter2.Y;
                                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                                myLine.VerticalAlignment = VerticalAlignment.Center;
                                myLine.StrokeThickness = Rack.UserSettings.LineThickness;
                                Panel.SetZIndex(myLine, 1);
                                drawnLines.Add(myLine);

                                if (isPairRow)
                                {
                                    netytarButtons[row - 1, col].L_p3 = myLine;
                                    netytarButtons[row, col].L_m3 = myLine;
                                }
                                else
                                {
                                    netytarButtons[row - 1, col].L_p1 = myLine;
                                    netytarButtons[row, col].L_m1 = myLine;
                                }

                                // Diagonale B: se riga pari p+3, se dispari p+1

                                if (isPairRow)
                                {
                                    if (col < Rack.ButtonsSettings.NCols - 1)
                                    {
                                        if (scale.AreConsequent(netytarButtons[row, col].Note, netytarButtons[row - 1, col + 1].Note))
                                        {
                                            brush = inScaleBrush;
                                        }
                                        else
                                        {
                                            if (DrawMode == NetytarSurfaceLineModes.OnlyScaleLines)
                                            {
                                                brush = Rack.ColorCode.TransparentBrush;
                                            }
                                            else
                                            {
                                                brush = Rack.ColorCode.NotInScaleBrush;
                                            }
                                        }
                                        realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col]) + 11, Canvas.GetTop(netytarButtons[row, col]) + 2);
                                        realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row - 1, col + 1]) + 2, Canvas.GetTop(netytarButtons[row - 1, col + 1]) + 11);

                                        Line diaLine = new Line();

                                        diaLine.Stroke = brush;
                                        diaLine.X1 = realCenter1.X;
                                        diaLine.X2 = realCenter2.X;
                                        diaLine.Y1 = realCenter1.Y;
                                        diaLine.Y2 = realCenter2.Y;
                                        diaLine.HorizontalAlignment = HorizontalAlignment.Left;
                                        diaLine.VerticalAlignment = VerticalAlignment.Center;
                                        diaLine.StrokeThickness = Rack.UserSettings.LineThickness;
                                        Panel.SetZIndex(diaLine, 1);
                                        drawnLines.Add(diaLine);

                                        netytarButtons[row - 1, col + 1].L_p1 = diaLine;
                                        netytarButtons[row, col].L_m1 = diaLine;
                                    }
                                }
                                else
                                {
                                    if (col > 0)
                                    {
                                        if (scale.AreConsequent(netytarButtons[row, col].Note, netytarButtons[row - 1, col - 1].Note))
                                        {
                                            brush = inScaleBrush;
                                        }
                                        else
                                        {
                                            if (DrawMode == NetytarSurfaceLineModes.OnlyScaleLines)
                                            {
                                                brush = Rack.ColorCode.TransparentBrush;
                                            }
                                            else
                                            {
                                                brush = Rack.ColorCode.NotInScaleBrush;
                                            }
                                        }
                                        realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col]) + 2, Canvas.GetTop(netytarButtons[row, col]) + 2);
                                        realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row - 1, col - 1]) + 11, Canvas.GetTop(netytarButtons[row - 1, col - 1]) + 11);

                                        Line diaLine = new Line();

                                        diaLine.Stroke = brush;
                                        diaLine.X1 = realCenter1.X;
                                        diaLine.X2 = realCenter2.X;
                                        diaLine.Y1 = realCenter1.Y;
                                        diaLine.Y2 = realCenter2.Y;
                                        diaLine.HorizontalAlignment = HorizontalAlignment.Left;
                                        diaLine.VerticalAlignment = VerticalAlignment.Center;
                                        diaLine.StrokeThickness = Rack.UserSettings.LineThickness;
                                        Panel.SetZIndex(diaLine, 1);
                                        drawnLines.Add(diaLine);

                                        netytarButtons[row - 1, col - 1].L_p3 = diaLine;
                                        netytarButtons[row, col].L_m3 = diaLine;
                                    }
                                }
                            }

                            #endregion Draw diagonal lines
                        }
                    }

                    foreach (Line line in drawnLines)
                    {
                        canvas.Children.Add(line);
                    }
                }
            }
        }

        private void MoveHighlighter(NetytarButton checkedButton)
        {
            Canvas.SetLeft(highlighter, Canvas.GetLeft(checkedButton) - highlighter.ActualWidth / 2);
            Canvas.SetTop(highlighter, Canvas.GetTop(checkedButton) - highlighter.ActualHeight / 2);
        }
    }
}