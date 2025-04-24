//This should be the startup project. This is the one that starts up the API and the Web. 
//We might want to consider moving the web project into the same folder as this one and renaming
//this overall project folder to PWA? 
//
//Video on building a .NET Aspire app along with deployment howto: https://www.youtube.com/watch?v=0Cyb4OFhQbI
//Includes a cool bit at the end about how to create our own custom commands on a resource (like the web or api)

var builder = DistributedApplication.CreateBuilder(args);

//Marking redis as Persistent so that it doesn't get deleted when the container is stopped. Also makes it faster to start up
var cache = builder.AddRedis("redis")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisInsight();

/* KEF - todo
 * There seems to be a way to build/attach a sql server instance when deployed to Docker. I haven't figured 
 * out that part yet. Right now it's still connecting to the local instance.
 * */
/* KEF - todo
var password = builder.AddParameter("password", secret: true);

var sql = builder.AddSqlServer("sqldb", password, 58434)
    .WithBindMount("c:\\temp\\docker\\mssql\\data", "/var/opt/mssql/data")
    .WithBindMount("c:\\temp\\docker\\mssql\\log", "/var/opt/mssql/log")
    .WithBindMount("c:\\temp\\docker\\mssql\\secrets", "/var/opt/mssql/secrets");

var sqldb = sql.AddDatabase("EventDb");

var apiService = builder.AddProject<Projects.PWAApi_ApiService>("pwaapi").WithReference(sqldb);
*/

//without sql
var apiService = builder.AddProject<Projects.PWAApi_ApiService>("pwaapi")
    .WithReference(cache)
    .WaitFor(cache);


//This needs to point to the folder where the Web project is located. I contemplated putting the 
//Web project in the same folder as this solution, but we can talk about that later. Just make sure that your PWA-Web
//project sits at the same level as the PWA-APIv2 folder
builder.AddNpmApp("angular", "../../PWA-Web")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithHttpEndpoint(port: 4200, env: "PORT") //without the port: 4200 it will be a random port every time you restart AppHost
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .PublishAsDockerFile();


builder.Build().Run();
