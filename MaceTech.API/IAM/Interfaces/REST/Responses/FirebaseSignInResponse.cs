namespace MaceTech.API.IAM.Interfaces.REST.Responses;

public record FirebaseSignInResponse(
    string IdToken,
    string RefreshToken,
    string LocalId,
    string ExpiresIn
);