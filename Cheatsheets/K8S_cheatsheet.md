# Kubernetes Cheatsheet

- ## Common commands

### Create a new namespace "project-namespace"

```
kubectl create namespace project-namespace
```

### Get namespaces

```
kubectl get namespaces
```

### Check namespace privileges

```
kubectl auth can-i list namespaces
```

### Get deployments / pods / services in "project-namespace" namespace

```
kubectl get deployments -n project-namespace
kubectl get pods -n project-namespace
kubectl get services -n project-namespace

*** This format is also possible ***

kubectl get pods --namespace=project-namespace
```

### Delete a deployment / pod / service (e.g. LoadBalancer, ClusterIP, etc.) in "project-namespace" namespace

```
kubectl delete deployment deployment-name -n project-namespace
kubectl delete pod pod-name -n project-namespace
kubectl delete service service-name -n project-namespace
```

### Delete a Kubernetes resource specified in the "deployment.yaml" file from a Kubernetes cluster

```
kubectl delete -f deployment.yaml
```

### Delete a namespace

```
kubectl delete namespace project-namespace
```

### Force a deployment to pull an image from a registry and refresh the deployment (re-applying the file won't work)

```
kubectl rollout restart deployment deployment-name -n project-namespace
```

### Monitor the progress of the rolling update

```
kubectl rollout status deployment/deployment-name -n project-namespace
```

- ## Secrets

### Create a secret (password for mssql so that it's not in some config file) in "project-namespace" namespace

```
kubectl create secret generic SECRET_NAME --from-literal=SA_PASSWORD="ACCEPTED_PASSWORDS" -n project-namespace
```

### Get secrets in "project-namespace" namespace

```
kubectl get secret -n project-namespace
```

### Delete secrets in "project-namespace" namespace

```
kubectl delete secret SECRET_NAME -n project-namespace
```

- ## PersistentVolumeClaim

### Delete PersistentVolumeClaim in "project-namespace" namespace

```
kubectl delete persistentvolumeclaim claim-name -n project-namespace
```

### Change the default namespace

```
kubectl config set-context --current --namespace=project-namespace
```

### Delete ingress in "project-namespace" namespace

```
kubectl delete ingress ingress-srv-name -n project-namespace
```

- ## Scaling

### Scale down a deployment to 0 in "project-namespace" namespace

```
kubectl scale deployment DEPLOYMENT_NAME --replicas=0 -n project-namespace
```

- ## Scan a script file with snyk

### Scan a script file (ex: deployment.yaml)

```
snyk iac test deployment.yaml
```
