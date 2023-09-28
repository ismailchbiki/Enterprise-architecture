# Setting up an external access to a gateway (ingress)

Next step is to creating a routing file that this controller is going to use to route to our 2 services

- File name : ingress-srv.yaml

Once the ingress-srv.yaml file is created we can run it with:

> kubectl apply -f ingress-srv.yaml

<b>Note:</b><br>

- Don't forget to update hosts locked file in ~/etc/hosts
