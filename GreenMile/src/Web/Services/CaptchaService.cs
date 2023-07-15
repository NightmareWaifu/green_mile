using Newtonsoft.Json.Linq;
using System.Net;
using Web.Utils;

namespace Web.Services
{
    public class CaptchaService: ICaptchaService
    {
        private const string SECRET_KEY = "6LeHUwckAAAAAOOAdYLPLT4GACzKBzkQMnYEAKD9";
        private const decimal CAPTCHA_THRESHOLD_SCORE = 0.5m;
        public const string SITE_KEY = "6LeHUwckAAAAAA0wOUeJAjtDczI5kGeF-C_usJxy";

        public async Task<Result<bool>> CaptchaPassed(string captchaResponse)
        {
            HttpClient httpClient = new HttpClient();

            var res = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={SECRET_KEY}&response={captchaResponse}");

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return Result<bool>.Success("User is not verified", false);
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= CAPTCHA_THRESHOLD_SCORE)
            {
                return Result<bool>.Success("User is not verified", false);
            }

            return Result<bool>.Success("User is verified", true);
        }
    }
}
