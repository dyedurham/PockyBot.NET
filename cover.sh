#!/usr/bin/env bash

dotnet test src/PockyBot.NET.Tests/PockyBot.NET.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat="opencover"
bash <(curl -s https://codecov.io/bash) -f src/PockyBot.NET.Tests/coverage.opencover.xml

# Codacy
export CODACY_PROJECT_TOKEN="$CODACY_PROJECT_TOKEN"
bash <(curl -Ls https://coverage.codacy.com/get.sh) report -r src/PockyBot.NET.Tests/coverage.opencover.xml
