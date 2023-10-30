# Build
```
cd C:\projects\digi\Reconhece 2.0\ms-catalog-api\src
docker build . -t catalog-api:1.0.0
```
# Test
```
docker run --name catalog-api -e DatabaseSettings__ConnectionString="mongodb+srv://authdbuser:4uthdbus3r!@cluster0.ij6uy.mongodb.net/myFirstDatabase?retryWrites=true&w=majority" -p 8085:80 -d catalog-api:1.0.0
 docker container rm -f b9d0f28fd286 
```

# Tag
```
docker tag catalog-api:1.0.0 gcr.io/gke-teste-338515/catalog-api:1.0.0
```
# Push
```
docker push gcr.io/gke-teste-338515/catalog-api:1.0.0
```
