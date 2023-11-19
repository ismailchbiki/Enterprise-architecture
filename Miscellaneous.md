# Miscellaneous

### To connect to DB in Google Cloud from remote console:

```
gcloud sql connect sql-server-instance -u sqlserver
```

### To get info about the sql server instance:

```
gcloud sql instances describe sql-server-instance
```

### GitHub Actions CI/CD (automatic) image versioning:

```
docker build -t $DOCKER_USER/platform-service-api:$(date +%s) ./UserService
```

### gRPC ("Google" Remote Procedure Call)

- Uses HTTP/2 protocol to transport binary messages (inc. TLS),
- Focused on high performance,
- Relies on "Protocol Buffers" (aka Protobuf) to define the contract between end points,
- Multi-language support (C# client can call a Ruby service),
- Frequently used as a method of service to service communication

### Database Migration

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Connection to Kubernetes Cluster in Google Cloud

- #### Login

```
gcloud auth application-default login
OR
gcloud auth login <email-address>
```

- #### IAM Permissions

```
gcloud projects add-iam-policy-binding <your-project-id> --member=user:<email-address> --role=roles/container.viewer
```

- #### Set-quota-project

```
gcloud auth application-default set-quota-project <your-project-id>
```

- #### Check namespace privileges

```
kubectl auth can-i list namespaces
```

- #### Grant Kubernetes Cluster Role

```
gcloud container clusters get-credentials CLUSTER_NAME --zone ZONE

In this case:
CLUSTER_NAME = gke-google-cloud
ZONE = europe-west1

And then:
kubectl create clusterrolebinding cluster-admin-binding \
  --clusterrole=cluster-admin \
  --user=<email-address>
```

- #### List projects

```
gcloud projects list

* Results;
PROJECT_ID              NAME              PROJECT_NUMBER
gothic-sequence-404223  My First Project  956488113671
```

- #### Set a project

```
gcloud config set project <PROJECT_ID>
```
