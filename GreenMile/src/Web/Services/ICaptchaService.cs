using Web.Utils;

namespace Web.Services
{
    public interface ICaptchaService
    {
        public Task<Result<bool>> CaptchaPassed(string captchaResponse);
    }
}
