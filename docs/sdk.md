# Ask Finance Strategy Development Kit (SDK)

The AskFi SDK is a library of types written in F# backed by an ontology defined in IEML.

The ontology defines the trading systems abstract data model.

The code library provides the necessary interface types to built strategy and analysis code out of.

This document gives an overview about its design and serves as a starting point for everybody interrested in contributing code or developing and operating trading bots.

## Goals

The AskFi SDK must be able to address all possible strategies across all possible external systems.

From a traders perspective, this translate into _"addressing all possible trading strategies across all possible auction systems"_.

This is important so the user is not constrained by the system in a deal-breaking way. It also escapes a lot of re-engineering efforts and maximizes code reuse and stability.

Furthermore, it allows to author fairly generalized algorithms to optimize user-defined values, which is esential for automated counterparty discovery in the Ask Finance protocol.

## Structure

Overall the SDK models the sensory-motor cycle. But in a way that allows for reusable infrastructure and tooling built for it.

### Sensory-Motor Cycle

In [IEML](https://intlekt.io/ieml/), the sensory-motor cycle can be found in the Virtual/Actual Binary Symmetry, also called _Interaction Phases_ and looks like this:

![Interaction Phases](./images/interaction-phases.png)

To understand its meaning, we must relate to the fundamental sensory-motor cycle. Let us begin with the virtual act of orientation, which takes place in the mind (wo.). From this virtual place we act (wa.). As a result, our current environment is transformed and a new reality manifests itself (we.). Finally, from the actuality of the external event, we perceive (wu.) by a return to interiority. [[source](https://intlekt.io/25-basic-categories/)]

Note the two intertwined oppositions:

- wa. act (inside → outside)   |   wu. perceive (outside → inside).
- wo. orient (inside → inside) |   we. appear (outside → outside)

For more context on how this relates to machine learning and artificial intelligence, take a look at the **IEML Neuro-Semantic Architecture** described in [this essay](https://intlekt.io/2022/01/18/ieml-towards-a-paradigm-shift-in-artificial-intelligence/).

### Ontology

Consistes of:

- [Observers](./observations.md) that scrape the external world and produce strongly-typed _Perceptions_,
- [Queries](./queries.md) that aggregate and/or semantically transform those _Perceptions_ into other types (_Query Results_, or: _Appearances_),
- [Strategies](./strategies.md) that compose decision-trees out of _Query Results_ (reactive conditions), mapping each case to a _Decision_, which is a (possibly empty) set of _Actions_ to initiate.
- [Brokers](./brokers.md) that take an _Action_ initiation and send according network IO to external computer networks, essentially executing the requested _Action_.

A [Runtime](https://github.com/BrunoZell/AskFi.Runtime) is used to compose the execution of _Observers_, _Queries_, _Strategies_, and _Brokers_.

### Implementation

See the code [here](../source/AskFi.Sdk.fs).
