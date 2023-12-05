using Infrustructure.ErrorHandling.Exceptions.Media;
using Infrustructure.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Infrustructure.Services.Media;

public abstract class MediaServiceBase
{
    private readonly MediaServiceOptions _options;
    private readonly string _mediaPath;
    
    protected MediaServiceBase(IOptions<MediaServiceOptions> options, IHostingEnvironment environment)
    {
        _options = options.Value;
        _mediaPath = Path.Combine(environment.WebRootPath, _options.MediaFolder);
    }
    
    protected Task SaveMedia(byte[] bytes, string path, string name)
    {
        var filePath = Path.Combine(_mediaPath, path, name);
        
        return File.WriteAllBytesAsync(filePath, bytes);
    }

    protected bool MediaExists(string path, string name)
    {
        var filePath = Path.Combine(_mediaPath, path, name);

        return File.Exists(filePath);
    }

    protected string GetMediaUrl(string path, string name)
    {
        var filePath = Path.Combine(_mediaPath, path, name);

        if (!MediaExists(path, name)) throw new MediaNotFoundException(filePath);

        return filePath;
    }
}