namespace Tetris.Source
{
    public class BlockQueue
    {
        private readonly Block[] r_blocks =
        [
            Block.IBlock,
            Block.JBlock,
            Block.LBlock,
            Block.OBlock,
            Block.SBlock,
            Block.TBlock,
            Block.ZBlock
        ];

        private readonly Random r_random = new();

        // TODO maybe preview next 3 block instead of just 1
        public Block NextBlock { get; private set; }

        public BlockQueue() => NextBlock = RandomBlock();

        public Block GetAndUpdate()
        {
            var block = NextBlock;
            do
                NextBlock = RandomBlock();
            while (block.ID == NextBlock.ID);

            return block;
        }

        private Block RandomBlock()
            => r_blocks[r_random.Next(r_blocks.Length)];
    }
}
