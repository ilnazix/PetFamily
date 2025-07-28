namespace PetFamily.Volunteers.Contracts.Requests
{
    public record UpdateRequisitesRequest
    {
        public IEnumerable<RequisiteDto> Requisites { get; }

        public UpdateRequisitesRequest(IEnumerable<RequisiteDto> requisites)
        {
            Requisites = requisites;
        }
    }
}
