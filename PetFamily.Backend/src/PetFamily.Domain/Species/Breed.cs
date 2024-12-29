using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Species
{
    public class Breed : Entity<BreedId>
    {
        private Breed(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }

        public static Result<Breed> Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result.Failure<Breed>("Breed title cannot be empty");
            }

            return Result.Success(new Breed(title));
        }
    }
}
