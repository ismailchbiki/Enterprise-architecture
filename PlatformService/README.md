# Steps of the entire setup

### 1. Create an API

### 2. Set up Docker for the API

- To build an image (using Dockerfile in the root API dir)<br>
  docker build -t ismailchbiki/platform-service-ARM64:v1 .

- To run an instance (detached) of the source-image as a container. (run is different than start)<br>
  <b>Note:</b> 8080:80 = hostPort:containerPort (containerPort exposed in Dockerfile)<br>
  docker run -d -p 8080:80 --name platform-service-ARM64 ismailchbiki/platform-service-ARM64:v1

- multi-arch image building:

1. List contexts:
   docker context ls
2. choose context:
   docker context use default
3. List builders:
   docker buildx ls
4. choose builder:
   docker buildx use elastic_panini

build multi-arch image (The builder in this case 'elastic_panini'):
docker buildx build --platform linux/amd64,linux/arm64 -t ismailchbiki/platform-service_multi-arch:v1 .

- To push an image to Docker Hub<br>
  docker push ismailchbiki/platform-service-ARM64:v1

- To pull an image from Docker Hub<br>
  docker pull ismailchbiki/platform-service-ARM64:v1

> **Deletions:**

- To stop a container (after running it)<br>
  docker stop platform-service

- To delete a container (after stopping it)<br>
  docker rm platform-service

- To force stop and delete a container (after running it)<br>
  docker rm -f platform-service

- To delete an image<br>
  docker rmi ismailchbiki/platform-service-api:v1

### 3. Set up Kubernetes (files are in K8S dir)

- To create a service/deployment (Setup a clusterIP for internal communication between pods)<br>
  (make sure to have an image prepared to be used) (script file in K8S)<br>
  kubectl apply -f platform-depl.yaml

> The service (api image) now is up and running, but we don't have access to it. So now we need to create a NodePort (platform-np-srv.yaml) to give us access to our service (api image) running in Kubernetes.

- Run this for the NodePort service (platform-np-srv.yaml)<br>
  kubectl apply -f platform-nodport-service.yaml<br>

- Run this command to get the API access port (e.g. 31412)<br>
  kubectl get services<br>

  > Result<br>
  > NAME TYPE CLUSTER-IP EXTERNAL-IP PORT(S) AGE<br>
  > command-clusterip-srv ClusterIP 10.110.25.28 <none<none>> 80/TCP 14h<br>
  > kubernetes ClusterIP 10.96.0.1 <none<none>> 443/TCP 6d14h<br>
  > platform-clusterip-srv ClusterIP 10.104.175.164 <none<none>> 80/TCP 14h<br>
  > platform-np-srv NodePort 10.98.25.5 <none<none>> 80:31412/TCP 14h<br>

> (Our API running in Kubernetes is accessed now via this port: 31412)

### 4. Set up Kubernetes for microservices deployment

#### 4.1. Create an appsettings.Production.json (NodePort for local development and testing)

- Pods inside a Kubernetes Cluster use ClusterIP for internal communication.
- Pods inside a Kubernetes can be reached by using NodePort Service (for external communication).
- The endpoint from PlatformService which needs to be reached out is the clusterIP service (name: commands-clusterip-srv in the commands-depl.yaml file) attached to the command service pod.

#### 4.2. Ingress for production deployment with Nginx Ingress Controller (Serves to expose a port for external access)

> USED MOSTLY FOR GATEWAY SETUP
> Check README.md in K8S directory
