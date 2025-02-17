﻿using Application.AdminModels;
using Application.Validation;
using Application.ViewModels;
using Microsoft.EntityFrameworkCore;
using PersistenceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public class CrewMember
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly CrewMemberValidation _crewMemberValidation;

        private CrewMemberModel GetCrewMemberModelWithErrorID(object movie, object person)
        {
            if (movie == null && person != null)
            {
                return new CrewMemberModel
                {
                    ID = -1
                };
            }

            if (movie != null && person == null)
            {
                return new CrewMemberModel
                {
                    ID = -2
                };
            }

            return new CrewMemberModel
            {
                ID = -3
            };
        }

        public CrewMember(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _crewMemberValidation = new CrewMemberValidation();
        }

        public async Task<CrewMemberModel> Create(AdminCrewMemberModel adminCrewMemberModel)
        {
            if (_crewMemberValidation.IsInputValid(adminCrewMemberModel))
            {
                var movie = _applicationDbContext.Movies.FirstOrDefault(m => m.ID == adminCrewMemberModel.MovieID);
                var person = _applicationDbContext.Persons.FirstOrDefault(p => p.ID == adminCrewMemberModel.PersonID);

                if (movie != null && person != null)
                {
                    var crewMember = new Domain.CrewMember
                    {
                        CharacterName = adminCrewMemberModel.CharacterName,
                        Role = adminCrewMemberModel.Role,
                        MovieID = adminCrewMemberModel.MovieID,
                        PersonID = adminCrewMemberModel.PersonID
                    };

                    _applicationDbContext.CrewMembers.Add(crewMember);
                    await _applicationDbContext.SaveChangesAsync();

                    return await Read(crewMember.ID);
                }

                return GetCrewMemberModelWithErrorID(movie, person);

            }

            return null;
        }

        public async Task<CrewMemberModel> Read(int id)
        {
            return await _applicationDbContext.CrewMembers.Select(c => new CrewMemberModel
            {
                ID = c.ID,
                CharacterName = c.CharacterName,
                Role = c.Role.ToString(),
                Movie = new MovieModel 
                { 
                    ID = c.Movie.ID,
                    Name = c.Movie.Name
                },
                Person = new PersonModel
                {
                    ID = c.Person.ID,
                    FirstName = c.Person.FirstName,
                    LastName = c.Person.LastName
                }
            }).FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<IEnumerable<CrewMemberModel>> ReadAll()
        {
            return await _applicationDbContext.CrewMembers.Select(c => new CrewMemberModel
            {
                ID = c.ID,
                CharacterName = c.CharacterName,
                Role = c.Role.ToString(),
                Movie = new MovieModel
                {
                    ID = c.Movie.ID,
                    Name = c.Movie.Name
                },
                Person = new PersonModel
                {
                    ID = c.Person.ID,
                    FirstName = c.Person.FirstName,
                    LastName = c.Person.LastName
                }
            }).ToListAsync();
        }

        public async Task<CrewMemberModel> Update(AdminCrewMemberModel adminCrewMemberModel)
        {
            var crewMember = _applicationDbContext.CrewMembers.FirstOrDefault(x => x.ID == adminCrewMemberModel.ID);

            if (crewMember != null && _crewMemberValidation.IsInputValid(adminCrewMemberModel))
            {
                var movie = _applicationDbContext.Movies.FirstOrDefault(m => m.ID == adminCrewMemberModel.MovieID);
                var person = _applicationDbContext.Persons.FirstOrDefault(p => p.ID == adminCrewMemberModel.PersonID);

                if (movie != null && person != null)
                {
                    crewMember.CharacterName = adminCrewMemberModel.CharacterName;
                    crewMember.Role = adminCrewMemberModel.Role;
                    crewMember.MovieID = adminCrewMemberModel.MovieID;
                    crewMember.PersonID = adminCrewMemberModel.PersonID;

                    await _applicationDbContext.SaveChangesAsync();

                    return await Read(crewMember.ID);
                }

                return GetCrewMemberModelWithErrorID(movie, person);
            }

            return null;
        }

        public async Task<bool> Delete(int id)
        {
            var crewMember = _applicationDbContext.CrewMembers.FirstOrDefault(x => x.ID == id);

            if (crewMember != null)
            {
                _applicationDbContext.CrewMembers.Remove(crewMember);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
