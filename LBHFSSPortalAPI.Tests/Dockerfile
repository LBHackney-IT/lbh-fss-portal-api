FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./LBHFSSPortalAPI.sln ./
COPY ./LBHFSSPortalAPI/LBHFSSPortalAPI.csproj ./LBHFSSPortalAPI/
COPY ./LBHFSSPortalAPI.Tests/LBHFSSPortalAPI.Tests.csproj ./LBHFSSPortalAPI.Tests/

RUN dotnet restore ./LBHFSSPortalAPI/LBHFSSPortalAPI.csproj
RUN dotnet restore ./LBHFSSPortalAPI.Tests/LBHFSSPortalAPI.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out LBHFSSPortalAPI.Tests/LBHFSSPortalAPI.Tests.csproj

CMD dotnet test
