# Ask Finance

This document gives an overview about the design of _Ask Finance_ and serves as a starting point for everybody interrested in contributing code or developing and operating _Askbots_.

To understanding the approach we are going after:

- [Sensory-Motor Cycle](sensory-motor-cycle.md)
- [Ontology](ontology.md)

Roadmap on how to achieve collective intelligence:

- [Build Phases](build-phases.md)
- [Adoption Phases](adoption-phases.md)

## Technical Overview

The AskFi trading system consists of three parts:

## SDK: Strategy Development Kit

The [SDK](https://github.com/BrunoZell/AskFi.Sdk) defines interface types and function signatures used by domain modules to define domain-specific logic.

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
