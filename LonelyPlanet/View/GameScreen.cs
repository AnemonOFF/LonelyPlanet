using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LonelyPlanet.Model;

namespace LonelyPlanet.View
{
    public partial class GameScreen : UserControl
    {
        private const int addBiomeDelta = 10;
        private const int removeBiomeDelta = 40;
        private readonly Renderer renderer;
        private readonly List<IBiome> biomes = new List<IBiome>();
        private UserControl loadingScreen;
        private int leftX;
        private int rightX;
        private bool isInitialized = false;

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
            biomes.Add(firstBiome);
            leftX = firstBiome.LeftX;
            rightX = firstBiome.LeftX + firstBiome.Length;
            isInitialized = true;
            CheckForBiomesRelevance();

            var updateTimer = new Timer {
                Interval = 16,
            };
            updateTimer.Tick += (sender, args) => Invalidate();
            updateTimer.Start();
            Controls.Remove(loadingScreen);
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
            var playerPosition = game.player.GetPosition();
            DrawBiomes(graphics);
            DrawPlayer(graphics);
            graphics.DrawString(
                "X: " + playerPosition.X + "\nY: " + playerPosition.Y,
                new Font("Arial", 16),
                Brushes.Black,
                new Point(0, 0)
                );
        }

        protected override void OnResize(EventArgs e)
        {
            CheckForBiomesRelevance();
        }

        private void DrawBiomes(Graphics graphics)
        {
            var playerPosition = game.player.GetPosition();
            foreach (var biome in biomes)
            {
                if (biome.Render is null)
                    renderer.RenderBiome(biome);
                graphics.DrawImage(
                    biome.Render,
                    (Width - PlayerSize.Width) / 2 - (int)((playerPosition.X - biome.LeftX) * BlockSize.Width),
                    (Height - PlayerSize.Height) / 2 - (int)(((Map.chunkSize - 1) - playerPosition.Y) * BlockSize.Height));
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

        private void CheckForBiomesRelevance()
        {
            if (!isInitialized)
                return;
            var playerPosition = game.player.GetPosition();
            var halfScreenBlocksAmount = (Width - PlayerSize.Width) / 2 / BlockSize.Width;
            while (playerPosition.X - halfScreenBlocksAmount - leftX < addBiomeDelta)
                AddNextBiome(Direction.left);
            while (rightX - playerPosition.X - halfScreenBlocksAmount < addBiomeDelta)
                AddNextBiome(Direction.right);
        }

        private void AddNextBiome(Direction direction)
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
            biomes.Add(newBiome);
        }
    }
}
