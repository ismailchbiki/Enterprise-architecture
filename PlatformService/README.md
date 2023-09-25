# Steps of the entire setup

### 1. Create an API

### 2. Set up Docker for the API

- To build an image (using Dockerfile in the root API dir)<br>
  docker build -t ismailchbiki/commands-service-api:v1 .

- To run an instance of the source-image as a container. (run is different than start)<br>
  docker run -p 8080:8080 --name container-name ismailchbiki/commands-service-api:v1

- To push an image to DockerHub account (registry)<br>
  docker push ismailchbiki/commands-service-api:v1

> **Deletions:**

- To stop a container (after running it)<br>
  docker rm container-name

- To delete a container (after stopping it)<br>
  docker rm container-name

- To force stop and delete a container (after running it)<br>
  docker rm -f container-name

- To delete an image<br>
  docker rmi ismailchbiki/commands-service-api:v1

### 3. Set up Kubernetes (files are in K8S dir)

- To create a service/deployment<br>
  (make sure to have an image prepared to be used) (script file in K8S)<br>
  kubectl apply -f platforms-depl.yaml

> The service (api image) now is up and running, but we don't have access to it. So now we need to create a nod port to give us access to our service (api image) running in Kubernetes.

- Run this for the NodPort service (script file in K8S)<br>
  kubectl apply -f platforms-nodport-service.yaml<br>

- Run this command to get the API access port<br>
  kubectl get services<br>

> Result<br>
> NAME TYPE CLUSTER-IP EXTERNAL-IP PORT(S) AGE<br>
> kubernetes ClusterIP 10.96.0.1 <none<none>> 443/TCP 3d15h<br>
> platformnpservice-srv NodePort 10.107.118.177 <none<none>> 80:30187/TCP 6s<br>

> (Our API running in Kubernetes is accessed now via this port: 30187)

### 4. Set up Kubernetes for microservices deployment

#### 4.1. Create an appsettings.Production.json

- Services inside a Kubernetes Cluster use ClusterIP services to talk to each other.
- The endpoint from PlatformService which needs to be reached out is the cluster ip service (name: commands-clusterip-srv in the commands-depl.yaml file) attached to the command service pod.
