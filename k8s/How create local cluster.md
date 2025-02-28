# How running with cluster Kubernetes

1. Install k3d (https://k3d.io/stable/#install-script)

2. Install kubectl (https://kubernetes.io/docs/tasks/tools/)

3. Create cluster with command:
k3d cluster create mycluster --servers 3 --agents 3 -p "8080:30000@loadbalancer"

> Access with 8080 localhost port.
