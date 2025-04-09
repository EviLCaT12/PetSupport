namespace PetFamily.Framework.Authorization;

public static class Permissions
{
    public static class Species
    {
        public const string CreateSpecies = "species.create";
        public const string DeleteSpecies = "species.delete";
        public const string GetSpecies = "species.get";
        public const string CreateBreeds = "species.breeds.create";
        public const string DeleteBreeds = "species.breeds.delete";
        public const string GetBreeds = "species.breeds.get";
        
    }

    public static class Volunteers
    {
        public const string CreateVolunteer = "volunteer.create";
        public const string GetVolunteer = "volunteer.get";
        public const string DeleteVolunteer = "volunteer.delete";
        public const string UpdateMainInfoVolunteer = "volunteer.update.main.info";
        public const string CreatePet = "volunteers.pets.create";
        public const string UpdatePet = "volunteers.pets.update";
        public const string GetPet = "volunteer.pets.get";
        public const string DeletePet = "volunteer.pets.delete";
        public const string CreatePetPhoto = "volunteer.pets.photo.create";
        public const string DeletePetPhoto = "volunteer.pets.photo.delete";
        public const string SetMainPetPhoto = "volunteer.pets.main.photo.set";
        public const string RemoveMainPetPhoto = "volunteer.pets.main.photo.remove";
        public const string ChangePetPosition = "volunteer.pets.position.change";
        public const string ChangePetHelpStatus = "volunteer.pets.help.change";
    }

    public static class VolunteerRequests
    {
        public const string CreateVolunteerRequest = "volunteer.request.create";
        public const string TakeRequestOnReview = "volunteer.request.take.onReview";
        public const string RejectRequest = "volunteer.request.reject";
        public const string SendToRevision = "volunteer.request.revision";
    }
}