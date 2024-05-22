$files = @(
	"schema"
)

dotnet tool restore

$file = "Hypermedia-Bndd"
$namespace = "bndd_hypermedia"
$namespace_client = $namespace + "_client"
$namespace_server = $namespace + "_server"
dotnet restyard-generator --schema-file "./$file.xml" --output-file "./$file.Server.g.cs" --language csharp --type server --namespace $namespace_server
dotnet restyard-generator --schema-file "./$file.xml" --output-file "./$file.Client.g.cs" --language csharp --type client --namespace $namespace_client