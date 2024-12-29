using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Volunteer
{
    public record Requisite
    {
        string Title { get;  } = string.Empty;
        string Description { get; } = string.Empty;

        private Requisite(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public static Result<Requisite> Create(string title, string description)
        {
            string errors = string.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                errors += "Название реквизита не может быть пустым\n";
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                errors += "Описание реквизита не может быть пустым\n";
            }

            if (string.IsNullOrEmpty(errors))
            {
                return Result.Success<Requisite>(new Requisite(title, description));
            }

            return Result.Failure<Requisite>(errors);
        }
    }
}
