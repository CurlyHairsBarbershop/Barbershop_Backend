using Infrustructure.Options;
using Infrustructure.Services;
using Infrustructure.Services.Media;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace BLL.Services.Users.Barbers;

public class BarberMediaService : MediaServiceBase
{
    private const string PATH = "profiles/barbers/images";
    
    public BarberMediaService(IOptions<MediaServiceOptions> options, IHostingEnvironment environment)
        : base(options, environment) { }

    public Task SaveProfileImage(byte[] bytes, string mediaName) =>
        SaveMedia(bytes, path: PATH, name: mediaName);

    public bool MediaExists(string mediaName) => MediaExists(PATH, mediaName);

    public string GetUrl(string mediaName) => GetMediaUrl(PATH, mediaName);
}