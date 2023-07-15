using Microsoft.AspNetCore.SignalR;

namespace Web.Services
{
    public class OpenAIHub : Hub
    {
        private readonly OpenAIApiService _openAIApiService;
        public OpenAIHub(OpenAIApiService openAIApiService) { 
            _openAIApiService = openAIApiService;
        }

        public async IAsyncEnumerable<string> QueryGPT(string query)
        {
          await foreach (var completedToken in _openAIApiService.GenerateDavinciPromptStream(query))
            {
                if(completedToken.Successful)
                {
                    yield return completedToken.Choices.FirstOrDefault()?.Text;
                } else
                {
                    if(completedToken.Error is null)
                    {
                        throw new Exception("Internal server error");
                    }
                    yield return $"{completedToken.Error.Code} {completedToken.Error.Message}";
                }
            }
        }
        
    }
}
