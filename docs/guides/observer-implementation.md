# Observer Implementation Guidelines

## Perception Types

Guidelines on how to model _Perceptions_.

### Embrace Abstraction

An observation-type is never really the _exact_ observation, so there for sure must be some abstraction allowed in it's definition.

Complexities like:

- network (dis)connections
- pub/sub session handling
- message (de)serialization
- message deduplication
- message reordering/sequencing

are typically abstracted out for practicality in the domain model. Such details, though very real in the observers implementation, have a negible effect on the relevant view of the observed external system. It keeps the domain model pragmatic, closer to the concepts that matter in strategy development.

Furthermore, all concepts exposed (or better: ingested, referenced, introduced) by perception-types are required to be accurately reverse-engineerd from system simulations. By including complexities listed above in the Perception-Type, all simulations must be capable of simulating such complexities. Given that they generally are not relevant to the domain, it adds unnecessary development overhead for no gain.

Thus, principle: Always go with the most abstract perception-type that exposes enough information first, only adding more details when it's actually referenced in any queries.

### Only use observation sessions with multiple observations when continuity is required by the domain

The only reason observation sessions are a relevant concept is that it provides continuity. That is, if event A and B happen within the same observation session immediately after another (that is, B.previous = A), there is are guaranteed (as a requirement) no sensory events in-between. For example, if the sensory-type is an order book trade, there where no trades  in between them.

It is NOT required to sequence different sensory events. Every single sensory event across all observation sessions is sequenced at the time it's consumed by a rabot session (immediately before it is published as an observation within the rabot session). Before that, no ordering between sensory events is necessary because they could arrive out of order anyways. And for the runtime to execute as quickly as possible, by design it does not wait for any external data producer. Therefore, late-arriving data is not a concern of the runtime.
