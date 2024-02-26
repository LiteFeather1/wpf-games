
namespace Tetris.Source.Blocks
{
    public class JBlock : Block
    {
        private readonly Position[][] r_tiles =
        [
            [new(0, 0), new(1, 0), new(1, 1), new(1, 2)],
            [new(0, 1), new(0, 2), new(1, 1), new(2, 1)],
            [new(1, 0), new(1, 1), new(1, 2), new(2, 2)],
            [new(0, 1), new(1, 1), new(2, 0), new(2, 1)]
        ];

        protected override Position[][] Tiles => r_tiles;
        protected override Position StartOffSet => new(0, 3);
        public override int ID => 2;
    }
}
