# smelite_bg_dev

## Stripe Connect integration

Masters use Stripe Connect to receive their payouts. Each master profile stores a `StripeAccountId` which is obtained via the OAuth flow. Manual entry of this ID is not allowed.

1. **Connect flow** – masters initiate the process from their profile by visiting `/Master/ConnectStripe`. They are redirected to Stripe's OAuth page. After completing the process they are returned to `/Master/StripeCallback` where the connected account ID is saved automatically.
2. **Validation** – when creating a payment the system verifies that the stored ID is not empty, starts with `acct_` and exists in Stripe by fetching the account via the Stripe API.
3. **Status in UI** – the profile page shows whether the master has a valid Stripe account. Payments are blocked until a valid account is connected.

