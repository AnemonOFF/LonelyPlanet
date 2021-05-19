using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LonelyPlanet.Model;

namespace LonelyPlanet.View
{
    public partial class GameScreen : UserControl
    {
        private const int addBiomeDelta = 50;
        private const int removeBiomeDelta = 100;
        private readonly Renderer renderer;
        private readonly List<IBiome> biomes = new List<IBiome>();
        private UserControl loadingScreen;
        private int leftX;
        private int rightX;
        private bool isInitialized = false;
        private int idleCount = 0;
        private int fps = 0;
        private bool DevInfoFlag = false;
        private bool fpsCheckFlag = false;

        public readonly Game game = new Game();
        public Size PlayerSize { get; } = new Size(40, 80);
        public Size BlockSize { get; } = new Size(40, 40);

        public GameScreen()
        {
            InitializeComponent();
            DoubleBuffered = true;

            ShowLoadingScreen();

            renderer = new Renderer(BlockSize);
            var firstBiome = game.map[0];
            renderer.RenderBiome(firstBiome);
            biomes.Add(firstBiome);
            leftX = firstBiome.LeftX;
            rightX = firstBiome.LeftX + firstBiome.Length;
            isInitialized = true;
            var a = CheckForBiomesRelevance();
            a.ContinueWith((t) => Controls.Remove(loadingScreen), TaskScheduler.FromCurrentSynchronizationContext());
            Application.Idle += new EventHandler(HandleApplicationIdle);

            var fpsTimer = new Timer { Interval = 1000 };
            fpsTimer.Tick += (s, args) => {
                fps = idleCount;
                idleCount = 0;
            };
            fpsTimer.Start();
            /*var updateTimer = new Timer {
                Interval = 100,
            };
            updateTimer.Tick += (sender, args) => Invalidate();
            updateTimer.Start();*/
        }

        private void ShowLoadingScreen()
        {
            loadingScreen = new LoadingScreen
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
                Name = "loadingScreen"
            };
            Controls.Add(loadingScreen);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        bool IsApplicationIdle()
        {
            return PeekMessage(out _, IntPtr.Zero, 0, 0, 0) == 0;
        }

        private void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                idleCount++;
                //var g = CreateGraphics();
                //OnPaint(new PaintEventArgs(g, new Rectangle(0, 0, Width, Height)));
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            //var bmp = new Bitmap(Width, Height);
            //var graphics = Graphics.FromImage(bmp);
            DrawBiomes(graphics);
            DrawPlayer(graphics);
            DrawGUI(graphics);
            if (DevInfoFlag)
                DrawDevInfo(graphics);
            //g.DrawImage(bmp, 0, 0, Width, Height);
            //bmp.Dispose();
        }
        
        private void DrawDevInfo(Graphics graphics)
        {
            var playerPosition = game.player.GetPosition();
            graphics.DrawString(
                    "X: " + playerPosition.X + "\nY: " + playerPosition.Y + "\nFPS: " + fps,
                    new Font("Arial", 16),
                    Brushes.Black,
                    new Point(0, 0)
                    );
            graphics.FillRectangle(fpsCheckFlag ? Brushes.Red : Brushes.Green, Width - 20, 10, 10, 10);
            fpsCheckFlag = !fpsCheckFlag;
        }

        protected override void OnResize(EventArgs e)
        {
            CheckForBiomesRelevance();
        }

        private void DrawBiomes(Graphics graphics)
        {
            var playerPosition = game.player.GetPosition();
            lock (biomes)
            {
                foreach (var biome in biomes)
                {
                    if (biome.NeedToRender || biome.Render is null)
                        renderer.RenderBiome(biome);
                    lock (biome.Render)
                    {
                        graphics.DrawImage(
                            biome.Render,
                            (Width - PlayerSize.Width) / 2 - (int)((playerPosition.X - biome.LeftX) * BlockSize.Width),
                            (Height - PlayerSize.Height) / 2 - (int)(((Map.chunkSize - 1) - playerPosition.Y) * BlockSize.Height));
                    }
                }
            }
        }

        private void DrawPlayer(Graphics graphics)
        {
            graphics.DrawImage(
                GameSprites.Player,
                (Width - PlayerSize.Width) / 2,
                (Height - PlayerSize.Height) / 2,
                PlayerSize.Width, PlayerSize.Height
                );
        }

        private async Task CheckForBiomesRelevance()
        {
            if (!isInitialized)
                return;
            var playerPosition = game.player.GetPosition();
            var halfScreenBlocksAmount = (Width - PlayerSize.Width) / 2 / BlockSize.Width;
            while (playerPosition.X - halfScreenBlocksAmount - leftX < addBiomeDelta)
                await AddNextBiome(Direction.left);
            while (rightX - (playerPosition.X + halfScreenBlocksAmount) < addBiomeDelta)
                await AddNextBiome(Direction.right);
        }

        private Task AddNextBiome(Direction direction)
        {
            var task = new Task(() =>
            {
                IBiome newBiome;
                if (direction == Direction.left)
                {
                    newBiome = game.map.GetBiomeByX(leftX - 1);
                    leftX = newBiome.LeftX;
                }
                else if (direction == Direction.right)
                {
                    newBiome = game.map.GetBiomeByX(rightX + 1);
                    rightX = newBiome.LeftX + newBiome.Length;
                }
                else
                    throw new ArgumentException("Biome can be added only on left or right side");
                if (newBiome.Render is null)
                    renderer.RenderBiome(newBiome);
                lock (biomes)
                {
                    biomes.Add(newBiome);
                }
            });
            task.Start();
            return task;
        }
    }
}
