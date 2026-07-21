# SplitCents UI Planning

## Overall Direction
SplitCents should start as a single responsive web app that works well on mobile and desktop. The UI should feel simple, calm, and focused on money clarity rather than complicated controls.

The initial plan is:
- Build one app for web first
- Make it responsive so it works on phones, tablets, and desktops
- Use a mobile-first layout, then expand for larger screens
- Keep the experience consistent across devices
- Consider a native mobile app later only if device-specific features become necessary

## Recommended UI Approach
- Use React with TypeScript
- Prefer Next.js for routing, layout, and a strong modern setup
- Use a component library such as shadcn/ui or MUI
- Use Tailwind CSS for styling
- Design for a mobile-first experience, with desktop enhancements such as wider layouts and side navigation

## Primary UX Goals
The UI should help users:
- See their current financial position quickly
- Add and review transactions with minimal friction
- Understand spending patterns and progress toward savings goals
- Feel confident managing money with a partner or household
- Enjoy a calm and approachable experience rather than a cluttered finance tool

## Likely Pages

### 1. Welcome / Onboarding
Purpose: Introduce the app and guide first-time users.

Features:
- Short welcome message
- Explain the app’s purpose
- Sign up / log in options
- Optional setup steps for goals, accounts, and household preferences

### 2. Authentication Pages
Purpose: Allow users to create accounts and sign in securely.

Features:
- Login form
- Register form
- Password reset flow
- Clear validation and friendly error states
- Option for social sign-in later if needed

### 3. Dashboard
Purpose: Give users a quick snapshot of their finances.

Features:
- Current balance summary
- Monthly income vs spending
- Upcoming bills or recurring payments
- Recent transactions
- Savings progress at a glance
- Quick actions such as “Add transaction” and “Create budget”

### 4. Transactions Page
Purpose: Let users manage everyday money movement.

Features:
- List of transactions with date, amount, category, and account
- Search and filter options
- Add, edit, and delete transaction actions
- Category selection
- Optional recurring transaction setup
- Positive and negative transaction styling

### 5. Accounts Page
Purpose: Show the user’s financial buckets.

Features:
- List of accounts such as checking, savings, cash, and investments
- Current balance for each account
- Add/edit account details
- Simple account grouping and summary views

### 6. Budgets Page
Purpose: Help users plan spending by category.

Features:
- Monthly budget overview
- Category-by-category spending progress
- Visual progress bars or indicators
- Budget creation and editing
- Alerts when spending is nearing or exceeding limits

### 7. Goals / Savings Page
Purpose: Help users plan for meaningful financial targets.

Features:
- Savings or investment goals
- Target amount and current progress
- Visual progress indicators
- Option to assign money toward a goal
- Timeline or deadline support

### 8. Insights / Reports Page
Purpose: Help users understand spending trends.

Features:
- Charts for spending over time
- Breakdown by category
- Monthly and yearly views
- Simple trends such as rising or falling spending
- Lightweight reporting for the first version

### 9. Shared / Household Page
Purpose: Support couples or shared household planning.

Features:
- Shared budget and transaction visibility
- Household members overview
- Optional permission-based access
- Shared goals or shared categories

### 10. Settings / Profile Page
Purpose: Manage personal preferences and account details.

Features:
- Profile editing
- Notification preferences
- Currency and locale settings
- Theme preferences
- Logout and account management options

## Suggested Navigation Structure

### Mobile
- Bottom navigation with 4–5 core sections:
  - Dashboard
  - Transactions
  - Budgets
  - Goals
  - More

### Desktop
- Left sidebar navigation with the same sections
- Wider content areas for lists, charts, and summaries
- Persistent top bar for search and actions

## MVP Scope for Version 1
The initial version should focus on the essentials:
- Authentication
- Dashboard
- Transactions
- Accounts
- Budgets
- Basic settings

This gives users the core financial workflow without overcomplicating the first release.

## Later Enhancements
Once the core flow is working, the app could expand with:
- Advanced charts and forecasting
- Recurring bills and reminders
- Payment integrations
- Shared household collaboration tools
- Mobile app packaging through React Native or Expo if native features are needed

## Design Principles
- Keep the interface simple and low-friction
- Prioritise clarity over cleverness
- Use strong visual hierarchy for money data
- Make frequent actions easy, especially adding transactions
- Design for confidence and reassurance
