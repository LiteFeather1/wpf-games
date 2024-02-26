
namespace Tetris.Source.Blocks
{
    public class OBlock : Block
    {
        private readonly Position[][] r_tiles =
        [
            [new(0, 0), new(0, 1), new(1, 0), new(1, 1)]
        ];

        protected override Position[][] Tiles => r_tiles;
        protected override Position StartOffSet => new (0, 4);
        public override int ID => 4;
    }
}
