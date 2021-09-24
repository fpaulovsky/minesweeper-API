using AutoMapper;
using MinesweeperAPI.Data;
using MinesweeperAPI.Dtos;
using MinesweeperAPI.Model;

namespace MinesweeperAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>();

            CreateMap<BoardCell, BoardCellData>();

            CreateMap<Game, GameDto>()
                .ForMember(
                    dto => dto.Board,
                    opt => 
                    {
                        opt.PreCondition(g => g.Board != null);
                        opt.MapFrom(src =>
                            new BoardDto
                            {
                                Cells = src.Board.GetFlaggedOrUncoveredCells(),
                                Width = src.Board.Width,
                                Height = src.Board.Height
                            });
                    });
        }
    }
}
