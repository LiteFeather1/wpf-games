
namespace Tetris.Source
{
    public class IBlock : Block
    {
        private readonly Position[][] r_tiles =
        [
            [new (1, 0), new(1, 1), new(1, 2), new(1, 3)],
            [new (0, 2), new(1, 2), new(2, 2), new(3, 2)],
            [new (2, 0), new(2, 1), new(2, 2), new(2, 3)],
            [new (0, 1), new(1, 1), new(2, 1), new(3, 1)]
        ];

        protected override Position[][] Tiles => r_tiles;
        public override int ID => 1;
        protected override Position StartOffSet => new(-1, 3);
    }
}
