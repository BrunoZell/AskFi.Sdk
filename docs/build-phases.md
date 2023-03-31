# Build Phases

The SDK is not yet done. It will gradually model more domains required to implmenet a Collective Intelligence system.

## Phase A: Sensory-Motor Cycle ✅

First version of the SDK defining:

- Observers
- Queries
- Strategies
- Actions

Enables:

- Askbot: An algo-trading system for expert strategies

## Phase B: Expectations ♻️

Defining a format of expectations of the behavior of external systems. Possibly conditioned under an action.

It will be defined as a Finite Markov Decision Process to be compatible with typical reinforcement learning methods.

Enables:

- Backtesting of Askbot strategies
- Simulation of external systems

## Phase C: User Defined Value

Allow user to define a value function in a first principle way, referring to a _Perspective_.

By defining both _Expectation_ and _Value_, we have a formal description of _Expected Value_. And due to it's structure, it works with **all possible values across all possible domain models**.

This enables **automatic strategy development**:

- Automatic domain modelling through causal inference. We are looking to implement a variant of [AutumnSynth](https://www.basis.ai/blog/autumn/) on an IEML ontology.
- Automated strategy optimization through a variant of model-based reinforcement learning.

## Phase D: Consensus

It is modelled as a CRDT (conflict-free replicated data type) that esentially creates common knowledge by merging multiple perspectives into a deduplicated view onto reality, considering [HGTP L0](https://docs.constellationnetwork.io/learn/) authentication mechanisms.

We are looking to implement a variant of the [Convex Convergent Proof of Stake consensus (CPoS) algorithm](https://convex.world/technology?section=Convergent+Proof+of+Stake) that uses the SDKs _User Defined Values_ as a fork choice rules in case of a conflict, and for the detection of a conflict in the first place.

Enables:

- Asknet: A collaborative market making botnet facilitating a p2p exchange
- SemanticOs: A personal knowledge management system with a common knowledge database

## Phase E: Coordination

Convergent conversation about who does what.

Coordination, covering o. (want) and a. (commit), is about a structured conversation among subjects to collaborate and steer reality into a desired direction. Esentially it is about placing signs in the environment so that all actions done by subjects are compatible with each other and won't result in conflict.

The coordination protocol is what we call _AskFi_, or _Ask Finance_. You can read the specification of it [here](https://github.com/BrunoZell/ask.fi).

Enables:

- A non-monetary economy more efficient and effective than money:
  - More efficient in that it can provide the same service at a lower cost
  - More effective in that it has a tendency to offer a better service than every other economy

At this point, the SDK is mostly complete. The project is mature and now focusses on ecosystem growth. For that, we lay out the [Adoption Phases](./adoption-phases.md).
