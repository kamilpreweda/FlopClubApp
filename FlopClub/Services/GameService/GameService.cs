using Microsoft.EntityFrameworkCore;

namespace FlopClub.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly DataContext _context;
        private readonly IEncrypter _encrypter;
        private readonly IMapper _mapper;

        public GameService(DataContext context, IEncrypter encrypter, IMapper mapper)
        {
            _context = context;
            _encrypter = encrypter;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetGameDto>> CreateGame(CreateGameDto newGame)
        {
            var response = new ServiceResponse<GetGameDto>();
            if(await GameExists(newGame.Name))
            {
                response.Success = false;
                response.Message = "Game already exists";
                return response;
            }

            _encrypter.CreatePasswordHash(newGame.Password, out byte[] passwordHash, out byte[] passwordSalt);
   
            var game = _mapper.Map<Game>(newGame);
            game.PasswordHash = passwordHash;
            game.PasswordSalt = passwordSalt;

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(game);
            return response;
        }

        public async Task<bool> GameExists(string gameName)
        {
            if (await _context.Games.AnyAsync(g => g.Name.ToLower() == gameName.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}
