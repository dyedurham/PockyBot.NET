module.exports = {
    verifyConditions: [
        // Verifies the conditions for the plugins used below
        // For example verifying a GITHUB_TOKEN environment variable has been provided
        '@semantic-release/changelog',
        '@semantic-release/git',
        '@semantic-release/github',
    ],
    prepare: [
        // https://github.com/semantic-release/changelog
        // Set of semantic-release plugins for creating or updating a changelog file.
        '@semantic-release/changelog',
        // https://github.com/semantic-release/git
        // Git plugin is need so the changelog file will be committed to the Git repository and available on subsequent builds in order to be updated.
        '@semantic-release/git'
    ],
    publish: [
        // https://github.com/semantic-release/git
        // Exec plugin uses to call dotnet nuget push to push the packages from
        // the artifacts folder to NuGet
        {
            path: '@semantic-release/exec',
            cmd: `dotnet nuget push ./artifacts/*.nupkg -k ${process.env.NUGET_API_KEY} -s https://api.nuget.org/v3/index.json`,
        },

        // https://github.com/semantic-release/github
        // Set of Semantic-release plugins for publishing a GitHub release.
        // Includes the packages from the artifacts folder as assets
        {
            path: '@semantic-release/github',
            assets: 'artifacts/**/*.nupkg',
        },
    ],
};
