﻿using Application;
using Application.AdminModels;
using Application.ViewModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class GenreTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public GenreTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "movions_genre")
                .Options;
        }

        [Theory]
        [InlineData("Name")]
        public async Task Create_ValidInput_ReturnsCorrectData(string name)
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            var newGenre = new AdminGenreModel
            {
                Name = name
            };

            var expectedGenre = new GenreModel
            {
                ID = 1,
                Name = name
            };

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var actualGenre = await appGenre.Create(newGenre);
            #endregion

            #region Assert
            Assert.Equal(expectedGenre.ID, actualGenre.ID);
            Assert.Equal(expectedGenre.Name, actualGenre.Name);
            #endregion
        }

        public static IEnumerable<object[]> Data_Create_InvalidInput_ReturnNull()
        {
            // name = null
            yield return new object[] { null };
            // name = empty
            yield return new object[] { "" };
        }

        [Theory]
        [MemberData(nameof(Data_Create_InvalidInput_ReturnNull))]
        public async Task Create_InvalidInput_ReturnNull(string name)
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            await dbContext.SaveChangesAsync();

            var newGenre = new AdminGenreModel
            {
                Name = name
            };

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var actualGenre = await appGenre.Create(newGenre);
            #endregion

            #region Assert
            Assert.Null(actualGenre);
            #endregion
        }

        [Theory]
        [InlineData(1)]
        public async Task Read_ValidInput_ReturnsCorrectData(int id)
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            var genre = new Domain.Genre
            {
                Name = "Name"
            };
            dbContext.Genres.Add(genre);
            await dbContext.SaveChangesAsync();

            var expectedGenre = new GenreModel
            {
                ID = id,
                Name = genre.Name
            };

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var actualGenre = await appGenre.Read(id);
            #endregion

            #region Assert
            Assert.Equal(expectedGenre.ID, actualGenre.ID);
            Assert.Equal(expectedGenre.Name, actualGenre.Name);
            #endregion
        }

        [Theory]
        [InlineData(1)]
        public async Task Read_InvalidInput_ReturnsNull(int id)
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var actualGenre = await appGenre.Read(id);
            #endregion

            #region Assert
            Assert.Null(actualGenre);
            #endregion
        }

        [Fact]
        public async Task ReadAll_GenresExist_ReturnsAllGenres()
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            int expectedAmount = 2;

            dbContext.Genres.AddRange(
                Enumerable.Range(1, expectedAmount).Select(x => new Domain.Genre
                {
                    ID = x,
                    Name = $"Name {x}"
                })
            );

            await dbContext.SaveChangesAsync();

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var result = await appGenre.ReadAll();
            #endregion

            #region Assert
            var actualAmount = Assert.IsAssignableFrom<IEnumerable<GenreModel>>(result).Count();
            Assert.Equal(expectedAmount, actualAmount);
            #endregion
        }

        [Fact]
        public async Task ReadAll_NoGenresExist_ReturnsEmptyList()
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            int expectedAmount = 0;

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var result = await appGenre.ReadAll();
            #endregion

            #region Assert
            var actualAmount = Assert.IsAssignableFrom<IEnumerable<GenreModel>>(result).Count();
            Assert.Equal(expectedAmount, actualAmount);
            #endregion
        }

        [Theory]
        [InlineData(1, "New Name")]
        public async Task Update_ValidInput_ReturnsCorrectData(int id, string name)
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            var genre = new Domain.Genre
            {
                Name = "Name"
            };
            dbContext.Genres.Add(genre);

            await dbContext.SaveChangesAsync();

            var newGenre = new AdminGenreModel
            {
                ID = id,
                Name = name
            };

            var expectedGenre = new GenreModel
            {
                ID = id,
                Name = name
            };

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var actualGenre = await appGenre.Update(newGenre);
            #endregion

            #region Assert
            Assert.Equal(expectedGenre.ID, actualGenre.ID);
            Assert.Equal(expectedGenre.Name, actualGenre.Name);
            #endregion
        }

        public static IEnumerable<object[]> Data_Update_InvalidInput_ReturnsNull()
        {
            int id = 1;
            string name = "New Name";

            // id = 0
            yield return new object[] { 0, name };
            // id = 2 (does not exist)
            yield return new object[] { 2, name };
            // name = null
            yield return new object[] { id, null };
            // name = empty
            yield return new object[] { id, "" };
        }

        [Theory]
        [MemberData(nameof(Data_Update_InvalidInput_ReturnsNull))]
        public async Task Update_InvalidInput_ReturnsNull(int id, string name)
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            var genre = new Domain.Genre
            {
                Name = "Name"
            };
            dbContext.Genres.Add(genre);

            await dbContext.SaveChangesAsync();

            var newGenre = new AdminGenreModel
            {
                ID = id,
                Name = name
            };

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var actualGenre = await appGenre.Update(newGenre);
            #endregion

            #region Assert
            Assert.Null(actualGenre);
            #endregion
        }

        [Theory]
        [InlineData(1)]
        public async Task Delete_ValidInput_ReturnsTrue(int id)
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            dbContext.Genres.Add(new Domain.Genre());
            await dbContext.SaveChangesAsync();

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var actual = await appGenre.Delete(id);
            #endregion

            #region Assert
            Assert.True(actual);
            #endregion
        }

        [Theory]
        [InlineData(1)]
        public async Task Delete_InvalidInput_ReturnsFalse(int id)
        {
            #region Arrange
            var dbContext = new ApplicationDbContext(_dbContextOptions);
            await dbContext.Database.EnsureDeletedAsync();

            var appGenre = new Genre(dbContext);
            #endregion

            #region Act
            var actual = await appGenre.Delete(id);
            #endregion

            #region Assert
            Assert.False(actual);
            #endregion
        }
    }
}
