using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;

using static System.Net.Mime.MediaTypeNames;

namespace Web.Services
{
    public class GoogleAIService
    {

        public async Task<Google.Cloud.Vision.V1.Image> ConvertFileStreamToImage(IFormFile file)
        {

            byte[] imageBytes;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                imageBytes = stream.ToArray();
            }

            return Google.Cloud.Vision.V1.Image.FromBytes(imageBytes);

            

        }
        public async Task<string> IdentifyObject(Google.Cloud.Vision.V1.Image image)
        {
            ImageAnnotatorClient client = await ImageAnnotatorClient.CreateAsync();
            IReadOnlyList<LocalizedObjectAnnotation> annotations =await client.DetectLocalizedObjectsAsync(image);
            foreach (LocalizedObjectAnnotation annotation in annotations)
            {
                string poly = string.Join(" - ", annotation.BoundingPoly.NormalizedVertices.Select(v => $"({v.X}, {v.Y})"));
                return annotation.Name;
            }
            return "Unknown";
        }
    }
}
