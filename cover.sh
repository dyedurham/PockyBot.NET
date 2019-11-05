#!/usr/bin/env bash

dotnet test src/PockyBot.NET.Tests/PockyBot.NET.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat="opencover"
bash <(curl -s https://codecov.io/bash) -f src/PockyBot.NET.Tests/coverage.opencover.xml

# Codacy
curl -Ls "https://github.com/codacy/csharp-codacy-coverage/releases/download/$CODACY_COVERAGE_VERSION/Codacy.CSharpCoverage_linux-x64" --output "Codacy.CSharpCoverage_linux-x64"
chmod +x ./Codacy.CSharpCoverage_linux-x64
./Codacy.CSharpCoverage_linux-x64 -c "$TRAVIS_COMMIT" -t "$CODACY_PROJECT_TOKEN" -r src/PockyBot.NET.Tests/coverage.opencover.xml -e opencover
