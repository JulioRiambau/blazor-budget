## create frontend project

```powershell
dotnet new blazorwasm -o frontend
```

## create backend project min api

```powershell
dotnet new webapi -o backend
```
## create backend project with controllers

```powershell
dotnet new webapi -o backend --use-controllers
```
## add swagger

update program.cs

```C#
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

...

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```
check at

https://localhost:<port>/swagger
[v1/swagger.json](https://localhost:<port>/swagger/v1/swagger.json)

client code can be created using NSwag

```powershell
dotnet add package Swashbuckle.AspNetCore
```

## check dotnet varsion

```powershell
dotnet --version
```

## build project

```powershell
dotnet build  
```

## run project and update on changes

```powershell
dotnet watch
```

## add ef core

dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design

## perform initial migrations

```powershell
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## create solution and add both projects
```powershell
dotnet new sln --name FullStackBudget
dotnet sln add backend/backend.csproj
dotnet sln add frontend/frontend.csproj
```

## setup mudblazor

https://mudblazor.com/getting-started/installation#manual-install-add-imports

```powershell
dotnet new install MudBlazor.Templates

dotnet add package MudBlazor
```

add @using MudBlazor in Imports.razor

add css and js in index.html