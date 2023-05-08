# Ask Finance Strategy Development Kit (SDK)

The AskFi trading system consists of an SDK in terms of which domain-specifc logic is specified, a runtime which executes the sensory-motor cycle, and a set of data structures that are produced by executing the runtime.

This document gives an overview about its design and serves as a starting point for everybody interrested in contributing code or developing and operating trading bots.

## SDK

Defines interface types and function signatures used by domain modules to define domain-specific logic.

This library is written in F#, and it can be implemented by any language that compiles into .NET Intermediate Language.

Users can define custom:

- Observation logic (control network communication that produces percepts)
- Interpretations of observations into semantically computable IEML phrases
- Strategies that map Scenes of abstract objects to a decision about actions
- Visualization

[Source Code](https://github.com/BrunoZell/AskFi.Sdk/blob/main/source/AskFi.Sdk.fs)

## Runtime

Which executes domain-implementations defined in terms of the SDK and persists data via data structures defined by the Runtime Data Model.

It is modularized into:

- Observer Group: running one or more `IObserver<'Percept>`
- Perspective Merge: merging observations from multiples observation groups distributed thrughout space into a temporally ordered sequence
- Interpretation: applying custom `Interpreter`-implementations on `CapturedObservation`s.
- Evaluation: expressing preference between `Scene`s.
- Strategy: executes custom `Strategy`-implementations on a `Scene` and the strategies session `Reflection`.
- Analysis: executes custom code that can read information from a `Scene` through the _Scene Query Interface_.

[Source Code](https://github.com/BrunoZell/AskFi.Runtime)

## Runtime Data Model

Defines data structures to store:

- sensory-motor information as produced by observers and brokers
  - observation sequence
  - execution trace
- indexing information:
  - interpretations `captured-observation -> code<interpreter> : moment` with `scene = moment list`
    where `moment['ObjectIdentity : equatable, 'ReferencePhrase] = map[typeid<'ObjectIdentity>, map['ObjectIdentity, 'ReferencePhrase list]]`
  - visualizations `scene -> code<interpreter> : canvas`
  - strategy results `scene -> code<strategy> : decision-sequence-head`

[Source Code](https://github.com/BrunoZell/AskFi.Runtime/blob/main/source/AskFi.Runtime.DataModel/Runtime.DataModel.fs)

## Platform Interface

- Run backtest
- Run live strategy execution
- Run observers
- Run brokers



### SDK

First, at the top left corner, there is the group _SDK_ which is the sensory-motor cycle it attempts to practically model.

There is no explicit code relating to this group. Instead, it stands for the behavior of all external systems not in direct control of the Askbot operator. Examples include exchanges, distributed ledgers, externally hosted REST APIs, or the physical world itself, accessible through sensors only.

### Observation

_Observation_ contains j. (idea) and g. (message). It sits at the transition from the actual into the virtual and takes care of wu. (percept).

For that, the SDK defines **Observers** (type `AskFi.Sdk.IObserver<Percept>`).

Observers communicate with the computer networks related to an external system in an effort to extract and record information about it's internal state.

In case of an exchange, it may listen to order book updates via a WebSocket connection.

In case of a Blockchain, it may connect to the p2p network and records all gossiped transactions and blocks.

### Query

After the actuality of external systems has been mapped to virtual ideas, the system has access to that information.

_Observations_ are the only source of information, but there still is a need to semantically transform and combine that information into higher level ideas and abstractions.

For that, the SDK defined **Queries** (type `AskFi.Sdk.Query`), which are pure functions over a _Perspecive_, which is, broadly speaking, a collection of observations. The result of a query can be though of as the appearance of a new idea.

### Authentication

In a networked setting across multiple subjects, observations must be verified for some domains in order to be useful.

Essentially, it is about relating the virtual representations c. (individual) and x. (body) to their actual counterparts of h. (subject) or p. (object).

This is called authentication as this verification process typically is based on some form of evidence that messages are indeed reflecting reality to a good enough degree.

Such an authentication interface is not yet defined in the SDK. It is scheduled to be implemented once Asknet is being built, the collaborative market making network. For data validation, we are expecting to use the L0 data model from [HGTP (Hypergraph Transfer Protocol)](https://docs.constellationnetwork.io/learn/).

### Execution

Execution in the SDK is implemented via **Brokers** (type `AskFi.Sdk.IBroker<Action>`).

Each broker accepts instance of their `Action`-type and initiate it's execution, potentially collecting evidence of it.

e. (can) is captured by the `Action`-type, which is all possible commands a broker accepts.

i. (do) represents a broker actually executing an action.

The group _Exection_ may also refer to individuals who behave in a certain way. Altough the SDK does not specify any types for that since consciousnuss and decision making in the brain are not really measured. Such actions or inactions of humans are observed just as any other external system.

### Coordination

Coordination, covering o. (want) and a. (commit), is about a structured conversation among subjects to collaborate and steer reality into a desired direction. Esentially it is about placing signs in the environment so that all actions done by subjects are compatible with each other and won't result in conflict.

The coordination protocol is what we call _AskFi_, or _Ask Finance_. You can read the specification of it [here](https://github.com/BrunoZell/ask.fi).

### Consensus

Consensus, grouping y. (know) and u. (express), represents a convergent data strucure that esentially creates common knowledge by merging perspective into a deduplicated view onto reality.

We are looking to implement a variant of the [Convex Convergent Proof of Stake consensus (CPoS) algorithm](https://convex.world/technology?section=Convergent+Proof+of+Stake) that can accomodate user defined values, i.e. can address all possible wants across all possible perspectives.

### Human Domain

There is no code form the SDK that corresponds to the _Human Domain_.

It moreso represents a placeholder for all possible domain models that aim to improve the global human experience.

## Implementation

There are two aspects of implementing this:

- The SDK itself, which poses as a bridge between the IEML ontology described above, and executable code that the ecosystem targes.
- The [Runtime](https://github.com/BrunoZell/AskFi.Runtime), which is the software that orchestrates the execution of everything that is defined in terms if the SDK.

The outline described in this document focusses on how the types defined in F# relate to the categorization described above.

As the ultimate reference, take a look at the [SDK type defintions](../source/AskFi.Sdk.fs) themself.

### Observer Module

This module contains these SDK types:

- `AskFi.Sdk.IObserver<'Percept>`
- `AskFi.Sdk.Observation<'Percept>`

The task of this subsystem is to accept one or more instances if `IObserver<'Percept>` and sequence them into a `Perspective`.

[Observers](./observations.md) scrape the external world and produce strongly-typed _Percepts_. Observations happen spontaneously. In order to be able to fully deterministically execute all downstream components, it is essential to record the sequence of their occurence at the time of observation.

Therfore, all observations of a single _Observer_ instance are sequenced into an _Observation Sequence_ what essentially boils down to a linked list.

And further, all updates to those _Observation Sequences_ are then merged into a single _Perspective Sequence_, which sequences observations accross all _Observers_ in the session.

Each `Pespective` is represented by such an _Perspective Sequence_ under the hood. This subsystem has a stream of those _Perspectives_ as an output.

### Perspective Module

This module contains these SDK types:

- `AskFi.Sdk.Perspective`
- `AskFi.Sdk.IPerspectiveQueries`
- `AskFi.Sdk.Query<'Parameters, 'Result> = 'Parameters -> Perspective -> 'Result`

The task of this subsystem is to run custom .NET code in the form of `Query = 'Parameters -> Perspective -> 'Result` to aggregate and/or semantically transform observations of a `Perspective` into user defined types.

Note that this process does not add any information to the system. It just transforms the shape of available information into usually more useful data types.

Via `Perspective.Query`, an instance of `IPerspectiveQueries` can be obtained that serves as a window into the basket of all observations available to the system. It defines functions like `latest<'Percept>` to obtain the latest observation of percept classification `'Percept`. Or `since<'Percept>` which iterates all observations of the requested percept type since a passed timestamp.

[Queries](./queries.md) can call other _Queries_ during their execution. The Runtime ensures that results are adequately cached such that the domain modeller can focus on the transformations themself and not on the performance of those implementations.

### Strategy Nodule

This module contains these SDK types:

- `AskFi.Sdk.Reflection`
- `AskFi.Sdk.IReflectionQueries`
- `AskFi.Sdk.Decision`
- `AskFi.Sdk.ActionInitiation`
- `type Decide = Reflection -> Perspective -> Decision`

[Strategies](./strategies.md) that compose decision-trees out of _Queries_ (reactive conditions), mapping each case to a _Decision_, which is a (possibly empty) set of _Actions_ to initiate.

Actions that have been decided on to be initiated are sent to the _Execution Module_.

### Execution Module

This subsystem touches these SDK types:

- `AskFi.Sdk.IBroker<'Action>`

[Brokers](./brokers.md) that take an _Action_ initiation and send according network IO to external computer networks, essentially executing the requested _Action_.
