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

## Sensory-Motor Cycle

Overall the SDK models the sensory-motor cycle. But in a way that allows for reusable infrastructure and tooling built for it.

In [IEML](https://intlekt.io/ieml/), the sensory-motor cycle can be found in the Virtual/Actual Binary Symmetry, also called _Interaction Phases_ and looks like this:

![Interaction Phases](./images/interaction-phases.png)

To understand its meaning, we must relate to the fundamental sensory-motor cycle. Let us begin with the virtual act of orientation, which takes place in the mind (wo.). From this virtual place we act (wa.). As a result, our current environment is transformed and a new reality manifests itself (we.). Finally, from the actuality of the external event, we perceive (wu.) by a return to interiority. [[source](https://intlekt.io/25-basic-categories/)]

Note the two intertwined oppositions:

- wa. act (inside → outside)   |   wu. perceive (outside → inside).
- wo. orient (inside → inside) |   we. appear (outside → outside)

For more context on how this relates to machine learning and artificial intelligence, take a look at the **IEML Neuro-Semantic Architecture** described in [this essay](https://intlekt.io/2022/01/18/ieml-towards-a-paradigm-shift-in-artificial-intelligence/).

### Appear - External Systems

Represents external systems not in direct control of the Askbot operator. Examples include exchanges, distributed ledgers, externally hosted REST APIs, or the physical world itself, accessible through sensors only.

All these systems have some inherent behavior. An exchange, for example, does order matching in a fairly predictable way. But the rules that govern those systems and the state they are in are only indirectly accessible to an Askbot instance.

Rules of behavior can be inferred from public documentation, scientific experiments, or open source code.

Historic system states can be inferred from observing those systems through their public interaction interface.

### Perceive - Observations

To better reason about the state of external systems, the SDK defines **Observers**.

Observers communicate with the computer networks related to an external system in an effort to extract and record information about it's internal state.

In case of an exchange, it may listen to order book updates via a WebSocket connection.

In case of a Blockchain, it may connect to the p2p network and records all gossiped transactions and blocks.

### Orient - Semantic Transformations

It is important to realize that those recorded perceptions are not synonymous with the state of the external system. In most cases, the state itself is not directly accessible.

Therefore, there is some reconstruction to be done to convert recorded perceptions into a local imaginative state of the system. for this, the SDK defined **Queries**.

This is done on a best-effort bases with pragmatism in mind. Only relevant state must be computed, as defined by the trading strategy. Secondly, the rules with which that state is computed may not exactly match the behavior of the system. In some cases this imprecision does not matter. In other cases it does, where it is an iterative process to fine tune the rules so that all recorded observations are in line with the modelled behavior (i.e. there are no inconsistencies).

### Act - Causal Interventions

Finally, we have the action phase of the cycle. Acting is the way to have causal influence on external systems. What actions are available and what influence they end up having depends on the external system.

For example, a limit order can be sent to an exchange. Although there are certain expectations of what might happen, it really is up to the external system to decide. And whatever ends up manifesting, it can again only indirectly be observed through perceptions. And so the sensory-motor cycle repeats.

To manage action execution, the SDK defined **Brokers**.

## Ontology

For the AskFi trading system, we further break up the four phases  **manifestation -> perception -> orientation -> action** by means of the two Hexads _Actors_ (green) and _Actions_ (yellow):

![Lowercase Letters](./images/letters.png)

So far, this is just standard IEML and nothing unique to AskFi.

Read more about the six semantic primitives **Virtual, Actual, Sign, Being, Thing** [here](https://intlekt.io/semantic-primitives/).

Read more about the 25 lowercase symbols in IEML [here](https://intlekt.io/25-basic-categories/).

While these are useful and ubiquitus categories, for the sake of implementation, AskFi groups the IEML alphabet into eight domains of implementation: **Observation, Query, Authentication, Coordination, Execution, Consensus, SDK, Human Domain**:

![SDK Components](./images/components.png)

### SDK

First, at the top left corner, there is the group _SDK_ which is the sensory-motor cycle it attempts to practically model.

There is no explicit code relating to this group. Instead, it stands for the behavior of all external systems not in direct control of the Askbot operator. Examples include exchanges, distributed ledgers, externally hosted REST APIs, or the physical world itself, accessible through sensors only.

### Observation

_Observation_ contains j. (idea) and g. (message). It sits at the transition from the actual into the virtual and takes care of wu. (perception).

For that, the SDK defines **Observers** (type `AskFi.Sdk.IObserver<Perception>`).

Observers communicate with the computer networks related to an external system in an effort to extract and record information about it's internal state.

In case of an exchange, it may listen to order book updates via a WebSocket connection.

In case of a Blockchain, it may connect to the p2p network and records all gossiped transactions and blocks.

### Query

After the actuality of external systems has been mapped to virtual ideas, the system has access to that information.

_Observations_ are the only source of information, but there still is a need to semantically transform and combine that information into higher level ideas and abstractions.

For that, the SDK defined **Queries** (type `AskFi.Sdk.Query`), which are pure functions over a _Perspecive_, which is, broadly speaking, a collection of observations. The result of a query can be though of as the appearance of a new idea.

### Authentication

In a networked setting across multiple subjects, observations must be verified for some domains in order to be useful.

Esentially, it is about relating the virtual representations c. (individual) and x. (body) to their actual counterparts of h. (subject) or p. (object).

This is called authentication as this verification process typically is based on some form of evidence that messages are indeed reflecting reality to a good enough degree.

Such an authentication interface is not yet defined in the SDK. It is scheduled to be implemented once Asknet is being built, the collaborative market making network.

### Execution

### Coordination

The coordination protocol is what we call AskFi, or Ask Finance. You can read the specification of it [here](https://github.com/BrunoZell/ask.fi).

### Consensus

Consensus, grouping y. (know) and u. (express), represents a convergent data strucure that esentially creates common knowledge by merging perspective into a deduplicated view onto reality.

We are looking to implement a variant of the [Convex Convergent Proof of Stake consensus (CPoS) algorithm](https://convex.world/technology?section=Convergent+Proof+of+Stake) that can accomodate user defined values. i.e. can address all possible wants across all possible perspectives.

### Human Domain

There is no code form the SDK that corresponds to the _Human Domain_.

It moreso represents a placeholder for all possible domain models that aim to improve the global human experience.

## Implementation

Consists of:

- [Observers](./observations.md) that scrape the external world and produce strongly-typed _Perceptions_,
- [Queries](./queries.md) that aggregate and/or semantically transform those _Perceptions_ into other types (_Query Results_, or: _Appearances_),
- [Strategies](./strategies.md) that compose decision-trees out of _Query Results_ (reactive conditions), mapping each case to a _Decision_, which is a (possibly empty) set of _Actions_ to initiate.
- [Brokers](./brokers.md) that take an _Action_ initiation and send according network IO to external computer networks, essentially executing the requested _Action_.

A [Runtime](https://github.com/BrunoZell/AskFi.Runtime) is used to compose the execution of _Observers_, _Queries_, _Strategies_, and _Brokers_.

See the code [here](../source/AskFi.Sdk.fs).
