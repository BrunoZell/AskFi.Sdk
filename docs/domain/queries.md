# Queries

Queries aggregate and/or semantically transform _References_ from a `Scene` into different _Query Results_. They don't add any information to the _Scene_. Instad, queries only aggregate and transform available information. This can be compared to the act of reasoning.

```fsharp
type Query<'Parameters, 'Result> =
    'Parameters -> Scene -> 'Result
```

Query high level view: A query is a question. A semantic (strongly typed) question frame. The caller (strategies or analyisis code) expects a query to have a signature of `type Query<'Parameters, 'Result> = 'Parameters -> Scene -> 'Result`. The function should be logically pure, where the same query on the same Perspective always yields exactly the same answer. With this guarantee, the runtime can cache query results to optimize repeated evaluations. (logically pure in a sense of still being able to call IPFS nodes or blockchain archive nodes to resolve content-addressed data).

`Scene` containes interpreted _References_ in the temporal order of the underlying _Perspective_.

All queries depend on just a `Scene` and the _Interpreter_ that are referenced in the implementation. It may inspect all available historic _References_ to compute the query result.

## Implementation Requirements

- Code must only reference data from passed `'Parameters` and `Scene`.
- Object instance referenes from the `Scene` are read via the query interface defined as `AskFi.Sdk.ISceneQueries`.
- Function must be logically pure. Resolving content-addressed data is fine.
