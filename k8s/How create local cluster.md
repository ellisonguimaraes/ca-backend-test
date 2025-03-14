# How running with cluster Kubernetes

> **Observation!** \
> Docker is required

1. Install Kind local cluster (https://kind.sigs.k8s.io/docs/user/quick-start/#installation)


1. Install kubectl (https://kubernetes.io/docs/tasks/tools/)

2. Create cluster with command:
kind create cluster --name cluster-nexer --config kind-config.yaml

1. Run command `kubectl apply -f <file_name>.yaml` for each of the yaml files:
  - `ingress-nginx.yaml`;
  - `seq.yaml`;
  - `redis.yaml`;
  - `postgres.yaml`;
  - `ingress-nginx.yaml`;
  - `ingress.yaml`;
  - `deployment.yaml`.

2. Access API with 30000 localhost port.
