# Runtime Internals

[Source Code](https://github.com/BrunoZell/AskFi.Runtime)

The _Runtime_ is modularized. Each modules implements a sub-problem while sharing the same data strutures to pass data around.

## Modules

- Observer Group: running one or more `IObserver<'Percept>`
- Perspective Merge: merging observations from multiples observation groups distributed thrughout space into a temporally ordered sequence
- Interpretation: applying custom `Interpreter`-implementations on `CapturedObservation`s.
- Evaluation: expressing preference between `Scene`s.
- Strategy: executes custom `Strategy`-implementations on a `Scene` and the strategies session `Reflection`.
- Analysis: executes custom code that can read information from a `Scene` through the _Scene Query Interface_.

## Data Model

Defines data structures to store:

- sensory-motor information as produced by observers and brokers
  - observation sequence
  - execution trace
- indexing information:
  - interpretations `captured-observation -> code<interpreter> : moment` with `scene = moment list`
    where `moment['ObjectIdentity : equatable, 'ReferencePhrase] = map[typeid<'ObjectIdentity>, map['ObjectIdentity, 'ReferencePhrase list]]`
  - visualizations `scene -> code<interpreter> : canvas`
  - strategy results `scene -> code<strategy> : decision-sequence-head`

[Source Code](https://github.com/BrunoZell/AskFi.Runtime/blob/main/source/AskFi.Runtime.DataModel/Runtime.DataModel.fs)
