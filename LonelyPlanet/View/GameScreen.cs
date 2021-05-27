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
using System.Windows.Media;
using LonelyPlanet.Model;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;

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
        private int idleCount = 0;
        private int fps = 0;
        private int soundsVolume;
        private readonly Dictionary<string, int> currentAnimationSteps = new Dictionary<string, int>();
        private readonly Dictionary<string, Bitmap> currentBitmaps = new Dictionary<string, Bitmap>();
        private readonly Dictionary<string, Direction> previousMovingDirection = new Dictionary<string, Direction>();
        private bool devInfoFlag = false;
        private bool fpsCheckFlag = false;
        private bool isInitialized = false;

        public readonly Game game = new Game();
        public Size PlayerSize { get; } = new Size(40, 80);
        public Size BlockSize { get; } = new Size(40, 40);

        public GameScreen(int soundsVolume)
        {
            InitializeComponent();
            DoubleBuffered = true;

            ShowLoadingScreen();

            this.soundsVolume = soundsVolume;
            game.player.JumpEvent += (e) => PlaySound(new Uri(@"sounds\sfx\jump.wav", UriKind.Relative));
            game.player.FallEvent += (e) => PlaySound(new Uri(@"sounds\sfx\fall.wav", UriKind.Relative));
            game.player.DamageEvent += (e) => PlaySound(new Uri(@"sounds\sfx\damage2.wav", UriKind.Relative));
            game.player.BlockPlace += (e) => PlaySound(new Uri(@"sounds\sfx\Place.wav", UriKind.Relative));
            game.player.BlockDestroy += (e) => PlaySound(new Uri(@"sounds\sfx\Place.wav", UriKind.Relative));

            renderer = new Renderer(BlockSize);
            var firstBiome = game.map[0];
            renderer.RenderBiome(firstBiome);
            biomes.Add(firstBiome);
            leftX = firstBiome.LeftX;
            rightX = firstBiome.LeftX + firstBiome.Length;
            currentAnimationSteps.Add("Player", 0);
            currentBitmaps.Add("Player", GameSprites.PlayerStaying[0]);
            previousMovingDirection.Add("Player", Direction.None);
            var animationFrameUpdater = new Timer { Interval = 100 };
            animationFrameUpdater.Tick += (s, e) => UpdateAnimationFrames();
            animationFrameUpdater.Start();
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
        }

        public void PlaySound(Uri sound)
        {
            var player = new MediaPlayer();
            player.Volume = soundsVolume / 100.0;
            player.Open(sound);
            player.Play();
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

        private bool IsApplicationIdle()
        {
            return PeekMessage(out _, IntPtr.Zero, 0, 0, 0) == 0;
        }

        private void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                idleCount++;
                Invalidate();
            }
        }

        private void UpdateAnimationFrames()
        {
            var needToFlip = game.player.MovingDirection == Direction.Left;
            if (game.player.IsFalling)
                SetAnimationFrame("Player", GameSprites.PlayerFalling, Direction.Down, needToFlip);
            else if (game.player.IsFlying)
                SetAnimationFrame("Player", GameSprites.PlayerFlying, Direction.Up, needToFlip);
            else if (game.player.IsMoving)
                SetAnimationFrame("Player", GameSprites.PlayerMoving, Direction.Horizontal, needToFlip);
            else
                SetAnimationFrame("Player", GameSprites.PlayerStaying, Direction.None, needToFlip);
        }

        private void SetAnimationFrame(string name, Bitmap[] frames, Direction currentDirection, bool needToFlip = false)
        {
            if (previousMovingDirection[name] != currentDirection)
            {
                currentAnimationSteps[name] = 0;
                previousMovingDirection[name] = currentDirection;
            }
            else if (currentAnimationSteps[name] >= frames.Length)
                currentAnimationSteps[name] = 0;
            var oldBitmap = frames[currentAnimationSteps[name]];
            var newBitmap = oldBitmap.Clone(new Rectangle(0, 0, oldBitmap.Width, oldBitmap.Height), oldBitmap.PixelFormat);
            if (needToFlip)
                newBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            currentBitmaps[name] = newBitmap;
            currentAnimationSteps[name]++;
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

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            DrawBiomes(graphics);
            DrawPlayer(graphics);
            DrawEntities(graphics);
            DrawGUI(graphics);
            if (devInfoFlag)
                DrawDevInfo(graphics);
        }

        private void DrawEntities(Graphics graphics)
        {
            var playerPosition = game.player.GetPosition();
            foreach(var entity in game.map.entities)
            {
                var entityPosition = entity.GetPosition();
                var x = (Width - PlayerSize.Width) / 2 - (int)((playerPosition.X - entityPosition.X) * BlockSize.Width - (BlockSize.Width - entity.Texture.Width) / 2);
                var y = (Height - PlayerSize.Height) / 2 - (int)((entityPosition.Y - playerPosition.Y) * BlockSize.Height - BlockSize.Height - entity.Texture.Height);
                graphics.DrawImage(entity.Texture, x, y);
            }
        }
        
        private void DrawDevInfo(Graphics graphics)
        {
            var playerPosition = game.player.GetPosition();
            graphics.DrawString(
                    "X: " + playerPosition.X + "\nY: " + playerPosition.Y + "\nFPS: " + fps,
                    new Font("Arial", 16),
                    Brushes.Black,
                    new Point(10, 104)
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
            var halfScreenBlocksAmount = (Width - PlayerSize.Width) / 2 / BlockSize.Width;
            lock (biomes)
            {
                foreach (var biome in biomes)
                {
                    var isBiomeOnScreen = !((biome.LeftX < playerPosition.X - halfScreenBlocksAmount
                        && biome.LeftX + biome.Length < playerPosition.X - halfScreenBlocksAmount)
                        || (biome.LeftX > playerPosition.X + halfScreenBlocksAmount));
                    if (isBiomeOnScreen && (biome.NeedToRender || biome.Render is null))
                        renderer.RenderBiome(biome);
                    lock (biome.Render)
                    {
                        graphics.DrawImage(
                            biome.Render,
                            (Width - PlayerSize.Width) / 2 - (int)((playerPosition.X - biome.LeftX) * BlockSize.Width),
                            (Height - PlayerSize.Height) / 2 - (int)((Map.chunkSize - 1 - playerPosition.Y) * BlockSize.Height));
                    }
                }
            }
        }

        private void DrawPlayer(Graphics graphics)
        {
            graphics.DrawImage(
                currentBitmaps["Player"],
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
                await AddNextBiome(Direction.Left);
            while (rightX - (playerPosition.X + halfScreenBlocksAmount) < addBiomeDelta)
                await AddNextBiome(Direction.Right);
        }

        private Task AddNextBiome(Direction direction)
        {
            var task = new Task(() =>
            {
                IBiome newBiome;
                if (direction == Direction.Left)
                {
                    newBiome = game.map.GetBiomeByX(leftX - 1);
                    leftX = newBiome.LeftX;
                }
                else if (direction == Direction.Right)
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
