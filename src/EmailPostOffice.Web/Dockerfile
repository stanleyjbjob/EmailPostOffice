FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ConnectionStrings:Default="Server=192.168.1.12;Database=EmailPostOffice;User ID=jb;Password=^Hsx9bu5t@"
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/EmailPostOffice.Web/EmailPostOffice.Web.csproj", "src/EmailPostOffice.Web/"]
COPY ["src/EmailPostOffice.Application/EmailPostOffice.Application.csproj", "src/EmailPostOffice.Application/"]
COPY ["src/EmailPostOffice.Domain/EmailPostOffice.Domain.csproj", "src/EmailPostOffice.Domain/"]
COPY ["src/EmailPostOffice.Domain.Shared/EmailPostOffice.Domain.Shared.csproj", "src/EmailPostOffice.Domain.Shared/"]
COPY ["src/EmailPostOffice.Application.Contracts/EmailPostOffice.Application.Contracts.csproj", "src/EmailPostOffice.Application.Contracts/"]
COPY ["src/EmailPostOffice.HttpApi/EmailPostOffice.HttpApi.csproj", "src/EmailPostOffice.HttpApi/"]
COPY ["src/EmailPostOffice.EntityFrameworkCore/EmailPostOffice.EntityFrameworkCore.csproj", "src/EmailPostOffice.EntityFrameworkCore/"]
RUN dotnet restore "src/EmailPostOffice.Web/EmailPostOffice.Web.csproj"
COPY . .
WORKDIR "/src/src/EmailPostOffice.Web"
RUN dotnet build "EmailPostOffice.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailPostOffice.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailPostOffice.Web.dll"]