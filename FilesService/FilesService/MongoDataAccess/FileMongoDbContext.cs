using FilesService.Core;
using MongoDB.Driver;

namespace FilesService.MongoDataAccess;

public class FileMongoDbContext(IMongoClient mongoClient)
{
    private const string FILES_COLLECTION = "files";
    
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("files_db");
    
    public IMongoCollection<FileData> Files => _database.GetCollection<FileData>(FILES_COLLECTION);
}