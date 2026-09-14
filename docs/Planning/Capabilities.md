# SplitCents Capabilities

## Product goal
SplitCents is a budgeting app for individuals and couples who want a simple way to plan their money each month. The initial focus is on helping users understand what money is coming in, what is committed to bills, how much can be spent, and how quickly they can reach their savings goals.

## Core capabilities for the initial version

### 1. Income tracking
- Users can add their monthly take-home pay.
- Users can record multiple income sources if needed.
- The app should support a simple personal budgeting workflow for a solo user first.
- Users should be able to review how much income is available for monthly planning.

### 2. Recurring bills
- Users can add recurring bills such as rent, utilities, subscriptions, loan payments, and insurance.
- Each bill should include a name, amount, due date, and frequency.
- The app should support monthly bills by default, while also allowing weekly and annual bills.
- Users should be able to mark bills as paid or unpaid and review upcoming payments.

### 3. Budget planning
- Users can create monthly budget plans by category such as housing, food, transport, fun, and debt payments.
- Budget planning should support both fixed amounts and percentages of income.
- The app should show how much of each budget category remains.
- Users should be able to compare planned spending against actual spending over time.

### 4. Savings goals
- Users can create savings goals such as an emergency fund, holiday, car, or home deposit.
- Each goal should include a target amount, current amount saved, target date, and configured monthly contribution.
- The app should show progress toward the goal and estimate how long it will take to reach it based on the current plan.
- Users should be able to link a savings goal to a portion of their monthly budget.

### 5. Monthly overview and insights
- Users should be able to see a simple monthly summary of income, bills, left-over money, and savings progress.
- The app should highlight when spending is over budget or when savings goals are falling behind.
- Simple reminders for upcoming bills would be useful.
- Overspending alerts should be considered, but they may be limited in the MVP because the app will not yet connect to bank accounts.

## Future capabilities
Once the core experience is working well, the app could grow to include:
- Bank account linking and transaction import (would require open banking and legal considerations)
- Debt payoff planning
- Subscription tracking
- Investment and retirement planning
- A sandbox or forecasting mode for testing “what happens if income drops or bills rise?”
- AI-generated suggestions and financial coaching

## MVP scope decision summary
- Focus: personal budgeting first, with a solo-user workflow that is easier to build and test.
- Bill frequency: monthly by default, with support for weekly and annual bills.
- Budget style: both fixed amounts and percentage-based budgeting.
- Savings goals: configurable monthly contributions, with time-to-goal projections.
- Alerts: reminders for bills are a good MVP fit; overspending detection is possible but limited without bank integration.
- Forecasting: this belongs better in a later sandbox or advanced feature set rather than the first release.
