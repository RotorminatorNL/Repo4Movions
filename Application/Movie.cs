﻿using Application.AdminModels;
using Application.Validation;
using Application.ViewModels;
using Microsoft.EntityFrameworkCore;
using PersistenceInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class Movie
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly MovieValidation _movieValidation;

        private MovieModel GetMovieModelWithErrorID(object genre, object movie)
        {
            if (genre == null && movie != null)
            {
                return new MovieModel
                {
                    ID = -1
                };
            }

            if (genre != null && movie == null)
            {
                return new MovieModel
                {
                    ID = -2
                };
            }

            if (genre == null && movie == null)
            {
                return new MovieModel
                {
                    ID = -3
                };
            }

            return new MovieModel
            {
                ID = -4
            };
        }

        public Movie(IApplicationDbContext applicationDbContext)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("nl-NL");

            _applicationDbContext = applicationDbContext;
            _movieValidation = new MovieValidation();
        }

        public async Task<MovieModel> Create(AdminMovieModel adminMovieModel)
        {
            if (_movieValidation.IsInputValid(adminMovieModel))
            {
                var language = await _applicationDbContext.Languages.FirstOrDefaultAsync(x => x.ID == adminMovieModel.LanguageID);

                if (language != null)
                {
                    var movie = new Domain.Movie
                    {
                        Description = adminMovieModel.Description,
                        LanguageID = adminMovieModel.LanguageID,
                        Length = adminMovieModel.Length,
                        ReleaseDate = adminMovieModel.ReleaseDate.ToString("dd-MM-yyyy"),
                        Name = adminMovieModel.Name
                    };

                    _applicationDbContext.Movies.Add(movie);
                    await _applicationDbContext.SaveChangesAsync();

                    return await Read(movie.ID);
                }

                return new MovieModel();
            }

            return null;
        }

        public async Task<MovieModel> ConnectGenre(AdminGenreMovieModel adminGenreMovieModel)
        {
            var genre = await _applicationDbContext.Genres.FirstOrDefaultAsync(x => x.ID == adminGenreMovieModel.GenreID);
            var movie = await _applicationDbContext.Movies.FirstOrDefaultAsync(x => x.ID == adminGenreMovieModel.MovieID);

            if (genre != null && movie != null)
            {
                var doesConnectionExist = await _applicationDbContext.GenreMovies.FirstOrDefaultAsync(x => x.GenreID == genre.ID && x.MovieID == movie.ID);

                if (doesConnectionExist == null)
                {
                    var genreMovie = new Domain.GenreMovie
                    {
                        GenreID = adminGenreMovieModel.GenreID,
                        MovieID = adminGenreMovieModel.MovieID
                    };

                    _applicationDbContext.GenreMovies.Add(genreMovie);
                    await _applicationDbContext.SaveChangesAsync();

                    return await Read(adminGenreMovieModel.MovieID);
                }

                return GetMovieModelWithErrorID(genre, movie);
            }

            return GetMovieModelWithErrorID(genre, movie);
        }

        public async Task<MovieModel> Read(int id)
        {
            return await _applicationDbContext.Movies.Select(movie => new MovieModel
            {
                ID = movie.ID,
                Description = movie.Description,
                Genres = movie.Genres.Select(genre => new GenreModel 
                { 
                    ID = genre.Genre.ID,
                    Name = genre.Genre.Name
                }),
                Language = new LanguageModel
                {
                    ID = movie.Language.ID,
                    Name = movie.Language.Name
                },
                Length = movie.Length,
                ReleaseDate = movie.ReleaseDate,
                Name = movie.Name
            }).FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<IEnumerable<MovieModel>> ReadAll()
        {
            return await _applicationDbContext.Movies.Select(movie => new MovieModel
            {
                ID = movie.ID,
                Description = movie.Description,
                Language = new LanguageModel
                {
                    ID = movie.Language.ID,
                    Name = movie.Language.Name
                },
                Length = movie.Length,
                ReleaseDate = movie.ReleaseDate,
                Name = movie.Name
            }).ToListAsync();
        }

        public async Task<MovieModel> Update(AdminMovieModel adminMovieModel)
        {
            var movie = _applicationDbContext.Movies.FirstOrDefault(x => x.ID == adminMovieModel.ID);

            if (movie != null && _movieValidation.IsInputValid(adminMovieModel))
            {
                var language = await _applicationDbContext.Languages.FirstOrDefaultAsync(x => x.ID == adminMovieModel.LanguageID);

                if (language != null)
                {
                    movie.Description = adminMovieModel.Description;
                    movie.Length = adminMovieModel.Length;
                    movie.LanguageID = adminMovieModel.LanguageID;
                    movie.ReleaseDate = adminMovieModel.ReleaseDate.ToString("dd-MM-yyyy");
                    movie.Name = adminMovieModel.Name;

                    await _applicationDbContext.SaveChangesAsync();

                    return await Read(movie.ID);
                }

                return new MovieModel();
            }

            return null;
        }

        public async Task<bool> Delete(int id)
        {
            var movie = _applicationDbContext.Movies.FirstOrDefault(x => x.ID == id);

            if (movie != null)
            {
                _applicationDbContext.Movies.Remove(movie);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<MovieModel> DisconnectGenre(AdminGenreMovieModel adminGenreMovieModel)
        {
            var genre = await _applicationDbContext.Genres.FirstOrDefaultAsync(c => c.ID == adminGenreMovieModel.GenreID);
            var movie = await _applicationDbContext.Movies.FirstOrDefaultAsync(c => c.ID == adminGenreMovieModel.MovieID);

            if (genre != null && movie != null)
            {
                var genreMovie = _applicationDbContext
                                    .GenreMovies
                                    .FirstOrDefault(c => c.GenreID == genre.ID && c.MovieID == movie.ID);

                if (genreMovie != null)
                {
                    _applicationDbContext.GenreMovies.Remove(genreMovie);
                    await _applicationDbContext.SaveChangesAsync();

                    return new MovieModel();
                }
            }

            return GetMovieModelWithErrorID(genre, movie);
        }
    }
}
