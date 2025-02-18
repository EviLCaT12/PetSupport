using System.Text.Json;
using System.Text.Json.Serialization;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Infrastructure.Converters;

public class PetPhotoConverter : JsonConverter<PetPhoto>
{
    public override PetPhoto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            var pathToStorage = root.GetProperty("PathToStorage").GetProperty("Path").GetString();
            var filePath = FilePath.Create(pathToStorage!, null).Value;
            return PetPhoto.Create(filePath).Value;
        }
    }

    public override void Write(Utf8JsonWriter writer, PetPhoto value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("PathToStorage");
        writer.WriteStartObject();
        writer.WriteString("Path", value.PathToStorage.Path);
        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}

public class FilePathConverter : JsonConverter<FilePath>
{
    public override FilePath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var path = reader.GetString();
        return FilePath.Create(path!, null).Value;
    }

    public override void Write(Utf8JsonWriter writer, FilePath value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Path);
    }
}