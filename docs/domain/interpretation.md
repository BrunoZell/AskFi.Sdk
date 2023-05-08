# Interpretation

How the external world is represented is user defined. It is defined by implementing an `Interpreter` of the form:

```fsharp
type Interpreter<'Percept, 'Identity, 'Reference when 'Identity: comparison> =
    Observation<'Percept> -> Map<'Identity, 'Reference list>
```

Let's unpack that. First, there are three type parameters:

- `'Percept`: Defines what _Percepts_ this interpreter can understand, as emitted by an `IObserver<'Percept>`.
- `'Identity : comparison`: Defines how to disambiguate multiple object instances of the same object type that are refered to in the same _Scene_.
- `'Reference`: Typically a union-type that specifies all different types of phrases that refer to the object this interpreter identifies.

As an input, it accepts an `Observation<'Percept>`, so the interpreter is analyzing a single observation (with possible multiple _Percepts_) at a time. No context is shared between observations as this code only does the mapping from raw communication bytes into what domain-specific message was communicated. All further analysis is done as a _Query_ on the combined _Scene_.

The output is typed as `Map<'Identity, 'Reference list>`, which basicaly translates into a list of object instances where each instance carries one or more reference phrases.

Each interpreter can only identify a single object class. To identify instances of distinct object classes, implement an _Interpreter_ for each of them.
