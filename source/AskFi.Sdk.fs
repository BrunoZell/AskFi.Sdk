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
/// An individual observation, by definition, appeared at a singular instant (point in time)
[<IsReadOnly; Struct>]
type Observation<'Percept> = {
    Percepts: 'Percept array
}

type IObserver<'Percept> =
    abstract member Observations : IAsyncEnumerable<Observation<'Percept>>

// ########################
// #### INTERPRETATION ####
// ########################

type Interpreter<'Percept, 'Identity, 'Reference when 'Identity: comparison> =
    Observation<'Percept> -> Map<'Identity, 'Reference list>

// ###############
// #### QUERY ####
// ###############

/// An atomic appearance of sensory information from outside of IContextQueries.
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
    abstract member latest<'Perception> : unit -> CapturedObservation<'Perception> option
    
    /// Get an iterator the all Observations of type `'Perception` since the passed `timestamp`
    /// (as determined by the runtime clock used during context sequencing).
    abstract member since<'Perception> : timestamp: System.DateTime -> CapturedObservation<'Perception> seq

    /// Todo: get an ordered sequenced of multiple Perception-types
    /// Get an iterator the all Observations of the two types `'Perception1` and `'Perception2` since (as by the runtime clock used for context sequencing) the passed `timestamp`.
    abstract member since<'Perception1, 'Perception2> : timestamp: System.DateTime -> System.ValueTuple<CapturedObservation<'Perception1> option, CapturedObservation<'Perception2> option> seq

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

type IReflectionQueries = interface end

[<IsReadOnly; Struct>]
type Reflection = {
    /// Built in default query interface for the runtimes Decision Sequence
    /// "AskFi.Runtime.DataModel.DecisionSequenceHead"
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
