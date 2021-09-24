namespace MinesweeperAPI.Data
{
    public class BoardCellData
    {
        public int X { get; set; }

        public int Y { get; set; }

        public bool HasMine { get; set; }

        public string State { get; set; }
    }
}
