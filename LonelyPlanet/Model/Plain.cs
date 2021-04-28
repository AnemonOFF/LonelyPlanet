namespace LonelyPlanet.Model
{
    class Plain : IBiome
    {
        private readonly Chunk[] chunks;
        private readonly int averageHeight;

        public int Length { get; }
        public int LeftX { get; }
        public string Name { get; } = "Plain";

        public Chunk this[int index]
        {
            get
            {
                return chunks[index];
            }
        }

        public Plain(Chunk referance, int x, int length)
        {
            LeftX = x;
            Length = length;
            averageHeight = referance.Height;
            chunks = new Chunk[Length];
            chunks[0] = GenerateChunk(referance, 0);
            for (var i = 1; i < Length; i++)
                chunks[i] = GenerateChunk(chunks[i - 1], LeftX + i);
        }

        private Chunk GenerateChunk(Chunk referance, int x)
        {
            var height = GetRandomChunkHeight(referance.Height);
            var blocks = new IBlock[height];
            for (var i = 0; i < height - 10; i++)
                blocks[i] = Block.GetRandomUndergroundBlock(x, i);
            for (var i = height - 10; i < height; i++)
                blocks[i] = new Rock(x, i);
            return new Chunk(x, height, blocks);
        }

        private int GetRandomChunkHeight(int referanceHeight)
        {
            if (Map.randomGenerator.NextDouble() > 0.1)
                return referanceHeight;
            if (Map.randomGenerator.NextDouble() > 0.5 && referanceHeight + 1 < averageHeight + 5)
                return referanceHeight + 1;
            if (referanceHeight - 1 > averageHeight - 5)
                return referanceHeight - 1;
            return referanceHeight;
        }
    }
}
