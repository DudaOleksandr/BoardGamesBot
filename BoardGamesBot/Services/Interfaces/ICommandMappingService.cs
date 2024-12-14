namespace BoardGamesBot.Services.Interfaces;

public interface ICommandMappingService
{
    Dictionary<string, string> GetCommandMappings();
}