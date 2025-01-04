
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class RequisitesList : ValueObject 
    {
        public IReadOnlyList<Requisite> Requisites { get;  }

        public RequisitesList() { }
        public RequisitesList(List<Requisite> requisites)
        {
            Requisites = requisites;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var requisite in Requisites)
            {
                yield return requisite;
            }
        }
    }
}
