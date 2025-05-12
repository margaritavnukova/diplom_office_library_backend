using NUnit.Framework;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;

[TestFixture]
public class GenresDtoTests
{
    [Test]
    public void Constructor_WithValidGenre_SetsPropertiesCorrectly()
    {
        // Arrange
        var genre = new Genre_Dictionary
        {
            Id = "1",
            Name = "Science Fiction",
            Description = "Sci-fi genre description"
        };

        // Act
        var dto = new GenresDto(genre);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(dto.Id, Is.EqualTo(genre.Id), "Id should match");
            Assert.That(dto.Name, Is.EqualTo(genre.Name), "Name should match");
            Assert.That(dto.Description, Is.EqualTo(genre.Description), "Description should match");
        });
    }

    [Test]
    public void Constructor_WithNullProperties_SetsNullValues()
    {
        // Arrange
        var genre = new Genre_Dictionary
        {
            Id = "2",
            Name = null,
            Description = null
        };

        // Act
        var dto = new GenresDto(genre);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(dto.Id, Is.EqualTo("2"), "Id should match");
            Assert.That(dto.Name, Is.Null, "Name should be null");
            Assert.That(dto.Description, Is.Null, "Description should be null");
        });
    }

    [Test]
    public void Constructor_WithEmptyStrings_SetsEmptyValues()
    {
        // Arrange
        var genre = new Genre_Dictionary
        {
            Id = "3",
            Name = "",
            Description = string.Empty
        };

        // Act
        var dto = new GenresDto(genre);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(dto.Id, Is.EqualTo("3"), "Id should match");
            Assert.That(dto.Name, Is.Empty, "Name should be empty");
            Assert.That(dto.Description, Is.Empty, "Description should be empty");
        });
    }
}