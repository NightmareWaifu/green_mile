using OpenAI.GPT3.Interfaces;


namespace Web.Services
{
    public class OpenAIApiService
    {
        private readonly IOpenAIService _openAIService;
        public OpenAIApiService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
            _openAIService.SetDefaultModelId(OpenAI.GPT3.ObjectModels.Models.TextDavinciV3);
        }

        public async Task<string> GenerateImage(string prompt)
        {
           
            if(prompt is null)
            {
                throw new ArgumentNullException(nameof(prompt));
            }
            var imageResult = await _openAIService.Image.CreateImage(new OpenAI.GPT3.ObjectModels.RequestModels.ImageCreateRequest
            {
                Prompt = prompt,
                N = 1,
                Size = OpenAI.GPT3.ObjectModels.StaticValues.ImageStatics.Size.Size512,
                ResponseFormat = OpenAI.GPT3.ObjectModels.StaticValues.ImageStatics.ResponseFormat.Url,
                User = "TestUser"
            });



            if (imageResult.Successful)
            {
                return string.Join("\n", imageResult.Results[0].Url);
            }
            throw new InvalidProgramException(nameof(prompt));

        }

        public async IAsyncEnumerable<OpenAI.GPT3.ObjectModels.ResponseModels.CompletionCreateResponse> GenerateDavinciPromptStream(string prompt)
        {
            if (prompt is null)
            {
                throw new ArgumentNullException(nameof(prompt));
            }
        
           await foreach (var token in _openAIService.Completions.CreateCompletionAsStream(new OpenAI.GPT3.ObjectModels.RequestModels.CompletionCreateRequest()
            {
                Prompt = prompt,
                MaxTokens = 1000,
                Stream = true,
                User = "TestUser",

            })) {

                yield return token;

            }
            
         
        }

        public async Task<OpenAI.GPT3.ObjectModels.ResponseModels.CompletionCreateResponse> GenerateDavinciPrompt(string prompt)
        {
            if (prompt is null)
            {
                throw new ArgumentNullException(nameof(prompt));
            }
            return await _openAIService.Completions.CreateCompletion(new OpenAI.GPT3.ObjectModels.RequestModels.CompletionCreateRequest()
            {
                Prompt = prompt,
                MaxTokens = 1000,

                User = "TestUser",

            });

        }
    }
}
