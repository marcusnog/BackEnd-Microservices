# Build
```
cd C:\projects\digi\Reconhece 2.0\ms-authorization\src
docker build . -t userapi:1.0.6
docker build . -t gcr.io/gke-teste-338515/userapi:1.0.8
```
# Test
```
docker run --name userapi -e DatabaseSettings__ConnectionString="mongodb+srv://authdbuser:4uthdbus3r!@cluster0.ij6uy.mongodb.net/myFirstDatabase?retryWrites=true&w=majority" -e SMTP_ID="20212201ce231aa7cc205095" -e COMMUNICATION_TEMPLATE_ID="000000000000000000000001" -e COMMUNICATION_API="http://34.132.220.49/communication/api/Email/template/{0}/{1}" -p 8081:80 -d userapi:1.0.6
 docker container rm -f b9d0f28fd286 
```

# Tag
```
docker tag userapi:1.0.6 gcr.io/gke-teste-338515/userapi:1.0.6
```
# Push
```
docker push gcr.io/gke-teste-338515/userapi:1.0.6
```
