dnu restore && dnu build src\Aqua test\Aqua.NET35.Tests test\Aqua.Tests && dnx -p test\Aqua.Tests test && dotnet pack src\Aqua --output artifacts --configuration Release --version-suffix 007