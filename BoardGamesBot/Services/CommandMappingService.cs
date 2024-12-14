using BoardGamesBot.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BoardGamesBot.Services;

public class CommandMappingService : ICommandMappingService
{
    private readonly Dictionary<string, string> _commandMappings;

    public CommandMappingService(IConfiguration configuration)
    {
        var jsonPath = configuration["CommandsJson"];
        _commandMappings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(jsonPath));
    }

    public Dictionary<string, string> GetCommandMappings()
    {
        return _commandMappings;
    }
}