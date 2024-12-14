namespace BoardGamesBot.Interfaces;

public interface ICommandMappingService
{
    Dictionary<string, string> GetCommandMappings();
}