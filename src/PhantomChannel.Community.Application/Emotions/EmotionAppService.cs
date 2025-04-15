using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using PhantomChannel.Community.Permissions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace PhantomChannel.Community.Emotions;

[Authorize(CommunityPermissions.Forums.Default)]
[Route("/api/app/emotions")]
public class EmotionAppService(IConfiguration configuration) : ApplicationService, IApplicationService
{

    private readonly IConfiguration _configuration = configuration;

    // [Authorize(CommunityPermissions.Forums.Default)]
    [AllowAnonymous]
    [HttpGet("judgment")]
    public async Task<string> JudgmentAsync([FromQuery] string text)
    {
        var apiKey = _configuration["DeepSeek:ApiKey"];
        var apiUrl = _configuration["DeepSeek:Url"];
        var model = _configuration["DeepSeek:Model"];


        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestData = new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = "请根据sentiment进行情感判断,如果是正面情感,请返回数字1,如果是负面情感,请返回数字0" },
                new { role = "user", content= text }
            },
            temperature = 1.0,
            max_tokens = 2048
        };

        var json = JsonSerializer.Serialize(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(apiUrl!, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        return responseJson;

    }

}
