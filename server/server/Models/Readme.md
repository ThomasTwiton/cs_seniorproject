# Database

## Migrations

When there are changes to the database in the form of changes to the Tables.cs file, you need to create a new migration to update the database.

Begin by going to Tools >> NuGet Package Manager >> Package Manager Console. Then, run the following commands:
```bash
Add-Migration [filename-of-choice]
Update-Database
```
When `Update-Database` is run, NuGet applies the migration file with the most recent timestamp. 
