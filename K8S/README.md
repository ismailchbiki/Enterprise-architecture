# Setting up an external access to a gateway (ingress)

Next step is to creating a routing file that this controller is going to use to route to our 2 services

<b>Note:</b><br>

- Don't forget to update hosts locked file in ~/etc/hosts to include the domain name and local ip address
- Running the command (online pre-made Ingress configuration) to deploy the Nginx Ingress Controller is necessary to have a working Ingress setup in your Kubernetes cluster. The Ingress Controller is responsible for implementing the routing rules specified in your Ingress resources. Without the Ingress Controller, the Ingress resources won't have any effect, and external traffic won't be correctly routed to your services.

<br/>
Before applying my own ingress service file, I need to deploy the Nginx Ingress Controller in my Kubernetes cluster using this command:<br/>
  kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/cloud/deploy.yaml
  <br/>

- My file name : ingress-srv.yaml

Once the ingress-srv.yaml file is created we can run it with:

> kubectl apply -f ingress-srv.yaml
