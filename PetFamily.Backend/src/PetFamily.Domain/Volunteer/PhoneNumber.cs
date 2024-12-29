using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.Volunteer
{
    public class PhoneNumber
    {
        string Value { get;  }

        private PhoneNumber(string value) 
        { 
            Value = value;
        }

        public static Result<PhoneNumber> Create(string value)
        {
            string pattern = @"^\+?\d{1,4}?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";
            
            if(Regex.IsMatch(value, pattern))
            {
                return Result.Success<PhoneNumber>(new PhoneNumber(value));
            }

            return Result.Failure<PhoneNumber>("Phone number has incorrect format");
        }
    }
}
