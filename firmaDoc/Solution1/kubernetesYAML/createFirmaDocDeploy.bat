kubectl apply -f documentapi-deploy.yaml
kubectl apply -f usersapi-deploy.yaml

kubectl apply -f certificateapi-deploy.yaml
kubectl apply -f fileapi-deploy.yaml
kubectl apply -f logapi-deploy.yaml


kubectl apply -f usersapi-nodeport.yaml
kubectl apply -f certificateapi-nodeport.yaml

kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingressgw.yaml

kubectl apply -f rabbitmq-deploy.yaml