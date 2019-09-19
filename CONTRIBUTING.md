# Contributing

## Commit Message Format

This repository uses conventional commit messages. These are commits in the
format `<type>(<optional scope>): <commit message>`. Allowed types for this
project are:

- `feat` &mdash; a new feature
- `fix` &mdash; a bug fix
- `docs` &mdash; documentation only changes
- `style` &mdash; changes that do not affect the meaning of the code
  (whitespace, formatting, missing semi-colons etc)
- `refactor` &mdash; a code change that neither fixes a bug nor adds a feature
- `perf` &mdash; a code change that improves performance
- `test` &mdash; adding missing tests or correcting existing tests
- `build` &mdash; changes that affect the build system or external dependencies
- `ci` &mdash; changes to our CI configuration
- `chore` &mdash; other changes that don't modify source or test files
- `revert` &mdash; reverts a previous commit

## Tools

### Commitizen

You can automate the process of commiting by using commitizen. You can install
it using npm:

```bash
npm install -g commitizen
```

Then you can commit using:

- `git add .`
- `git cz`
