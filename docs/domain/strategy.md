# Strategy

A _Strategy_ is a function of the following form, as defined by the type `AskFi.Sdk.Strategy`:

```fsharp
type Strategy =
    Reflection -> Context -> Decision
```

with a decision defined as:

```fsharp
type ActionInitiation = {
    Action: obj
    Type: System.Type
}

type Decision =
    | Inaction
    | Initiate of Initiatives:ActionInitiation array
```

A strategy maps any given `Perspective` to a `Decision`, where it analyzes object instances from a `Context` that was obtained by interpreting the `Perspecive` according to the _Interpreters_ requested by the strategy implementation.

## Live Execution

The platform allows the execution of a strategy on live data with actions that have been decided on being forwarded to running _Broker_-Instances.

The strategy function is executed by the _Runtime_ every time new observations appear.

If the decision is to initate an _Action_, that request is handed off to the _Broker_ for the according _Action_-type.

## Implementation Requirements

- Code must only reference data from passed `Reflection` and `Context`.
- Object instance referenes from the `Context` are read via the query interface defined as `AskFi.Sdk.IContextQueries`.
- Function must be logically pure. Resolving content-addressed data is fine.
