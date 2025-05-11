FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DeletarContatos.Api/DeletarContatos.Api.csproj", "DeletarContatos.Api/"]
COPY ["DeletarContatos.Domain/DeletarContatos.Domain.csproj", "DeletarContatos.Domain/"]
COPY ["DeletarContatos.Infrastructure/DeletarContatos.Infrastructure.csproj", "DeletarContatos.Infrastructure/"]
COPY ["DeletarContatos.Service/DeletarContatos.Service.csproj", "DeletarContatos.Service/"]
RUN dotnet restore "DeletarContatos.Api/DeletarContatos.Api.csproj"
COPY . .
WORKDIR "/src/DeletarContatos.Api"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "DeletarContatos.Api.dll"]
