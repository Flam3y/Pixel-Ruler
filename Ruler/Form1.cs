
using System.Diagnostics;
using System.Drawing;
using WindowsInput.Events.Sources;

namespace Ruler
{
    public partial class Ruler : Form
    {
        private Point Start;
        private Point End;
        private IMouseEventSource Mouse = WindowsInput.Capture.Global.Mouse();
        private Graphics gr;

        public Ruler()
        {

            Mouse.DragStarted += Mouse_DragStarted;
            Mouse.DragFinished += Mouse_DragFinished;

            InitializeComponent();
        }

        private void Mouse_MouseMove(object? sender, WindowsInput.Events.Sources.EventSourceEventArgs<WindowsInput.Events.MouseMove> e)
        {
            End = new(e.Data.X, e.Data.Y);

            //pictureBox1.Size = new Size(Math.Abs(Start.X - End.X), Math.Abs(Start.Y - End.Y));
            //pictureBox1.Location = new Point(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
            var sv = Stopwatch.StartNew();

            pictureBox1.Image = null;
            pictureBox1.Invalidate();
        }


        private void Mouse_DragStarted(object? sender, WindowsInput.Events.Sources.EventSourceEventArgs<IReadOnlyList<WindowsInput.Events.DragStart>> e)
        {

            Debug.WriteLine("Mouse_DragStarted started");
            Mouse.MouseMove += Mouse_MouseMove;

            var start = e.Data[0].PositionDown;

            Start.Y = start.Y;
            Start.X = start.X;
        }

        private void Mouse_DragFinished(object? sender, WindowsInput.Events.Sources.EventSourceEventArgs<IReadOnlyList<WindowsInput.Events.DragDrop>> e)
        {
            Debug.WriteLine("Mouse_DragStarted finished");
            Mouse.MouseMove -= Mouse_MouseMove;
        }

        private void PictureBox1_Paint(object? sender, PaintEventArgs e)
        {
            Pen pen = new(Color.Gray, 3);

            //int startY = Start.Y > End.Y && Start.X < End.X ? pictureBox1.Bottom : pictureBox1.Top;
            //int endY = Start.Y > End.Y && Start.X < End.X ? pictureBox1.Top : pictureBox1.Bottom;
            int lenght = (int)Math.Sqrt(Math.Pow(Math.Abs(Start.X-End.X), 2) + Math.Pow(Math.Abs(Start.Y - End.Y), 2));

            e.Graphics.DrawLine(pen, Start.X, Start.Y, End.X, End.Y);

            e.Graphics.DrawString(lenght.ToString() + " px", Font, new SolidBrush(Color.Gray), (Start.X+End.X)/2, (Start.Y + End.Y) / 2 - 32);
        }

        private void Ruler_Disposed(object? sender, EventArgs e)
        {
            gr.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.FormBorderStyle = FormBorderStyle.None;
            Location = Point.Empty;
            Size = new(Screen.AllScreens.Sum(x => x.Bounds.Width), Screen.AllScreens.Sum(x => x.Bounds.Height));
            pictureBox1.Size = Size;
            gr = pictureBox1.CreateGraphics();
            Disposed += Ruler_Disposed;
            pictureBox1.Paint += PictureBox1_Paint;
        }

    }
}