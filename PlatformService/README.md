# Docker

### To build and create an image from Dockerfile

docker build . -t source-image

### To run an instance of the source-image as a container. (run is different than start)

docker run -p 8080:8080 --name container-name source-image
