using AutoMapper;
using MinesweeperAPI.Dtos;
using MinesweeperAPI.Model;

namespace MinesweeperAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameDto>()
                .ForMember(dto => dto.Board, o => o.MapFrom(src => src.Board.GetPlayerView()));
        }
    }
}
