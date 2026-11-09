namespace FinTrack.WebApi.IntegrationTests.Helpers;

public static class HttpResponseExtensions
{
    public static async Task<string> ReadBodyOrThrowAsync(this HttpResponseMessage response, string step)
    {
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"{step} fallo con {(int)response.StatusCode}. Cuerpo: {body}"
            );

        return body;
    }
}
