FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/MatrixCompute.Runner/*.csproj", "src/MatrixCompute.Runner/"]
COPY ["src/MatrixCompute.Core/*.csproj", "src/MatrixCompute.Core/"]
COPY ["MatrixCompute.sln", "./"]

RUN dotnet restore

COPY . .

RUN dotnet publish "src/MatrixCompute.Runner/MatrixCompute.Runner.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MatrixCompute.Runner.dll"]