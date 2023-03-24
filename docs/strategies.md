# Strategies

A _Strategy_ is a function of the form `StrategyReflection -> Perspective -> Decision` (type `AskFi.Sdk.Decide`).

It's a special case query that is executed by the _Askbot Runtime_ every time new observations appear.

If the decision is to initate an _Action_, that request is handed off to the _Broker_ for the according _Action_-type.

The same implementation requirements as for _Queries_ apply:

- Code must only reference data from passed `StrategyReflection` and `Perspective`.
- `Perspective` is read via the query interface defined as `Sdk.IPerspectiveQueries`.
- Function must be logically pure. Resolving content-addressed data is fine.
