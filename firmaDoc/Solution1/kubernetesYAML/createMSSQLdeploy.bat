kubectl create secret generic mssql --from-literal=SA_PASSWORD="admIN123**"

kubectl apply -f local-pvc.yaml
kubectl apply -f mssql.yaml