# .NET Core worker processing Azure Service Bus Queue scaled by KEDA
A Docker container written in .NET 6 that will receive messages from a Service Bus queue and scale via KEDA.

## Flow

![Flow](images/product-flow.png)


# Build
```
cd C:\projects\digi\Reconhece 2.0\ms-product-integration\src\MsProductIntegration
docker build . -t product-processor:1.0.1 --build-arg PAT=xtyjyzv7vq4fvw4wzyvfwq7mvedlbbhgfppuxpnrg2rrdmbhu4ga
```
# Test
```
docker run --name product-processor -e DatabaseSettings__ConnectionString="mongodb+srv://authdbuser:4uthdbus3r!@cluster0.ij6uy.mongodb.net/myFirstDatabase?retryWrites=true&w=majority" -e KEDA_SERVICEBUS_AUTH_MODE="ConnectionString" -e KEDA_SERVICEBUS_QUEUE_CONNECTIONSTRING="Endpoint=sb://hml-reconhece.servicebus.windows.net/;SharedAccessKeyName=product-consumer;SharedAccessKey=djdr//0pdJEtVcs+6PDaCZ4NWMSo4Yw8CLJqUIIeXWE=;EntityPath=products" -e KEDA_SERVICEBUS_QUEUE_NAME="products" -p 8086:80 -d product-processor:1.0.1
 docker container rm -f b9d0f28fd286 
```

# Tag
```
docker tag product-processor:1.0.1 gcr.io/gke-teste-338515/product-processor:1.0.1
```
# Push
```
docker push gcr.io/gke-teste-338515/product-processor:1.0.1
```

# Queue
Login to azure
az login 
If not default subscription 'Digi Projetos'
az account list --output table 
az account set --subscription 0d604cab-4134-4a75-bb02-0150b593114f 
az account list --output table 

Send Subscription
az servicebus queue authorization-rule create --resource-group reconhece --namespace hml-reconhece  --queue-name products --name product-producer --rights Send
az servicebus queue authorization-rule keys list --resource-group reconhece --namespace-name hml-reconhece --queue-name products --name product-producer

Listen Subscription
az servicebus queue authorization-rule create --resource-group reconhece --namespace hml-reconhece  --queue-name products --name product-consumer --rights Listen
az servicebus queue authorization-rule keys list --resource-group reconhece --namespace-name hml-reconhece --queue-name products --name product-consumer

Monitor Subscription
az servicebus queue authorization-rule create --resource-group reconhece --namespace-name hml-reconhece --queue-name products --name keda-monitor --rights Manage Send Listen
az servicebus queue authorization-rule keys list --resource-group reconhece --namespace-name hml-reconhece --queue-name products --name keda-monitor
