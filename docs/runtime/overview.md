# Runtime Internals

[Source Code](https://github.com/BrunoZell/AskFi.Runtime)

The _Runtime_ is modularized. Each modules implements a sub-problem while sharing the same data strutures to pass data around.

## Observer Group

Takes one or more `Sdk.IObserver<'Percept>`-instances of possible different _Percepts_.

Starts pulling observations from all _Observers_ and processes them sequentially as they appear:

Whenever an _Observer_ emits a new `Sdk.Observation<'Percept>`, it is processed like this:

1. Capture current timestamp as of runtime clock
2. Serialize into `DataModel.CapturedObservation`
3. Build new node in _Observation Sequence_: `DataModel.ObservationSessionSequenceHead`
4. Emit message `NewObservation` linking the newest._Observation Sequence_.

## Observation Pool

Maintains a reference to an `DataModel.ObservationPool` with the most unique `DataModel.CapturedObservations` included.

Merging observations from multiples observation groups distributed thrughout space into a temporally ordered sequence.

Whenever a message `NewObservation` is received:

- Received `DataModel.CapturedObservations` is merged into the latest `DataModel.ObservationPool`.
- If `DataModel.ObservationPool` changed, emit message `NewPerspective` linking the newest _Observation Pool_.

Whenever a message `NewPerspective` is received:

- Received `DataModel.ObservationPool` is merged into the latest `DataModel.ObservationPool`.
- If `DataModel.ObservationPool` changed, emit message `NewPerspective` linking the newest _Observation Pool_.

## Analysis

Two flavors:

- Live: Running strategy decisions immediately after observations come in.
- Replay: Running strategy decisions on historically recorded observations for analysis and optimization.

### Live

Whenever a message `NewPerspective` is received:

- It is merged with the latest observation pool from memory.
- If the merge produced a new perspective:
- Run strategy on all newly sequenced observations
- Emit message `NewDecision` for every non-inaction-decision, linking the newest decision index CRDT.

### Replay

Given:

- a `DataModel.PerspectiveSequenceHead`
- an `Sdk.Strategy`
- all `Sdk.Interpreter` and `Sdk.Query` required by the strategy

Iterates all _Captured Observation_ in the _Perspecive Sequence_ from latest to earliest and runs the _Strategy_ on it.

The `Scene` passed into the _Strategy_ is a lazily evaluated and cached wrapper around the _Perspective Sequence_.

While the strategy is executing, it calls `Sdk.ISceneQueries` to inspect object instance references. Each call references an `Sdk.Interpreter`. The implementation behind `Sdk.ISceneQueries` iterates through the _Perspective Sequence_ data structure, runs _Interpreters_ on `Sdk.Observation`s, and caches results.

Scene index:

```
cid<captured-observation> -> code<interpreter> =
  cid<map<typeid['Identity], map<'Identity, 'Reference list>>>
```

Decision index:

```
cid<perspective-sequence-head> -> code<strategy> =
  cid<decision>
```

The replay module is producing CRDTs of those indices, building up decisions over time.

## Broker Group

Whenever a message `NewDecision` is received, do for each action in decision:

1. Capture current timestamp as of runtime clock as execution start timestamp
2. Route to according `IBroker<'Action>` and wait for completion
3. Build new node in _Execution Sequence_: `DataModel.  ExecutionSequenceHead`.
4. Emit message `ActionExecuted` linking the newest _Execution Sequence_.

## Strategy Declaration Pool

- Asks in the form of `Scene -> Fulfilled | Unfulfilled`.
- Offers in the form of `{ Additions: ConditionalAction, Removals: ConditionalAction }`.
- Accept in the form of `ChangeOfPlan` that has a condition on other subjects action trace, or an according observation.

Each subject has:

- A declared strategy, as a list of conditional actions+proofspec
- A declared value set, as a list of asks
- A historical execution trace
