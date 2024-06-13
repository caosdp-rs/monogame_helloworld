using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ScoreClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseAddress;

    public ScoreClient(string baseAddress)
    {
        _httpClient = new HttpClient();
        _baseAddress = baseAddress.TrimEnd('/') + "/api/scores"; // Garantir que a URL esteja formatada corretamente
    }

    public async Task<bool> SendScoreAsync(string player, int points)
    {
        try
        {
            var score = new { Player = player, Points = points };
            var response = await _httpClient.PostAsync(_baseAddress, new StringContent(JsonSerializer.Serialize(score), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                // Tratar outros códigos de status se necessário
                Console.WriteLine($"Erro ao enviar score. Status code: {response.StatusCode}");
                return false;
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Erro de requisição HTTP: {e.Message}");
            return false;
        }
    }
}
