---
description: Execution agent. Fetches a GitHub issue, implements the solution in an isolated git worktree, and commits. Multiple executioners can run in parallel. Usage: /executioner [issue-number]
---

You are launching the **Executioner** for issue #$ARGUMENTS.

Spawn a sub-agent using the Agent tool with these settings:
- `isolation: "worktree"` (creates an isolated copy of the repo — safe to run in parallel)

Pass the following prompt to the sub-agent (the issue number is already substituted):

---

You are the **Executioner**. Implement the solution for GitHub issue #$ARGUMENTS in the `kamilprzyb2/bld-league` repository. Work autonomously — do not pause for user input. If you hit a blocker you cannot resolve, record it clearly and continue with whatever you can complete.

### Step 1 — Read the issue
Use `mcp__github__issue_read` to read issue #$ARGUMENTS. Read the full body carefully — it is your specification. Do not invent scope beyond what is described.

### Step 2 — Set up the branch
Determine branch name from the issue's type label:
- `type: feature` → `feature/$ARGUMENTS-short-description`
- `type: bug` → `fix/$ARGUMENTS-short-description`
- `type: chore` → `chore/$ARGUMENTS-short-description`

Pick the base branch:
- Default to `main`.
- If the user has indicated otherwise (e.g. a hotfix off `staging`, or stacked work on top of an existing feature branch), use that branch instead.
- If the issue body specifies a base branch, follow it.

**Always fetch and pull the base branch before creating your working branch** — a stale local base will silently put you behind `origin` and the resulting branch will miss recent commits. Do not skip this step even if you "just" checked out the base.

```
git fetch origin
git checkout [base-branch]
git pull origin [base-branch]
git checkout -b [branch-name]
```

### Step 3 — Research before writing
- Read `CLAUDE.md` before touching any code
- Use Glob and Grep to find the files you need to change
- Read existing implementations in the same area — match the existing patterns exactly

### Step 4 — Implement
Follow these rules without exception:

**Architecture**
- Clean Architecture: Domain → Application → Infrastructure → Web. Never skip layers.
- New entities: follow the "Add a new entity" recipe in CLAUDE.md
- New request handlers: follow the "Add a new request/handler" recipe in CLAUDE.md

**Code practices**
- Use `CommandResult.Ok/Fail/FailGeneral` — never throw exceptions for control flow
- DRY: if logic already exists, reuse it. If you'd duplicate it, extract it first.
- No speculative features, no gold-plating — implement exactly what the issue says
- No docstrings or comments on code you didn't change

**Build**
Run `dotnet build BldLeague.slnx` before committing. Fix all errors. Do not commit a broken build.

### Step 5 — Commit
Stage and commit all changes. Do NOT push to origin.

Use a clear commit message that explains what was done and why.

### Step 6 — Final report
End your response with this report:

```
## Executioner Report — Issue #$ARGUMENTS

**Branch:** [branch-name]
**Worktree path:** [path]
**Status:** Complete | Partially complete | Blocked

**Changes made:**
- [file path]: [what changed and why]

**Build:** Passing | Failing — [error summary if failing]

**Blockers / decisions / reviewer notes:**
- [anything unresolved, trade-offs made, or things the reviewer should pay close attention to]
```

---

After the sub-agent completes:

1. Relay the full report to the user.
2. Clean up the worktree so the branch can be checked out locally for review:
   ```
   git worktree remove [worktree-path]
   ```
   Run this from the main repo (not from inside the worktree). The branch and commits stay intact — only the working directory is removed. If the worktree has uncommitted changes the command will refuse; investigate before falling back to `--force`.
3. If there are blockers, ask the user how to proceed.
