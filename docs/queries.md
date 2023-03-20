# Queries

Aggregate and/or semantically transform _Observations_ from a `WorldState` into different _Appearances_ (the _Query Result_ type). They are not _Perceptions_ because they don't add information to the _World State_. Instad, queries only interpret available data.

Query high level view: A query is a question. A semantic (strongly typed) question frame. The caller (strategies or analyisis code) expects a query to have a signature of `type Query<'Parameters, 'Result> = 'Parameters -> WorldState -> 'Result`). The function should be logically pure, where the same query on the same world state always yields exactly the same answer. With this guarantee, the runtime can cache query results to optimize repeated evaluations. (logically pure in a sense of still being able to call IPFS nodes or blockchain archive nodes to resolve content-addressed data).

`WorldState` references a version of a _World Event Stream_, which is a set of one or more _Observation Sequences_. It sequences Observations from all available Observer Instances of a single Askbot Session. This introduces session-wide ordering of all Observations available to the Askbot Session.

All queries depend on just the `WorldState` and may inspect all available observations to compute the query result. To see the Query API, take a look [at the code](../source/AskFi.Sdk.fs).
