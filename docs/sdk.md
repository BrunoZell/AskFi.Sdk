# Ask Finance Strategy Development Kit (SDK)

Consistes of:

- [Observers](./observations.md) that scrape the external world and produce strongly-typed _Perceptions_,
- [Queries](./queries.md) that aggregate and/or semantically transform those _Perceptions_ into other types (_Query Results_, or: _Appearances_),
- [Strategies](./strategies.md) that compose decision-trees out of _Query Results_ (reactive conditions), mapping each case to a _Decision_, which is a (possibly empty) set of _Actions_ to initiate.
- [Brokers](./brokers.md) that take an _Action_ initiation and send according network IO to external computer networks, essentially executing the requested _Action_.

A [Runtime](https://github.com/BrunoZell/AskFi.Runtime) is used to compose the execution of _Observers_, _Queries_, _Strategies_, and _Brokers_.
