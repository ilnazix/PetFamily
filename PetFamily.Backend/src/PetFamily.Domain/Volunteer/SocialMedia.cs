using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Volunteer
{
    public record SocialMedia
    {
        string Link { get; }
        string Title { get; }

        private SocialMedia(string link, string title)
        {
            Link = link;
            Title = title;
        }

        public static Result<SocialMedia> Create(string link, string title)
        {
            string errors = string.Empty;

            if (string.IsNullOrWhiteSpace(link))
            {
                errors += "Link cannot be empty\n";
            }
            
            if (string.IsNullOrWhiteSpace(title))
            {
                errors += "Title cannot be empty\n";
            }

            if (string.IsNullOrWhiteSpace(errors))
            {
                return Result.Success(new SocialMedia(link, title));
            }

            return Result.Failure<SocialMedia>(errors);
        }
    }
}
