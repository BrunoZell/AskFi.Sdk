# Observation

Observers define how new information is sourced from external systems.

Observers are implemented via `AskFi.Sdk.IObserver<Percept>`:

```fsharp
[<IsReadOnly; Struct>]
type Observation<'Percept> = {
    Percepts: 'Percept array
}

type IObserver<'Percept> =
    abstract member Observations : IAsyncEnumerable<Observation<'Percept>>
```

An `Observation<'Percept>` represents an atomic appearance of sensory information. Atomic meaning that the observation appeared at a singular instant, the same point in time.

Includes one or more _Percepts_, which is a typed representation of the newly observed information.

An observation can hold multiple _Percepts_ in case they all appeared at the same instant. If the domain defines a temporal ordering, it is done within that domains `Interpreter`.

An Observer-implementation defines a control loop that maintains required network connections to observe external computer networks.

Observations happen spontaneously. Their creation is determined by how the related observer is implemented. For example, an observer could scrape an API endpoint every 10 seconds by design. As far as the _Askbot Runtime_ is concerned, occurence of new observations are non-predictable.

## Future Work

Extract out communication protocols:

Currently the act of communication with other computers is intertwined with interpreting the received messages. To make the data pipeline more concise and have a higher degree of code reusability, we should abstract out communication protocols, do the recording of information on a protocol level, and have all domain logic in the _Interpreters_.
