﻿using Application;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class MovieTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public MovieTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "movions_movies")
                .Options;
        }

        [Theory]
        [InlineData("Test description", 104, "2010-10-04", "Test title")]
        public async Task Create_ValidInput_ReturnsCorrectData(string description, int length, string releaseDate, string title)
        {
            // Arrange 
            var dbContext = new ApplicationDbContext(_dbContextOptions);

            await dbContext.Database.EnsureDeletedAsync();

            var movie = new AdminMovieModel
            {
                Description = description,
                Length = length,
                ReleaseDate = DateTime.Parse(releaseDate),
                Title = title
            };

            var appMovie = new Movie(dbContext);

            // Act
            var result = await appMovie.Create(movie);

            // Assert
            Assert.NotEqual(0, result.ID);
            Assert.Equal(description, result.Description);
            Assert.Equal(length, result.Length);
            Assert.Equal(DateTime.Parse(releaseDate), result.ReleaseDate);
            Assert.Equal(title, result.Title);
        }

        [Fact]
        public async Task ReadAll_ReturnsAllMovies()
        {
            // Arrange 
            var dbContext = new ApplicationDbContext(_dbContextOptions);

            dbContext.Movies.AddRange(
                Enumerable.Range(1, 5).Select(m => new Domain.Movie { ID = m, Description = $"Description {m}" })
            );

            await dbContext.SaveChangesAsync();

            var appMovie = new Movie(dbContext);

            // Act
            var result = appMovie.ReadAll();

            // Assert
            var movieModel = Assert.IsAssignableFrom<IEnumerable<MovieModel>>(result);
            Assert.Equal(5, movieModel.Count());
        }
    }
}
