# Brokers

_Brokers_ handle all communication with external systems that expectedly yield a relevant effect.

An instance of a _Broker_ takes an istance of its `Action`-type and sends according network IO to external computer networks, essentially executing the requested _Action_.

Brokers are implemented via `AskFi.Sdk.IBroker<Action>`.

The _Askbot Runtime_ requires that there only ever is a single _Broker_-instance mapped to each `Action`-type in any given _Askbot Session_.

## Implementations

- Ethereum Wallet
- Polygon Wallet
- Arbitrum Wallet
- Bitfinex Trader
- Binance Trader
- Bybit Trader
- Coinbase Trader
