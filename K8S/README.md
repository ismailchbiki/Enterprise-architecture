# Kubernetes definitions

- I can expose my service with minimal <b>ClusterIp</b> (within k8s cluster) or larger exposure with <b>NodePort</b> (within cluster external to k8s cluster - within same network) or <b>LoadBalancer</b> (external world (e.g., internet) or whatever I define in my LB).<br>

```
ClusterIp exposure (Within cluster) < NodePort exposure (within network) < LoadBalancer exposure (Internet exposure)
```

#### ClusterIp:

- Expose service through and within k8s cluster with ip/name:port<br>

#### NodePort:

- Expose service through and within Internal network VM's also external to k8s ip/name:port<br>

#### LoadBalancer:

- Expose service through External world (e.g., internet) or whatever I define in my LB.<br>

<br>
<br>

# Setting up an external access to a gateway (ingress)

- Next step is to implement the routing rules (ingress-nginx file) to be used by my Ingress manifest.

<b>Note:</b><br>

- When working locally (no cloud deployment), make sure to update hosts locked file in ~/etc/hosts to include the domain name and local ip address.
- Running the command (online pre-made Ingress configuration) to deploy the Nginx Ingress Controller is necessary to have a working Ingress setup in my Kubernetes cluster. The Ingress Controller is responsible for implementing the routing rules specified in my Ingress resources. Without the Ingress Controller, the Ingress resources won't have any effect, and external traffic won't be correctly routed to my services.

<br/>
Before applying my own ingress service file, I need to deploy the Nginx Ingress Controller in my Kubernetes cluster using this command:

```
  kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/cloud/deploy.yaml
```

After that, I can run my own ingress service file with:

```
kubectl apply -f ingress-srv.yaml
```
