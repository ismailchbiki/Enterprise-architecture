# Miscellaneous

### To connect to DB in Google Cloud from remote console:

gcloud sql connect sql-server-instance -u sqlserver

### To get info about the sql server instance:

gcloud sql instances describe sql-server-instance

### GitHub Actions CI/CD image versioning:

docker build -t $DOCKER_USER/platform-service-api:$(date +%s) ./PlatformService

### gRPC ("Google" Remote Procedure Call)

- Uses HTTP/2 protocol to transport binary messages (inc. TLS),
- Focused on high performance,
- Relies on "Protocol Buffers" (aka Protobuf) to define the contract between end points,
- Multi-language support (C# client can call a Ruby service),
- Frequently used as a method of service to service communication
