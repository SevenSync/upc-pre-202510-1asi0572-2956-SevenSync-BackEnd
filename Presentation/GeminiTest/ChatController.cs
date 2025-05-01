using Domain.GeminiTest;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.GeminiTest;

[ApiController]
[Route("api/chat")]
public class ChatController(
    IGenerativeAiService aiService
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string userText)
    {
        var aiReply = await aiService.GenerateAsync(userText);
        return Ok(new { reply = aiReply });
    }
}