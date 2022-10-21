#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Microservices/Microservices.TasksManagement/Microservices.TasksManagement.csproj", "src/Microservices/Microservices.TasksManagement/"]
COPY ["src/Application/Admin/Admin.csproj", "src/Application/Admin/"]
COPY ["src/Application/Common/Common.csproj", "src/Application/Common/"]
COPY ["src/Domain/Jira.Domain/Jira.Domain.csproj", "src/Domain/Jira.Domain/"]
COPY ["src/Application/Tasks/Tasks.csproj", "src/Application/Tasks/"]
COPY ["src/Infrastructure/Infrastructure.Jira.Supabase/Infrastructure.Jira.Supabase.csproj", "src/Infrastructure/Infrastructure.Jira.Supabase/"]
RUN dotnet restore "src/Microservices/Microservices.TasksManagement/Microservices.TasksManagement.csproj"
COPY . .
WORKDIR "/src/src/Microservices/Microservices.TasksManagement"
RUN dotnet build "Microservices.TasksManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.TasksManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.TasksManagement.dll"]