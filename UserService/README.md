# Steps of the entire setup

### 1. Create an API

### 2. Set up Docker for the API

- To build an image (using Dockerfile in the root API dir)<br>
  docker build -t ismailchbiki/platform-service-arm64:v1 .

- To run an instance (detached) of the source-image as a container. (run is different than start)<br>
  <b>Note:</b> 8080:80 = hostPort:containerPort (containerPort exposed in Dockerfile)<br>
  docker run -d -p 8080:80 --name platform-service-arm64 ismailchbiki/platform-service-arm64:v1

- multi-arch image building:

1. List contexts:<br>
   docker context ls
2. choose context:<br>
   docker context use default
3. List builders:<br>
   docker buildx ls
4. choose builder:<br>
   docker buildx use elastic_panini

- build multi-arch image (The builder in this case 'elastic_panini'):<br>
  docker buildx build --platform linux/amd64,linux/arm64 -t ismailchbiki/platform-service_multi-arch:v1 .

- To push an image to Docker Hub<br>
  docker push ismailchbiki/platform-service-arm64:v1

- To pull an image from Docker Hub<br>
  docker pull ismailchbiki/platform-service-amd64:v1

> **Deletions:**

- To stop a container (after running it)<br>
  docker stop platform-service

- To delete a container (after stopping it)<br>
  docker rm platform-service

- To force stop and delete a container (after running it)<br>
  docker rm -f platform-service

- To delete an image<br>
  docker rmi ismailchbiki/image_name:version_tag
