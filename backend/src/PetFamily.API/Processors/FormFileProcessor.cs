using PetFamily.Application.Dto.PetDto;

namespace PetFamily.API.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<CreatePhotoDto> _photos = [];

    public List<CreatePhotoDto> Process(IFormFileCollection photos)
    {
        foreach (var photo in photos)
        {
            var stream = photo.OpenReadStream();
            var photoDto = new CreatePhotoDto(stream, photo.FileName);
            _photos.Add(photoDto);
        }
        return _photos;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var photo in _photos)
        {
            await photo.Stream.DisposeAsync();
        }
    }
}