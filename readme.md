# Questions

- How (in-depth) do lists work ??
- What are IEnumerables
- What are SingleOrDefaults
- How do [ApiController] & [HttpGet] headers work ??
- What are record types ?
- What are actionresults ?
- What other types of ConfigureServices are there (i.e services.AddSingleton...)
- What the fuck are all these select operators (.Select, .Where ...) from Sys.Linq ??
- What are Extensions and static classes ?
- Whats **this Item item** in Extensions.cs
- What are createdAtAction
- More in-depth understanding of MongoDB + .NET



# Running dis shit on docker
```console
1. First run "docker network create <networkName>"
2. Then run "docker run -d --name <containerName> -p <external-port>:27017 -e MONGO_INITDB_ROOT_USERNAME=<username> -e MONGO_INITDB_ROOT_PASSWORD=<password> --network=<networkName> mongo"
3. Then run "docker build -t <imagename>:<tag> ."
4. Run "docker run -it --rm -p <external-port> (the you'll connect to):80 -e MongoDbSettings:Host=<hostName>(mongo) -e MongoDbSettings:Password=<password> --network=<networkName> <imagename>:<tag>

To validate if user is created in Mongo DB:
1. Run "docker exec --it <containerName> (mongo) /bin/bash"
2. Run "use admin"
3. Run "db.auth(<username>,passwordPrompt()) --> <password>"
4. Run "db.getUsers()"
5. Run "db.hostInfo()" to get info
```
# Running dis shit on Kubernetezzz
```console
1. Make sure the previous docker image is on docker hub, Make sure your not on Aurizon context for kubezy
2. Run "kubectl create secret generic server-secrets --from-literal=mongodb-password='<password>'"
3. Create kubernetes yaml file for MongoDB, then run "kubectl apply -f <filename>.yaml"
4. Create kubernetes yaml file, then run "kubectl apply -f <filename>.yaml"
5. To get pods - "kubectl get pods"
6. To get logs - "kubectl get logs <pod-name>"
```