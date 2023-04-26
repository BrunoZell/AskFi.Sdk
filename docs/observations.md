# Observations

Observations happen spontaneously. Their creation is determined by how the related observer is implemented. For example, an observer could scrape an API endpoint every 10 seconds by design. As far as the _Askbot Runtime_ is concerned, occurence of new observations are non-predictable.

Observers are configured once at _Askbot Instance Build Time_. These static configurations must reference all data sources the bot would ever listen to. No new observers can be added later on the fly. Reason: This better decouples the independent spontaneous production of observations from state management within the _Runtime_.

Implemented via `AskFi.Sdk.IObserver<Percept>`.
