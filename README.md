# movions-backend
The backend/API for movions

# Available API calls:
## Company
### Create company:
  - expects: ```{ name: string, type: int }```
  - httpcode: 200  :white_check_mark:
  - method: POST
  - url: /api/company
  - returns: company

### Connect movie:
  - expects: ```{ movieId: int }```
  - httpcode: 200  :white_check_mark:
  - method: POST
  - url: /api/company/{id}/movies
  - returns: company

### Get companies: 
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/company
  - returns: list of companies

### Get company:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/company/{id}
  - returns: company

### Update company:
  - expects: ```{ id: int, name: string, type: int }```
  - httpcode: 200  :white_check_mark:
  - method: PUT
  - url: /api/company/{id}
  - returns: company

### Delete company:
  - httpcode: 200  :white_check_mark:
  - method: DELETE
  - url: /api/company/{id}
  - returns: nothing

### Disconnect movie:
  - httpcode: 200  :white_check_mark:
  - method: DELETE
  - url: /api/company/{id}/movies/{movieId}
  - returns: nothing

--------------------------------------------

## Crew member
### Create crew member:
  - expects: ```{ characterName: string, role: int, movieId: int, personId: int }```
  - httpcode: 200  :white_check_mark:
  - method: POST
  - url: /api/crewmember
  - returns: crew member


### Get crew members:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/crewmember
  - returns: list of crew members

### Get crew member:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/crewmember/{id}
  - returns: crew member

### Update crew member:
  - expects: ```{ id: int, characterName: string, role: int, movieId: int, personId: int }```
  - httpcode: 200  :white_check_mark:
  - method: PUT
  - url: /api/crewmember/{id}
  - returns: crew member

### Delete crew member:
  - httpcode: 200  :white_check_mark:
  - method: DELETE
  - url: /api/crewmember/{id}
  - returns: nothing


--------------------------------------------

## Genre
### Create genre:
  - expects: ```{ name: string }```
  - httpcode: 200  :white_check_mark:
  - method: POST
  - url: /api/genre
  - returns: genre

### Get genres:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/genre
  - returns: list of genres

### Get genre:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/genre/{id}
  - returns: genre

### Update genre:
  - expects: ```{ id: int, name: string }```
  - httpcode: 200  :white_check_mark:
  - method: PUT
  - url: /api/genre/{id}
  - returns: genre

### Delete genre:
  - httpcode: 200  :white_check_mark:
  - method: DELETE
  - url: /api/genre/{id}
  - returns: nothing

--------------------------------------------

## Language
### Create language:
  - expects: ```{ name: string }```
  - httpcode: 200  :white_check_mark:
  - method: POST
  - url: /api/language
  - returns: language

### Get languages:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/language
  - returns: list of languages

### Get language:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/language/{id}
  - returns: language

### Update language:
  - expects: ```{ id: int, name: string }```
  - httpcode: 200  :white_check_mark:
  - method: PUT
  - url: /api/language/{id}
  - returns: language

### Delete language:
  - httpcode: 200  :white_check_mark:
  - method: DELETE
  - url: /api/language/{id}
  - returns: nothing

--------------------------------------------

## Movie
### Create movie:
  - expects: ```{ description: string, languageId: int, length: int, name: string, releaseDate: date }```
  - httpcode: 200  :white_check_mark:
  - method: POST
  - url: /api/movie
  - returns: movie

### Connect genre:
  - expects: ```{ genreId: int }```
  - httpcode: 200  :white_check_mark:
  - method: POST
  - url: /api/movie/{id}/genres
  - returns: movie

### Get movies:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/movie
  - returns: list of movies

### Get movie:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/movie/{id}
  - returns: movie

### Update movie:
  - expects: ```{ id: int, description: string, languageId: int, length: int, name: string, releaseDate: date }```
  - httpcode: 200  :white_check_mark:
  - method: PUT
  - url: /api/movie/{id}
  - returns: movie

### Delete movie:
  - httpcode: 200  :white_check_mark:
  - method: DELETE
  - url: /api/movie/{id}
  - returns: nothing

### Disconnect genre:
  - httpcode: 200  :white_check_mark:
  - method: DELETE
  - url: /api/movie/{id}/genres{genreId}
  - returns: nothing

--------------------------------------------

## Person
### Create person:
  - expects: ```{ birthDate: date, birthPlace: string, description: string, firstName: string, lastName: string }```
  - httpcode: 200  :white_check_mark:
  - method: POST
  - url: /api/person
  - returns: person

### Get persons:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/person
  - returns: list of persons

### Get person:
  - httpcode: 200  :white_check_mark:
  - method: GET
  - url: /api/person/{id}
  - returns: person

### Update person:
  - expects: ```{ id: int, birthDate: date, birthPlace: string, description: string, firstName: string, lastName: string }```
  - httpcode: 200  :white_check_mark:
  - method: PUT
  - url: /api/person/{id}
  - returns: person

### Delete person:
  - httpcode: 200  :white_check_mark:
  - method: DELETE
  - url: /api/person/{id}
  - returns: nothing
