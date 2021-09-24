using AutoMapper;
using MinesweeperAPI.Data;
using MinesweeperAPI.Model;
using System.Linq;
using System.Text.Json;

namespace MinesweeperAPI
{
    public class BoardJsonSerializer
    {
        private readonly IMapper _mapper;

        public BoardJsonSerializer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string Serialize(Board board)
        {
            var cells = board.Cells.Where(c => c.HasMine || c.State != BoardCellState.Covered);
            return JsonSerializer.Serialize(new
            {
                Cells = cells.Select(c => _mapper.Map<BoardCell, BoardCellData>(c)),
                Width = board.Width,
                Height = board.Height
            });
        }

        public Board Deserialize(string boardJson)
        {
            var boardData = JsonSerializer.Deserialize<BoardData>(boardJson);
            
            var board = new Board(boardData.Width, boardData.Height);
            foreach (var cellData in boardData.Cells)
            {
                var cellCoordinate = new BoardCellCoordinate(cellData.X, cellData.Y);
                if (cellData.HasMine)
                {
                    board.TrySetMineOnCell(cellCoordinate);
                }
                else
                {
                    var cell = board.GetCell(cellCoordinate);
                    cell.State = cellData.State;
                }
            }
            
            return board;
        }
    }
}
