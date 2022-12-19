#!/usr/bin/env bash

docker run -v $(pwd)/src:/src mcr.microsoft.com/dotnet/sdk:6.0-alpine dotnet test src/PockyBot.NET.Tests/PockyBot.NET.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat="opencover"
curl -Os https://uploader.codecov.io/latest/linux/codecov
chmod +x codecov
./codecov -f src/PockyBot.NET.Tests/coverage.opencover.xml -t $CODECOV_TOKEN --sha $COMMIT_SHA

# Codacy
bash <(curl -Ls https://coverage.codacy.com/get.sh) report -r src/PockyBot.NET.Tests/coverage.opencover.xml --commit-uuid $COMMIT_SHA
