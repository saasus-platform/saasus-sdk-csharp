FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim

WORKDIR /workspace

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1 \
    NUGET_XMLDOC_MODE=skip

COPY saasus-sdk-csharp.csproj ./
RUN dotnet restore saasus-sdk-csharp.csproj

COPY . ./

CMD ["dotnet", "build", "saasus-sdk-csharp.csproj", "--configuration", "Debug", "--no-restore"]
