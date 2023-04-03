

using FlopClub.Dtos.Game;

namespace FlopClub
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateGameDto, Game>();
            CreateMap<Game, GetGameDto>();
            //CreateMap<Player, GetPlayerDto>();
            //CreateMap<AddPlayerDto, Player>();
        }
    }
}
