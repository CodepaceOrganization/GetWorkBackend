using CodePace.GetWork.API.Profiles.Domain.Model.Commands;
using CodePace.GetWork.API.Profiles.Interfaces.REST.Resources;

namespace CodePace.GetWork.API.Profiles.Interfaces.REST.Transform;

public static class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
    {
        return new CreateProfileCommand(resource.FirstName, resource.LastName, resource.Email, resource.Street,
            resource.Number, resource.City, resource.PostalCode, resource.Country);
    }
}