# Steps of the entire setup

### 1. Create an API

### 2. Set up Docker for the API

- To build an image (using Dockerfile in the root API dir)<br>
  docker build -t source-image .

- To run an instance of the source-image as a container. (run is different than start)<br>
  docker run -p 8080:8080 --name container-name source-image

- To push an image to DockerHub account (registry)<br>
  docker push ismailchbiki/source-image:tag

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
