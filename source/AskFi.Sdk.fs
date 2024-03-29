module AskFi.Sdk
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Threading.Tasks

// ######################
// #### OBSERVATIONS ####
// ######################

/// An atomic appearance of sensory information from within an IObserver<'Percept>.
/// Includes a Percept, which is a typed (via a domain model) representation of the newly observed information.
/// An observation can have multiple Percepts in case they all appeared at the same instant (point in time).
/// An individual observation, by definition, appeared at a singular instant (point in time).
/// Since the 'Percept here is alredy stringly typed, it is data resulted from already interpreting the observed system.
[<IsReadOnly; Struct>]
type Observation<'Percept> = {
    Percepts: 'Percept array
}

/// Implemented by domain modules. An implementation defines imperative networking behavior to interact with
/// an external system with the aim of observing it to later infer what was happening. The observer implementation
/// interprets the networking traffic and emits strongly-typed 'Percepts whenever there are new measurements obtained.
type IObserver<'Percept> =
    abstract member Observations : IAsyncEnumerable<Observation<'Percept>>

// ###############
// #### QUERY ####
// ###############

/// An atomic appearance of sensory information from callers of IContextQueries.
/// Includes a Percept, which is a typed (via a domain model) representation of the newly observed information.
/// An observation can have multiple Percepts in case they all appeared at the same instant (point in time).
/// An individual observation, by definition, appeared at a singular instant (point in time).
/// When this observation was captured, a runtime timestamp was attached to later filter it by query-provided unobserved timestamps.
[<IsReadOnly; Struct>]
type CapturedObservation<'Percept> = {
    /// Sensory information captured by the producing IObserver.
    Percepts: 'Percept array

    /// Runtime timestamp at which this observation was produced by the IObserver.
    At: System.DateTime
}

/// Public query interface into a given Context.
/// Used by strategies, visualizations, and standalone analysis code to retrieve information from a Context.
type IContextQueries = 
    /// Get the latest received perception of the requested type.
    /// Returns `None` if no observation of the requested type has been made yet.
    abstract member latest<'Percept> : unit -> CapturedObservation<'Percept> option
    
    /// Get an iterator the all Observations of type `'Perception` since the passed `from` until `to`
    /// (as determined by the runtime clock used during context sequencing).
    abstract member inTimeRange<'Percept> : from: System.DateTime * ``to``: System.DateTime -> CapturedObservation<'Percept> seq

    /// Get an iterator the all Observations of the two types `'Perception1` and `'Perception2` since the passed `from` until `to``.
    /// (as by the runtime clock used for context sequencing)
    abstract member inTimeRange<'Percept1, 'Percept2> : from: System.DateTime * ``to``: System.DateTime -> System.ValueTuple<CapturedObservation<'Percept1> option, CapturedObservation<'Percept2> option> seq

[<IsReadOnly; Struct>]
type Context = {
    /// Built in default query interface for a given Context
    Query: IContextQueries
}

type Query<'Parameters, 'Result> =
    'Parameters -> Context -> 'Result

// ##################
// #### STRATEGY ####
// ##################

type DecisionReflection<'Action> = {
    /// The action itself
    Action: 'Action

    /// Timestamp of the context on which this actions decisions was made.
    VirtualTimestamp: System.DateTime
    
    /// Runtime timestamp at which this actions decisions was made.
    /// For live execution, the difference between virtual and actual timestamp is the strategy evaluation time.
    /// For backtests, the difference between virtual and actual timestamp is the time duration looking back in time.
    ActualTimestamp: System.DateTime
}

/// Query interface into decisions from a decision sequence "AskFi.Runtime.DataModel.DecisionSequenceHead".
/// Used by strategies to reflect on past actions. This is important so the strategy can remember it just
/// did some 'Action to not do it again even if there is no external sensory-information hinting on it.
type IReflectionQueries = 
    /// Get the latest received perception of the requested type.
    /// Returns `None` if no observation of the requested type has been made yet.
    abstract member latest<'Action> : unit -> DecisionReflection<'Action> option
    
    /// Get an iterator the all Observations of type `'Perception` since the passed `from` until `to`
    /// (as determined by the runtime clock used during context sequencing).
    abstract member inTimeRange<'Action> : from: System.DateTime * ``to``: System.DateTime -> DecisionReflection<'Action> seq

[<IsReadOnly; Struct>]
type Reflection = {
    /// Built in default query interface for the runtimes decision Sequence
    Query: IReflectionQueries
}

type ActionInitiation = {
    Action: obj
    Type: System.Type
}

type Decision =
    | Inaction
    | Initiate of Initiatives:ActionInitiation array

/// Contains the code of a strategy decision, called upon each evolution of the Askbot sessions context (i.e. on every new observation).
type Strategy =
    Reflection -> Context -> Decision

// ###################
// #### EXECUTION ####
// ###################

type IBroker<'Action> =
    abstract member Execute : 'Action -> Task
