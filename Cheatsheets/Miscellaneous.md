# Miscellaneous

- ## Database schema migration in .NET

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- ## GitHub Actions

### CI/CD (automatic) image versioning:

```
docker build -t Docker_User_Account/IMAGE_NAME:tag ./Project_with_Dockerfile

** Example: (The tag below is dynamic, and DOCKER_USER is GitHub secret)
docker build -t $DOCKER_USER/user-service-api:$(date +%s) ./UserService
```

- ## gRPC

### gRPC ("Google" Remote Procedure Call)

- Uses HTTP/2 protocol to transport binary messages (inc. TLS),
- Focused on high performance,
- Relies on "Protocol Buffers" (aka Protobuf) to define the contract between end points,
- Multi-language support (C# client can call a Ruby service),
- Frequently used as a method of service to service communication
