using AutoMapper;
using MinesweeperAPI;
using MinesweeperAPI.Model;
using Xunit;

namespace MinesweeperAPITests
{
    public class BoardJsonSerializerTests
    {
        [Fact]
        public void DeserializeAfterSerializeReturnsSameBoard()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            var mapper = mapperConfig.CreateMapper();
            
            var serializer = new BoardJsonSerializer(mapper);

            var board = new Board(2, 2);
            board.TrySetMineOnCell(new BoardCellCoordinate(0, 0));
            board.TrySetMineOnCell(new BoardCellCoordinate(1, 1));

            var json = serializer.Serialize(board);
            var deserializedBoard = serializer.Deserialize(json);
            var json2 = serializer.Serialize(deserializedBoard);

            Assert.True(json.Equals(json2));
            Assert.True(board.Equals(deserializedBoard));
        }
    }
}
