# Ask Finance Strategy Development Kit (SDK)

## Table Of Contents

To understanding the approach we are going after:

- [Sensory-Motor Cycle](sensory-motor-cycle.md)
- [Ontology](ontology.md)

Roadmap on how to achieve collective intelligence:

- [Build Phases](build-phases.md)
- [Adoption Phases](adoption-phases.md)

## Technical Overview

The AskFi trading system consists of three parts:

- an SDK in terms of which domain-specifc logic is specified.
- a 
- a set of data structures that are produced by executing the runtime.

This document gives an overview about its design and serves as a starting point for everybody interrested in contributing code or developing and operating trading bots.

## SDK

The [SDK](https://github.com/BrunoZell/AskFi.Runtime) defines interface types and function signatures used by domain modules to define domain-specific logic.

Documentation about how to implement custom domains are [here](./domain/domain-modelling.md).

## Runtime and Data Model

The [Runtime](https://github.com/BrunoZell/AskFi.Runtime) is the software that orchestrates the execution of the sensory-motor cycle and its domain implementations as defined in terms if the SDK.

While the _Runtime_ is executing, it produces a data trace in the form of the runtimes _Data Model_.

Documentation about implementation details are [here](./runtime/overview.md).

## Platform

The _Runtime_ itself just implements the behavior that executes domain-specific logic. It still needs to be hosted somewhere. That is taken care of by a _Platform Implementation_.

The _Platform_ serves the infrastructure the runtime depends on, which is:

- Content-Addressed persistence
- Messaging between _Runtime Modules_.
- Networking with external computer systems
- Computing infrastructure to execute code

It also exposes a user interface through with the operator commands what domain implementations should execute. In the default platform implementation, those actions are exposed via a REST API:

- Run observers
- Run brokers
- Run live strategy execution
- Run custom analysis code
  - Backtest a strategy
  - Backtest expectations
  - Compute visualizations
  - Export analysis results into a custom format

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

### Strategy Module

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
