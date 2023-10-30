# Build
```
cd C:\projects\digi\Reconhece 2.0\ms-authentication\src\Auth.Api
docker build . -t authapi:1.0.0
```
# Test
```
docker run --name authapi -e GET_USERINFO_URL="http://34.134.242.113/userapi/api/User/GetUserInfo" -p 8082:80 -d authapi:1.0.1
 docker container rm -f b9d0f28fd286 
```

# Tag
```
docker tag authapi:1.0.1 gcr.io/gke-teste-338515/authapi:1.0.1
```
# Push
```
docker push gcr.io/gke-teste-338515/authapi:1.0.1
```

mongo admin -u authdbuser -p '4uthdbus3r!'
