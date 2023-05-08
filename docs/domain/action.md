# Action Execution via Brokers

_Brokers_ handle all communication with external systems that expectedly yield a relevant effect.

An instance of a _Broker_ takes an instance of its `Action`-type and sends according network IO to the respective external computer networks, essentially executing the requested _Action_.

Brokers are implemented via `AskFi.Sdk.IBroker<'Action>`:

```fsharp
type IBroker<'Action> =
    abstract member Execute : 'Action -> Task
```

The _Askbot Runtime_ calls `Execute` whenever the users live strategy decided to execute an action of the brokers respective `Action`-type.

## Implementations

Some brokers are already implemented, altough they aren't open sourced yet. If you want the code, just ask.

### Distributed Ledger Wallets

Wallet brokers typically have access to a private key and participate in the ledgers p2p network.

An instance of their `Action`-type contains all information requied to build, sign, and send the accoridng transaction to the public network.

- Ethereum Wallet
- Polygon Wallet
- Arbitrum Wallet

### Exchange Trader API

Exchange brokers typically have access to an api secret and communicate with the exchanges API via protocols like http, WebSocket, or FIX.

An instance of their `Action`-type contains all information requied to build an API request to place and cancel orders on the respective exchange.

- Bitfinex Trader
- Binance Trader
- Bybit Trader
- Coinbase Trader
