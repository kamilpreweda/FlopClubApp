

using FlopClub.Dtos.Game;

namespace FlopClub
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateGameDto, Game>();
            CreateMap<Game, GetGameDto>();

            CreateMap<User, GetUserDto>()
                .ForMember(dest => dest.LobbyIds, opt => opt.MapFrom(src => src.Lobbies.Select(l => l.Id)));
            CreateMap<GetUserDto, User>();

            //CreateMap<Player, GetPlayerDto>();
            //CreateMap<AddPlayerDto, Player>();
        }
    }
}
