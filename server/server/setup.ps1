#Once you have installed .NET and ASP.NET, download our master branch
#Navigate into server folder, and run this script

#compile the solution
dotnet build

#TESTS
#compile tests
cd ../..
cd PluggedIn_Tests
dotnet build
#run tests
dotnet test PluggedIn_Tests.sln
#get back to home folder
cd ..
cd server/server

#make a migration (code to build the database)
dotnet ef migrations add InitialCreate

#run the migrations (builds the database)
dotnet ef database update

#make sql scripts (for reference on how the database was built)
dotnet ef migrations script > plugged-database.sql

#launch locally
dotnet run