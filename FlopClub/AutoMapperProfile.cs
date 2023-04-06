

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
                .ForMember(dest => dest.LobbyIds, opt => opt.MapFrom(src => src.Lobbies.Select(l => l.Id)))
                .ForMember(dto => dto.Roles, opt => opt.MapFrom(src => src.UserRoles!.Select(ur => ur.Role).ToList()));
            CreateMap<GetUserDto, User>();

            CreateMap<Role, GetRoleDto>();

            //CreateMap<Player, GetPlayerDto>();
            //CreateMap<AddPlayerDto, Player>();
        }
    }
}
