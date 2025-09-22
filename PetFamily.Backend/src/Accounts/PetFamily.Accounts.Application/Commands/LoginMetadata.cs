namespace PetFamily.Accounts.Application.Commands;

public record LoginMetadata(
    string UserAgent,
    string IP,
    string Fingerprint);