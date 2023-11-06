gcloud sql instances describe sql-server-instance

Connection string: "Server=34.90.59.171,1433;Database=PlatformDB;User Id=sqlserver;Password=HelloWorld1&;"

### For versioning:

Docker Hub Workflow pipeline:
docker build -t $DOCKER_USER/platform-service-api:$(date +%s) ./PlatformService
docker build -t $DOCKER_USER/command-service-api:$(date +%s) ./CommandsService
