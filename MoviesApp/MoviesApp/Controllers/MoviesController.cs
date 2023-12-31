﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Models.DTOs;
using MoviesApp.Models.Enum;
using System.Linq.Expressions;
using System.Reflection;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<MovieDto>> GetAll()
        {
            try
            {
                var moviesDb = StaticDb.Movies;
                return Ok(moviesDb.Select(x => new MovieDto
                {
                    Title = x.Title,
                    Description = x.Description,
                    Year = x.Year,
                    Genre = x.Genre,
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetByRouteId(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return BadRequest("Bad request, the id cannot be negative number!");
                }

                var moviesDto = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
                if(moviesDto == null)
                {
                    return NotFound($"Movie with id {id} was not found");
                }

                return Ok(new MovieDto
                {
                    Title = moviesDto.Title,
                    Description = moviesDto.Description,
                    Year = moviesDto.Year,
                    Genre = moviesDto.Genre,

                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpGet("getMovieWIthQueryString")]
        public ActionResult<MovieDto> GetMovieByIdWithQuery(int? id)
        {
            try
            {
                if(id == null)
                {
                    return BadRequest("The id is a required parameter!");
                }
                if(id <= 0)
                {
                    return BadRequest("Bad request, the id cannot be negative number!");
                }

                var moviesDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
                if(moviesDb == null)
                {
                    return NotFound($"Movie with id {id} was not found");
                }

                return Ok(new MovieDto
                {
                    Title = moviesDb.Title,
                    Description = moviesDb.Description,
                    Year = moviesDb.Year,
                    Genre = moviesDb.Genre,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("filter")]
        public ActionResult<List<MovieDto>> FilterMovies(int? genre, int? year)
        {
            try
            {
                if (genre == null && year == null)
                {
                    return BadRequest("You have to send at least one filter parameter");
                }
                if (genre.HasValue)
                {
                    var enumValues = Enum.GetValues(typeof(GenreEnum))
                        .Cast<GenreEnum>()
                        .Select(g => (int)g)
                        .ToList();
                    if (!enumValues.Contains(genre.Value))
                    {
                        return NotFound($"The genre with id {genre.Value} was not found");
                    }
                }
                if (genre == null)
                {
                    var moviesYear = StaticDb.Movies.Where(y => y.Year == year).ToList();

                    return Ok(moviesYear.Select(y => new MovieDto
                    {
                        Title = y.Title,
                        Description = y.Description,
                        Year = y.Year,
                        Genre = y.Genre,
                    }));
                }
                if (year == null)
                {
                    List<Movie> moviesGenre = StaticDb.Movies.Where(x => x.Genre == (GenreEnum)genre).ToList();
                    return Ok(moviesGenre.Select(y => new MovieDto
                    {
                        Title = y.Title,
                        Description = y.Description,
                        Year = y.Year,
                        Genre = y.Genre,
                    }));
                }

                List<Movie> movies = StaticDb.Movies.Where(x => x.Year == year && x.Genre == (GenreEnum)genre).ToList();
                return Ok(movies.Select(x => new MovieDto
                {
                    Title = x.Title,
                    Description = x.Description,
                    Year = x.Year,
                    Genre = x.Genre,
                }));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addMovie")]
        public IActionResult AddMovie([FromBody] AddMovieDto addMovieDto)
        {
            try
            {
                if (string.IsNullOrEmpty(addMovieDto.Title))
                {
                    return BadRequest("Title is required");
                }
                if(!string.IsNullOrEmpty(addMovieDto.Description) && addMovieDto.Description.Length > 250)
                {
                    return BadRequest("Description cannot be longer than 250 characters!");
                }
                if(addMovieDto.Year == null || addMovieDto.Year <= 0 || addMovieDto.Year > DateTime.Now.Year)
                {
                    return BadRequest("Invalid value for year");
                }
                if(addMovieDto.Genre == null)
                {
                    return BadRequest("Genre is required");
                }

                var enumValues = Enum.GetValues(typeof(GenreEnum))
                    .Cast<GenreEnum>()
                    .Select(g => (int)g)
                    .ToList();

                if (!enumValues.Contains((int)addMovieDto.Genre))
                {

                    return NotFound($"The genre with id {(int)addMovieDto.Genre} was not found");
                }

                Movie movie = new Movie()
                {
                    Id = StaticDb.Movies.Count + 1,
                    Title = addMovieDto.Title,
                    Description = addMovieDto.Description,
                    Genre = addMovieDto.Genre,
                    Year = addMovieDto.Year,
                };

                StaticDb.Movies.Add(movie);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateMovie([FromBody] UpdateMovieDto updateMovieDto)
        {
            try
            {
                Movie movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == updateMovieDto.Id);
                if(movieDb == null)
                {
                    return NotFound($"Movie with id {updateMovieDto.Id} was not found");
                }
                if (string.IsNullOrEmpty(updateMovieDto.Title))
                {
                    return BadRequest("Title cannot be empty!");
                }
                if (updateMovieDto.Year <= 0)
                {
                    return BadRequest("The value for year cannot be negative");
                }
                if(!string.IsNullOrEmpty(updateMovieDto.Description) && updateMovieDto.Description.Length > 250)
                {
                    return BadRequest("Description cannot be longer than 250 characters");
                }

                var enumValues = Enum.GetValues(typeof(GenreEnum))
                        .Cast<GenreEnum>()
                        .Select(g => (int)g)
                        .ToList();
                if (!enumValues.Contains((int)updateMovieDto.Genre))
                {
                    return NotFound($"The genre with id {(int)updateMovieDto.Genre} was not found");
                }

                movieDb.Title = updateMovieDto.Title;
                movieDb.Description = updateMovieDto.Description;
                movieDb.Year = updateMovieDto.Year;
                movieDb.Genre = updateMovieDto.Genre;

                return StatusCode(StatusCodes.Status204NoContent, "Movie updated!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteMovie([FromBody]int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Id cannot have negative value!");
                }

                var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
                if (movieDb == null)
                {
                    return NotFound("Movie was not found");
                }

                StaticDb.Movies.Remove(movieDb);
                return StatusCode(StatusCodes.Status204NoContent, "Movie was deleted");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovieById(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return BadRequest("Id cannot have negative value!");
                }

                var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
                if(movieDb == null)
                {
                    return NotFound("Movie was not found");
                }

                StaticDb.Movies.Remove(movieDb);
                return StatusCode(StatusCodes.Status204NoContent, "Movie was deleted");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
