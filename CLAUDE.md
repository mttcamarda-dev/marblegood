# CLAUDE.md

This file provides guidance to AI assistants (Claude and others) working in this repository.

## Project Overview

**marblegood** is a new, empty repository. This CLAUDE.md will be updated as the project takes shape. AI assistants should update this file as structure, conventions, and workflows are established.

## Repository State

This repository is currently being initialized. There are no source files, dependencies, or configuration yet. When working on this project:

- Check for a README.md for product-level context
- Check for package.json, pyproject.toml, Cargo.toml, or similar for the technology stack
- Look for existing tests before writing new ones to match the established pattern

## Development Workflow

### Branch Strategy

- Development happens on feature branches prefixed with `claude/` for AI-assisted work
- Always push to the branch specified in your task context
- Never push directly to `main` without explicit instruction

### Commit Conventions

Use clear, descriptive commit messages that explain *why* a change was made, not just *what* changed. Example format:

```
Add user authentication with JWT tokens

Implement login/logout endpoints and token refresh logic to support
secure session management across the API.
```

### Before Committing

- Run the linter and fix any issues
- Run the test suite and ensure all tests pass
- Do not commit generated files, build artifacts, or secrets

## Working with This Codebase

### When the Stack Is Unknown

If no technology stack is established yet:

1. Ask the user what stack they want before writing code
2. Follow established conventions once any code exists
3. Prefer standard tooling and minimal dependencies

### Adding New Features

1. Read relevant existing files before writing or modifying code
2. Match the style, naming conventions, and patterns already in use
3. Add tests that mirror the existing test structure
4. Update this file if new conventions are introduced

### Modifying Existing Code

- Never remove or rename exports without checking all consumers
- Prefer editing existing files over creating new ones
- Do not add abstractions speculatively — solve the current problem
- Do not add comments to code that is self-explanatory

## Environment Variables

Document all required environment variables here as they are introduced. Example format:

| Variable | Required | Description |
|----------|----------|-------------|
| `DATABASE_URL` | Yes | Connection string for the primary database |
| `SECRET_KEY` | Yes | Secret used for signing tokens |
| `LOG_LEVEL` | No | Logging verbosity (default: `info`) |

## Testing

Document the test commands here once a test framework is chosen. Example:

```bash
# Run all tests
npm test

# Run tests in watch mode
npm run test:watch

# Run a specific test file
npm test -- path/to/file.test.ts
```

## Common Commands

Update this section with real commands as the project is set up:

```bash
# Install dependencies
# (command depends on package manager)

# Start development server
# (command depends on framework)

# Build for production
# (command depends on build tool)

# Run linter
# (command depends on linter)
```

## Code Style and Conventions

Document conventions here as they emerge. Until established:

- Follow the language's standard style guide
- Use the formatter configured in the project (Prettier, Black, rustfmt, etc.)
- Prefer explicit over implicit
- Keep functions small and focused on a single responsibility

## Security Notes

- Never commit secrets, API keys, or credentials
- Never log sensitive user data
- Validate all input at system boundaries
- Sanitize output rendered in HTML contexts

## Updating This File

This file should be updated whenever:

- A new technology or framework is added to the project
- New development conventions are established
- The test or build workflow changes
- New required environment variables are introduced

AI assistants should proactively update this file after making significant structural changes to the project.
