# Domain Modelling

The SDK defines the semantic and technical entry points for users to define their own imagination of reality.

It is a type library written in F#, and it can be implemented by any language that compiles into .NET Intermediate Language.

There are two communication adapters to be implemented:

- [Observers and Percepts](observation.md) to capture percepts by maintaining according network connections to external computer networks.
- [Brokers and Actions](action.md) to execute actions by sending the according network traffic.

And three functions:

- [Interpreters](interpretation.md) to extract semantically computable reference phrases from captured observations.
- [Queries](queries.md) which are reusable implementations of semantic aggregations or transformations of object instance references within a scene.
- [Strategies](strategy.md) to map the interpreted representation of reality to decisions about what to do.
