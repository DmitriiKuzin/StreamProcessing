# Otus-ApiGateWay

```shell
helm install otus .\deploy\stream-processing\ --namespace otus-sp --create-namespace --dependency-update
```

```shell
newman run .\StreamProcessing.postman_collection.json
```