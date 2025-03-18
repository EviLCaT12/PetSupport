using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PetFamily.Core.Files;
using PetFamily.Core.Providers;
using PetFamily.SharedKernel.Error;
using FileInfo = PetFamily.Core.Files.FileInfo;


namespace IntegrationTests;

public class TestsWebFactory : IntegrationTestsWebFactory
{
    private readonly IFileProvider _fileProviderMock = Substitute.For<IFileProvider>();

    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);
        var fileProvider = services.FirstOrDefault(x => x.ServiceType == typeof(IFileProvider));
        if (fileProvider != null)
            services.Remove(fileProvider);
        
        services.AddScoped<IFileProvider>(_ => _fileProviderMock);
    }

    public void SetupSuccessUploadFilesAsync()
    {
        _fileProviderMock
            .UploadFilesAsync(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<ErrorList>());
    }
    
    public void SetupFailedUploadFilesAsync()
    {
        var error = Error.Failure("test.error", "UploadFilesAsync is failed");
        
        _fileProviderMock
            .UploadFilesAsync(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
            .Returns(new ErrorList([error]));
    }
    
    public void SetupSuccessRemoveFilesAsync()
    {
        _fileProviderMock
            .RemoveFilesAsync(Arg.Any<IEnumerable<FileInfo>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<IEnumerable<string>, ErrorList>([]));
    }
    
    public void SetupFailedRemoveFilesAsync()
    {
        var error = Error.Failure("test.error", "RemoveFilesAsync is failed");
        
        _fileProviderMock
            .UploadFilesAsync(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
            .Returns(new ErrorList([error]));
    }
    
    public void SetupSuccessGetFilePresignedUrl()
    {
        _fileProviderMock
            .GetFilePresignedUrl(Arg.Any<FileInfo>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<string, ErrorList>(String.Empty));
    }
    
    public void SetupFailedGetFilePresignedUrl()
    {
        var error = Error.Failure("test.error", "GetFilePresignedUrl is failed");
        
        _fileProviderMock
            .UploadFilesAsync(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
            .Returns(new ErrorList([error]));
    }
}