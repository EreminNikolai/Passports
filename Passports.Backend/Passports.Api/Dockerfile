FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Passports.Api/Passports.Api.csproj", "Passports.Api/"]
RUN dotnet restore "Passports.Api/Passports.Api.csproj"
COPY . .
WORKDIR "/src/Passports.Api"
RUN dotnet build "Passports.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Passports.Api.csproj" -c Release -o /app/publish
VOLUME /home/logs

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Passports.Api.dll"]
