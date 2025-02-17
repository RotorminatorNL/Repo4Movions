﻿using API;
using Application.AdminModels;
using Application.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    [Collection("Sequential")]
    public class GenreControllerTests : IntegrationTestSetup
    {
        public GenreControllerTests(ApiFactory<Startup> factory)
            : base(factory) { }

        [Theory]
        [InlineData("Name")]
        public async Task Create_ValidRequest_ReturnsJsonResponseAndCreated(string name)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();

            var newGenre = new AdminGenreModel
            {
                Name = name
            };

            var expectedGenre = new GenreModel
            {
                ID = 1,
                Name = name
            };
            #endregion

            #region Act
            var response = await client.PostAsJsonAsync("/api/genre", newGenre);
            var responseBody = await response.Content.ReadAsStreamAsync();
            var actualGenre = await JsonSerializer.DeserializeAsync<GenreModel>(responseBody);
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(expectedGenre.ID, actualGenre.ID);
            Assert.Equal(expectedGenre.Name, actualGenre.Name);
            #endregion
        }

        public static IEnumerable<object[]> Data_Create_InvalidRequest_ReturnsJsonResponseAndBadRequestWithErrors()
        {
            // Name = null
            yield return new object[]
            {
                null,
                new string[]
                {
                    "Name"
                },
                new string[]
                {
                    "Cannot be null or empty."
                }
            };
            // Name = empty
            yield return new object[]
            {
                "",
                new string[]
                {
                    "Name"
                },
                new string[]
                {
                    "Cannot be null or empty."
                }
            };
        }

        [Theory]
        [MemberData(nameof(Data_Create_InvalidRequest_ReturnsJsonResponseAndBadRequestWithErrors))]
        public async Task Create_InvalidRequest_ReturnsJsonResponseAndBadRequestWithErrors(string name, IEnumerable<string> expectedErrorNames, IEnumerable<string> expectedErrorValues)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();

            var newGenre = new AdminGenreModel
            {
                Name = name
            };
            #endregion

            #region Act
            var response = await client.PostAsJsonAsync("/api/genre", newGenre);
            var responseBody = await response.Content.ReadAsStreamAsync();
            var actualGenre = await JsonSerializer.DeserializeAsync<JsonElement>(responseBody);

            var errorProp = actualGenre.GetProperty("errors");
            var errors = errorProp.EnumerateObject();
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedErrorNames.Count(), errors.Count());
            Assert.All(expectedErrorNames, errorName => Assert.Contains(errorName, errors.Select(prop => prop.Name)));
            Assert.All(expectedErrorValues, errorValue => Assert.Contains(errorValue, errors.Select(prop => prop.Value[0].ToString())));
            #endregion
        }

        [Theory]
        [InlineData(1)]
        public async Task Read_ValidRequest_ReturnsJsonResponseAndOk(int id)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();
            var dbContext = GetDbContext();

            var genre = new Domain.Genre
            {
                Name = "Name"
            };
            dbContext.Genres.Add(genre);
            await dbContext.SaveChangesAsync();

            var expectedGenre = new GenreModel
            {
                ID = genre.ID,
                Name = genre.Name
            };
            #endregion

            #region Act
            var response = await client.GetAsync($"/api/genre/{id}");
            var responseBody = await response.Content.ReadAsStreamAsync();
            var actualGenre = await JsonSerializer.DeserializeAsync<GenreModel>(responseBody);
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedGenre.ID, actualGenre.ID);
            Assert.Equal(expectedGenre.Name, actualGenre.Name);
            #endregion
        }

        [Theory]
        [InlineData(1)]
        public async Task Read_InvalidRequest_ReturnsJsonResponseAndNotFound(int id)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();
            #endregion

            #region Act
            var response = await client.GetAsync($"/api/genre/{id}");
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            #endregion
        }

        [Fact]
        public async Task ReadAll_GenresExist_ReturnsJsonResponseAndOk()
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();
            var dbContext = GetDbContext();

            dbContext.Genres.Add(new Domain.Genre
            {
                Name = "Name"
            });
            dbContext.Genres.Add(new Domain.Genre
            {
                Name = "Some other name"
            });
            await dbContext.SaveChangesAsync();

            int expectedCount = 2;
            #endregion

            #region Act
            var response = await client.GetAsync("/api/genre");
            var responseBody = await response.Content.ReadAsStreamAsync();
            var actualGenres = await JsonSerializer.DeserializeAsync<IEnumerable<GenreModel>>(responseBody);
            #endregion

            #region Assert
            Assert.NotNull(actualGenres);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedCount, actualGenres.Count());
            #endregion
        }

        [Fact]
        public async Task ReadAll_NoGenresExist_ReturnsJsonResponseAndNoContent()
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();
            #endregion

            #region Act
            var response = await client.GetAsync("/api/genre");
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            #endregion
        }

        [Theory]
        [InlineData(1, "New Name")]
        public async Task Update_ValidRequest_ReturnsJsonResponseAndOk(int id, string name)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();
            var dbContext = GetDbContext();

            dbContext.Genres.Add(new Domain.Genre
            {
                Name = "Name"
            });
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
            #endregion

            #region Act
            var response = await client.PutAsJsonAsync($"/api/genre/{id}", newGenre);
            var responseBody = await response.Content.ReadAsStreamAsync();
            var actualGenre = await JsonSerializer.DeserializeAsync<GenreModel>(responseBody);
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedGenre.ID, actualGenre.ID);
            Assert.Equal(expectedGenre.Name, actualGenre.Name);
            #endregion
        }

        public static IEnumerable<object[]> Data_Update_InvalidRequest_ReturnsJsonResponseAndBadRequestWithErrors()
        {
            int id = 1;

            // Name = null
            yield return new object[]
            {
                id, null,
                new string[]
                {
                    "Name"
                },
                new string[]
                {
                    "Cannot be null or empty."
                }
            };
            // Name = empty
            yield return new object[]
            {
                id, "",
                new string[]
                {
                    "Name"
                },
                new string[]
                {
                    "Cannot be null or empty."
                }
            };
        }

        [Theory]
        [MemberData(nameof(Data_Update_InvalidRequest_ReturnsJsonResponseAndBadRequestWithErrors))]
        public async Task Update_InvalidRequest_ReturnsJsonResponseAndBadRequestWithErrors(int id, string name, IEnumerable<string> expectedErrorNames, IEnumerable<string> expectedErrorMessages)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();
            var dbContext = GetDbContext();

            dbContext.Genres.Add(new Domain.Genre
            {
                Name = "Name"
            });
            await dbContext.SaveChangesAsync();

            var newGenre = new AdminGenreModel
            {
                ID = id,
                Name = name
            };
            #endregion

            #region Act
            var response = await client.PutAsJsonAsync($"/api/genre/{id}", newGenre);
            var responseBody = await response.Content.ReadAsStreamAsync();
            var actualGenre = await JsonSerializer.DeserializeAsync<JsonElement>(responseBody);

            var errorProp = actualGenre.GetProperty("errors");
            var errors = errorProp.EnumerateObject();
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedErrorNames.Count(), errors.Count());
            Assert.All(expectedErrorNames, errorName => Assert.Contains(errorName, errors.Select(prop => prop.Name)));
            Assert.All(expectedErrorMessages, errorMessage => Assert.Contains(errorMessage, errors.Select(prop => prop.Value[0].ToString())));
            #endregion
        }

        [Theory]
        [InlineData(2)]
        public async Task Update_InvalidRequest_ReturnsJsonResponseAndNotFound(int id)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();

            var newGenre = new AdminGenreModel
            {
                ID = id,
                Name = "New Name"
            };
            #endregion

            #region Act
            var response = await client.PutAsJsonAsync($"/api/genre/{id}", newGenre);
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            #endregion
        }

        [Theory]
        [InlineData(1)]
        public async Task Delete_ValidRequest_ReturnsJsonResponseAndOk(int id)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();
            var dbContext = GetDbContext();

            dbContext.Genres.Add(new Domain.Genre());
            await dbContext.SaveChangesAsync();
            #endregion

            #region Act
            var response = await client.DeleteAsync($"/api/genre/{id}");
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            #endregion
        }

        [Theory]
        [InlineData(1)]
        public async Task Delete_InvalidRequest_ReturnsJsonResponseAndNotFound(int id)
        {
            #region Arrange 
            await DeleteDbContent();
            var client = GetHttpClient();
            #endregion

            #region Act
            var response = await client.DeleteAsync($"/api/genre/{id}");
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            #endregion
        }
    }
}
