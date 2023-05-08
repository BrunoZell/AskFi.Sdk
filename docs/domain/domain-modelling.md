# Domain Modelling

The SDK defines the semantic and technical entry points for users to define their own imagination of reality.

There are two communication adapters to be implemented:

- [Observers and Percepts](observation.md) to capture percepts by maintaining according network connections
- [Brokers and Actions](action.md) to handle execute actions by sending according network traffic.

And two functions:

- [Interpreters](interpretation.md) to extract semantically computable reference phrases from captured observations.
- [Strategies](strategy.md) to map the interpreted representation of reality to decisions about what to do.
