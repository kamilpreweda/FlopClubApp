﻿namespace FlopClub.Services.GameService
{
    public interface IGameService
    {
        Task<ServiceResponse<GetGameDto>> CreateGame(CreateGameDto newGame);
        Task<bool> GameExists(string gameName);
    }
}