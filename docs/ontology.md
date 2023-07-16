# Collective Intelligence Ontology

The four phases of the [sensory-motor cycle](sensory-motor-cycle.md)  **manifestation -> perception -> orientation -> action** can be further broken up into the two Hexads _Actors_ (green) and _Actions_ (yellow):

![Lowercase Letters](./images/letters.png)

So far, this is just standard IEML and nothing unique to AskFi.

Read more about the six semantic primitives **Virtual, Actual, Sign, Being, Thing, Emptiness** [here](https://intlekt.io/semantic-primitives/).

Read more about the 25 lowercase symbols in IEML [here](https://intlekt.io/25-basic-categories/).

While these are useful and ubiquitus categories, for the sake of implementation, AskFi groups the IEML alphabet into eight domains of implementation: **SDK, Observation, Query, Authentication, Execution, Coordination, Consensus, and Human Domain**:

![SDK Components](./images/components.png)

First, at the top left corner, there is the group _SDK_ which is the sensory-motor cycle it attempts to practically model.

There is no explicit code relating to this group. Instead, it stands for the behavior of all external systems not in direct control of the Askbot operator. Examples include exchanges, distributed ledgers, externally hosted REST APIs, or the physical world itself, accessible through sensors only.

## Observation

_Observation_ contains `j.` (idea) and `g.` (message). It sits at the transition from the actual into the virtual and takes care of wu. (percept).

For that, the SDK defines **Observers** (type `AskFi.Sdk.IObserver<Percept>`).

Observers communicate with the computer networks related to an external system in an effort to extract and record information about it's internal state.

In case of an exchange, it may listen to order book updates via a WebSocket connection.

In case of a Blockchain, it may connect to the p2p network and records all gossiped transactions and blocks.

## Query

After the actuality of external systems has been mapped to virtual ideas, the system has access to that information.

_Observations_ are the only source of information, but there still is a need to semantically transform and combine that information into higher level ideas and abstractions.

For that, the SDK defined **Queries** (type `AskFi.Sdk.Query`), which are pure functions over a _Perspecive_, which is, broadly speaking, a collection of observations. The result of a query can be though of as the appearance of a new idea.

## Authentication

In a networked setting across multiple subjects, observations must be verified for some domains in order to be useful.

Essentially, it is about relating the virtual representations `c.` (individual) and `x.` (body) to their actual counterparts of `h.` (subject) or `p.` (object).

This is called authentication as this verification process typically is based on some form of evidence that messages are indeed reflecting reality to a good enough degree.

Such an authentication interface is not yet defined in the SDK. It is scheduled to be implemented once Asknet is being built, the collaborative market making network. For data validation, we are expecting to use the L0 data model from [HGTP (Hypergraph Transfer Protocol)](https://docs.constellationnetwork.io/learn/).

## Execution

Execution in the SDK is implemented via **Brokers** (type `AskFi.Sdk.IBroker<Action>`).

Each broker accepts instance of their `Action`-type and initiate it's execution, potentially collecting evidence of it.

`e.` (can) is captured by the `Action`-type, which is all possible commands a broker accepts.

`i.` (do) represents a broker actually executing an action.

The group _Exection_ may also refer to individuals who behave in a certain way. Altough the SDK does not specify any types for that since consciousnuss and decision making in the brain are not really measured. Such actions or inactions of humans are observed just as any other external system.

## Coordination

Coordination, covering `o.` (want) and `a.` (commit), is about a structured conversation among subjects to collaborate and steer reality into a desired direction. Esentially it is about placing signs in the environment so that all actions done by subjects are compatible with each other and won't result in conflict.

The coordination protocol is what we call _AskFi_, or _Ask Finance_. You can read the specification of it [here](https://github.com/BrunoZell/ask.fi).

## Consensus

Consensus, grouping `y.` (know) and `u.` (express), represents a convergent data strucure that esentially creates common knowledge by merging perspective into a deduplicated view onto reality.

We are looking to implement a variant of the [Convex Convergent Proof of Stake consensus (CPoS) algorithm](https://convex.world/technology?section=Convergent+Proof+of+Stake) that can accomodate user defined values, i.e. can address all possible wants across all possible perspectives.

## Human Domain

There is no code form the SDK that corresponds to the _Human Domain_.

It moreso represents a placeholder for all possible domain implementations that aim to improve the global human experience.
