# SplitCents Implementation Plan

## Approach
This plan breaks the MVP into a sequence of stages that can be treated as sprints or workflow phases. The goal is to deliver the core budgeting experience incrementally while keeping the scope manageable.

## Phase 1: Foundation and setup
**Goal:** Build the underlying structure for the app.

**Scope:**
- Create the solution structure and project layout
- Set up the API, core domain, and infrastructure layers
- Define the initial data model for users, income, bills, budgets, and savings goals
- Create basic authentication and user registration flow

**Deliverable:** A working base application with a user account and persisted data model.

## Phase 2: Income and recurring bills
**Goal:** Let users enter their monthly income and recurring obligations.

**Scope:**
- Add income entry screens and API endpoints
- Add recurring bill creation, editing, and deletion
- Support monthly, weekly, and annual frequencies
- Add bill status tracking such as paid and unpaid

**Deliverable:** Users can plan around income and regular bills.

## Phase 3: Budget categories and monthly planning
**Goal:** Let users create a simple monthly budget.

**Scope:**
- Add budget categories and amounts
- Support fixed amounts and percentage-based allocations
- Show remaining budget per category
- Create a monthly view that combines income, bills, and budgets

**Deliverable:** Users can create a practical monthly spending plan.

## Phase 4: Savings goals
**Goal:** Add goal-based saving to the experience.

**Scope:**
- Create savings goals with target amount, current saved amount, and target date
- Allow monthly contribution settings
- Show projected time to goal and progress percentage
- Link savings goals to the budgeting flow where appropriate

**Deliverable:** Users can track personal savings progress alongside their budget.

## Phase 5: Dashboard and polish
**Goal:** Improve clarity and usability of the core experience.

**Scope:**
- Create a simple dashboard with summary cards and upcoming bills
- Improve validation and error handling
- Refine the user experience for creating and editing plans
- Add basic reminders for upcoming payments

**Deliverable:** A more complete MVP experience that feels useful and coherent.

## Phase 6: MVP release and feedback
**Goal:** Prepare the product for initial use and review.

**Scope:**
- Test the end-to-end flow
- Fix bugs and improve edge cases
- Gather feedback from early users
- Decide what should be carried into the next phase

**Deliverable:** A release-ready MVP for solo personal budgeting.

## Suggested sprint breakdown
If you want to turn this into a board, a simple sprint structure could be:

- Sprint 1: Foundation and user accounts
- Sprint 2: Income and recurring bills
- Sprint 3: Budget categories and monthly planning
- Sprint 4: Savings goals
- Sprint 5: Dashboard, polish, and reminders
- Sprint 6: Testing, refinement, and release

## Recommended backlog categories
- Must have for MVP
- Nice to have for MVP polish
- Post-MVP / future ideas

## Notes for the board
A simple board could use columns such as:
- Backlog
- Planned
- In progress
- Review
- Done

This structure should make it easy to translate the plan into tickets and track progress over time.
