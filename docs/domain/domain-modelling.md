# Domain Modelling

The SDK defines the semantic and technical entry points for users to define their own imagination of reality.

It is a type library written in F#, and it can be implemented by any language that compiles into .NET Intermediate Language.

There are two communication adapters to be implemented:

- [Observers and Percepts](observation.md) to capture percepts by maintaining according network connections to external computer networks.
- [Brokers and Actions](action.md) to execute actions by sending the according network traffic.

And three functions:

- [Interpreters](interpretation.md) to extract semantically computable reference phrases from captured observations.
- [Queries](queries.md) which are reusable implementations of semantic aggregations or transformations of object instance references within a context.
- [Strategies](strategy.md) to map the interpreted representation of reality to decisions about what to do.

[what follows is vNext with SCMs]

## SCM virtual/actual intersection types

(capturable actual message exchange; describes what happened):

These types of actual messages are exchanged at the intersection of the inacessible external system and the outermost layer of the computable sensory-motor cycle:

- `'Percept` (only really exist at the single moment of observation)
  -> Each domain model creates as many structured types of perceptions as needed, and the according Observer implementations that can capture them.
- `'Action` (either actually executed; or hypothetically executed; only really exists at the single moment of execution)
  -> Each domain model creates as many structured types of actions as needed, and the according Broker implementations that can execute them.

Actions are the causal entry points into the SCM. They pass an effect (virtual message) to all systems that listen to it. This starts a chain reaction of virtual message exchange between systems according to the causal dependencies specified by the assumed true edges of the SMC graph.

Percepts are the causal exit points out of the SCM. They are the result of a system expectedly sending an actual message to a listening actual sensory-motor system. An SCM intervention simulation may produce virtual Percepts, which basically is the future observation we expect to see if we execute that action (by sending an actual message to the related external system).

## Sensori-motor cycle intersection behavior

**Observers**: Imperative behavior as .NET function with network access `Behavior -> Network IO`
Imperative behavior to communicate with external systems via computer networks and communication protocols. The goal is to gather information about the external system while practically not influencing the external system.
Code: implementatio of .NET `IObserver<'Message>` interface. With `'Message` being the virtual representation of the actual message sent or received from the external system, as low-level as possible (usually the type borrowed from a communication protocol). Interpretation of the virtual representation of the actual message is done by Interpreter implementations and out of scope for Observer.

**Brokers**: Imperative behavior as .NET function with network access `Behavior -> Network IO`
Imperative behavior to communicate with external systems via computer networks and communication protocols. The goal is to influence the external system so that future observations indicate a desired state transition of the external system.
Code: implementation of .NET `IBroker<'Action>` interface, with `'Action` being the virtual representation of the actual message to be sent to the external system, serialized into the actual message within the Broker-implementation (`'Action -> Message`).

## Virtual/Actual translation

Interpreters: Imperative behavior as pure .NET function `Message -> Percept list` where `Percept = (IEML Node, IEML Node Referents)`
Maps a communication protocol message to one or more IEML phrases (metadata) + referents (raw data). The goal is to extract the meaning of the message; what has been said in the conversation.

Analyzers: Imperative behavior as pure .NET function `'Percept -> SCM Observation Node` where `SCM Observation Node = SMC Effect Target Address (for causes to specify they emit a public actual message)`.
Analyzers: Imperative behavior as pure .NET function : `'Action -> SCM Intervention Node` where `SCM Intervention Node = SMC Effect Sender Address (for listeners to target) + 'Effect (what is sent to the listeners) [as SMC object in Context with state as IPLD data structure]`.

## Virtual representation of causal message exchange between external systems

SCM Effect Types (virtual message exchange between system representations):
Each domain describes what system types can emit what `'Effect` (via the `'Behavior` function). The domain can define as many system types (behavior) and effects (state transfer events) as needed.
The system types are the nodes of the SCM graph. Effect exchange allowed by a given system behavior is the *possible* edges of the SCM graph. Causal listening declarations are the *assumed* true edges of the SCM graph.

## Virtual representation of external system state

SCM Node Types (latent state persisted by the system consuming energy, influencing the systems behavior):

Those are the represenative latent states of the external system of interest.

- System (composed behavior; orchestration of object instances):
  - state: collection of particles
  - behavior: function `'State -> 'IncomingEffect -> ProducedEffect<'OutgoingEffect, 'State>`
    where this function is a deterministic sampling of the assumed probability distribution of the system's behavior.
- Object (state information; instances):
  - state: raw IEML/IPLD information
  - no behavior; read-only; can only be deconstructed or reconstructed.
