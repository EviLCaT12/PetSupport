docker-compose up -d

dotnet-ef database drop -f -c WriteDbContext -p .\src\Volunteers\src\PetFamily.Volunteers.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef database drop -f -c WriteDbContext -p .\src\Species\src\PetFamily.Species.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef database drop -f -c WriteAccountsDbContext -p .\src\Accounts\src\PetFamily.Accounts.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef database drop -f -c WriteContext -p .\src\VolunteerRequest\src\PetFamily.VolunteerRequest.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef database drop -f -c WriteDbContext -p .\src\Discussion\src\PetFamily.Discussion.Infrastructure\ -s .\src\PetFamily.Web\


dotnet-ef migrations remove -c WriteDbContext -p .\src\Volunteers\src\PetFamily.Volunteers.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef migrations remove -c WriteDbContext -p .\src\Species\src\PetFamily.Species.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef migrations remove -c WriteAccountsDbContext -p .\src\Accounts\src\PetFamily.Accounts.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef migrations remove -c WriteContext -p .\src\VolunteerRequest\src\PetFamily.VolunteerRequest.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef migrations remove -c WriteDbContext -p .\src\Discussion\src\PetFamily.Discussion.Infrastructure\ -s .\src\PetFamily.Web\

dotnet-ef migrations add Volunteers_Init -c WriteDbContext -p .\src\Volunteers\src\PetFamily.Volunteers.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef migrations add Species_Init -c WriteDbContext -p .\src\Species\src\PetFamily.Species.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef migrations add Accounts_Init -c WriteAccountsDbContext -p .\src\Accounts\src\PetFamily.Accounts.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef migrations add VolunteerRequest_Init -c WriteContext -p .\src\VolunteerRequest\src\PetFamily.VolunteerRequest.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef migrations add Discussion_Init -c WriteDbContext -p .\src\Discussion\src\PetFamily.Discussion.Infrastructure\ -s .\src\PetFamily.Web\


dotnet-ef database update -c WriteDbContext -p .\src\Volunteers\src\PetFamily.Volunteers.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef database update -c WriteDbContext -p .\src\Species\src\PetFamily.Species.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef database update -c WriteAccountsDbContext -p .\src\Accounts\src\PetFamily.Accounts.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef database update -c WriteContext -p .\src\VolunteerRequest\src\PetFamily.VolunteerRequest.Infrastructure\ -s .\src\PetFamily.Web\
dotnet-ef database update -c WriteDbContext -p .\src\Discussion\src\PetFamily.Discussion.Infrastructure\ -s .\src\PetFamily.Web\

pause