dotnet test src/PockyBot.NET.Tests/PockyBot.NET.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat="opencover"
bash <(curl -s https://codecov.io/bash) -f src/PockyBot.NET.Tests/coverage.opencover.xml
