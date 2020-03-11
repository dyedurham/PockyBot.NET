#!/usr/bin/env bash

# Dry run semantic-release to get next version number
VERSION="$(npx -p semantic-release@16.0.2 -p @semantic-release/changelog@3.0.6 -p @semantic-release/git@8.0.0 \
-p @semantic-release/exec@4.0.0 -p @semantic-release/github@6.0.1 semantic-release --dry-run | \
grep "next release version is" | sed -n "s/^.*next release version is\s*\(\S*\).*$/\1/p")"

if [ -z "$VERSION" ]
then
	echo "There are no relevant changes, skipping release"
	exit
fi

echo "The next release number is $VERSION"

mkdir -p ./artifacts
dotnet pack "src/PockyBot.NET/PockyBot.NET.csproj" -p:PackageVersion="$VERSION" -o "./artifacts"

npx -p semantic-release@16.0.2 -p @semantic-release/changelog@3.0.6 -p @semantic-release/git@8.0.0 \
-p @semantic-release/exec@4.0.0 -p @semantic-release/github@6.0.1 semantic-release
