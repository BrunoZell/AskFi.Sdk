# Queries

Aggregate and/or semantically transform _Observations_ from a `Perspective` into different _Appearances_ (the _Query Result_ type). They are different from _Perceptions_ in that _Query Results_ don't add information to the _Perspective_. Instad, queries only interpret available data. But _Query Results_ are 'there' in the same way as _Perceptions_ are there.

Query high level view: A query is a question. A semantic (strongly typed) question frame. The caller (strategies or analyisis code) expects a query to have a signature of `type Query<'Parameters, 'Result> = 'Parameters -> Perspective -> 'Result`). The function should be logically pure, where the same query on the same Perspective always yields exactly the same answer. With this guarantee, the runtime can cache query results to optimize repeated evaluations. (logically pure in a sense of still being able to call IPFS nodes or blockchain archive nodes to resolve content-addressed data).

`Perspective` references a version of a _Perspective Sequence_, which is a linked list of one or more _Observation Sequences_. It sequences _Observations_ from all available _Observer Instances_ of a single _Askbot Session_. This introduces session-wide ordering of all _Observations_ available to the related _Askbot Session_.

All queries depend on just the `Perspective` and may inspect all available _Observations_ to compute the query result. To see the Query API, take a look [at the code](../source/AskFi.Sdk.fs).
