kubectl delete deployment certificateapi-depl
kubectl delete deployment documentapi-depl 
kubectl delete deployment fileapi-depl
kubectl delete deployment logapi-depl
kubectl delete deployment usersapi-depl
kubectl delete deployment rabbitmq-deploy.yaml

kubectl delete services certificateapi-clusterip
kubectl delete services certificateapi-np
kubectl delete services documentapi-clusterip
kubectl delete services fileapi-clusterip
kubectl delete services usersapi-clusterip

kubectl delete deployment ingress-nginx-controller --namespace=ingress-nginx
kubectl delete services ingress-nginx-controller --namespace=ingress-nginx
kubectl delete services ingress-nginx-controller-admission --namespace=ingress-nginx

kubectl delete services rabbitmq-clusterip-srv
kubectl delete services rabbitmq-loadbalancer

kubectl delete ingress ingress-gw