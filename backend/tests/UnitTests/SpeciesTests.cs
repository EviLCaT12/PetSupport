using FluentAssertions;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace UnitTests;

public class SpeciesTests
{
    [Fact]
    public void GetBreedById_Should_ReturnBreed_When_BreedExists()
    {
        // Arrange
        var species = CreateSpecies();
        var breed = CreateBreed();
        species.AddBreeds(new List<Breed> { breed });

        // Act
        var result = species.GetBreedById(breed.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(breed);
    }
    
    [Fact]
    public void GetBreedById_Should_ReturnError_When_BreedNotFound()
    {
        // Arrange
        var species = CreateSpecies();
        var nonExistentBreedId = BreedId.Create(Guid.NewGuid()).Value;

        // Act
        var result = species.GetBreedById(nonExistentBreedId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void AddBreeds_Should_AddBreedsToSpecies_When_ValidDataProvided()
    {
        // Arrange
        var species = CreateSpecies();
        var breed1 = CreateBreed();
        var breed2 = CreateBreed();
        var breeds = new List<Breed> { breed1, breed2 };

        // Act
        species.AddBreeds(breeds);

        // Assert
        species.Breeds.Should().Contain(breeds);
    }

    [Fact]
    public void RemoveBreed_Should_RemoveBreedFromSpecies_When_BreedExists()
    {
        // Arrange
        var species = CreateSpecies();
        var breed = CreateBreed();
        species.AddBreeds(new List<Breed> { breed });

        // Act
        species.RemoveBreed(breed);

        // Assert
        species.Breeds.Should().NotContain(breed);
    }
    
    private Species CreateSpecies()
    {
        var id = SpeciesId.NewSpeciesId();
        var name = Name.Create("Test").Value;
        
        var species = Species.Create(id, name);
        return species.Value;
    }
    
    private Breed CreateBreed()
    {
        var id = BreedId.NewBreedId();
        var name = Name.Create("Test").Value;
        
        var breed = Breed.Create(id, name);
        return breed.Value;
    }
}