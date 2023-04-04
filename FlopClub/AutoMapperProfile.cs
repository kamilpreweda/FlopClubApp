

using FlopClub.Dtos.Game;

namespace FlopClub
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateGameDto, Game>();
            CreateMap<Game, GetGameDto>();

            CreateMap<User, GetUserDto>();
            CreateMap<GetUserDto, User>();


            //CreateMap<Player, GetPlayerDto>();
            //CreateMap<AddPlayerDto, Player>();
        }
    }
}
