#!/usr/bin/env bash

# Dry run semantic-release to get next version number
VERSION="$(npx -p semantic-release@15.13.19 -p @semantic-release/changelog@3.0.4 -p @semantic-release/git@7.0.16 \
-p @semantic-release/exec@3.3.5 -p @semantic-release/github@5.4.2 semantic-release --dry-run | \
grep "next release version is" | sed -n "s/^.*next release version is\s*\(\S*\).*$/\1/p")"

if [ -z "$VERSION" ]
then
	echo "There are no relevant changes, skipping release"
	exit
fi

echo "The next release number is $VERSION"

mkdir -p ./artifacts
dotnet pack "src/PockyBot.NET/PockyBot.NET.csproj" -p:PackageVersion="$VERSION" -o "../../artifacts"

# UNCOMMENT WHEN READY FOR RELEASE
# npx -p semantic-release@15.13.19 -p @semantic-release/changelog@3.0.4 -p @semantic-release/git@7.0.16 \
# -p @semantic-release/exec@3.3.5 -p @semantic-release/github@5.4.2 semantic-release
