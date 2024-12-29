using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Species
{
    public class Species : Entity<SpeciesId>
    {
        private readonly List<Breed> _breeds = new();

        public Species(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
        public IReadOnlyList<Breed> Breeds => _breeds;

        public static Result<Species> Create(string title) 
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result.Failure<Species>("Species title cannot be empty");
            }

            return Result.Success(new Species(title));
        }
    }
}
