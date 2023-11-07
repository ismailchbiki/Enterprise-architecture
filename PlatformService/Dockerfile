# Use the official .NET SDK image as a build stage.
# This image contains the necessary tools to build and publish a .NET Core application.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory to /app
WORKDIR /app

# Copy the .csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Explicit copy of the appsettings.template.json file
COPY appsettings.template.json ./

# Copy the remaining source code
COPY . .

# Publish the application for production
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image as the final image.
# This image contains the minimal runtime required to run an ASP.NET Core application.
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory to /app
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/out .

# Expose the port on which the application will run
EXPOSE 80

# Define the entry point for the application
ENTRYPOINT ["dotnet", "PlatformService.dll"]
