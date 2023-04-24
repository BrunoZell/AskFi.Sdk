module AskFi.Sdk
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Threading.Tasks
open System

// #########################
// #### OBSERVER MODULE ####
// #########################

/// An atomic appearance of sensory information.
/// Includes a Perception, which is a typed (via a domain model) representation of the newly observed information.
/// An observation can have multiple Perceptions in case they all appeared at the same instant (point in time).
/// An individual observation, by definition, appeared at a singular instant (point in time)
[<IsReadOnly; Struct>]
type Observation<'Perception> = {
    Perceptions: 'Perception array
}

type IObserver<'Perception> =
    abstract member Observations : IAsyncEnumerable<Observation<'Perception>>

// ################################
// ####   PERSPECTIVE MODULE   ####
// ################################

/// Public query interface into a given Perspective.
/// Used by queries, strategies and standalone analysis code to retrieve observations from a Perspective.
type IPerspectiveQueries =
    /// Get the latest received perception of the requested type.
    /// Returns `None` if no observation of the requested type has been made yet.
    abstract member latest<'Perception> : unit -> Observation<'Perception> option

    /// Get an iterator the all Observations of type `'Perception` since the passed `timestamp`
    /// (as determined by the runtime clock used during WorldEventStream sequencing).
    abstract member since<'Perception> : timestamp: DateTime -> Observation<'Perception> seq

    /// Todo: get an ordered sequenced of multiple Perception-types
    /// Get an iterator the all Observations of the two types `'Perception1` and `'Perception2` since (as by the runtime clock used for WorldEventStream sequencing) the passed `timestamp`.
    abstract member since<'Perception1, 'Perception2> : timestamp: DateTime -> System.ValueTuple<Observation<'Perception1> option, Observation<'Perception2> option> seq

[<IsReadOnly; Struct>]
type Perspective = {
    /// Built in default query interface for the runtimes Perspective Sequence
    /// "AskFi.Runtime.DataModel.PerspectiveSequenceHead"
    Query: IPerspectiveQueries
}

type Query<'Parameters, 'Result> = 'Parameters -> Perspective -> 'Result

// #############################
// ####   STRATEGY MODULE   ####
// #############################

type ActionInitiation = {
    Action: obj
    Type: System.Type
}

type Decision =
    | Inaction
    | Initiate of Initiatives: ActionInitiation array

type IReflectionQueries = interface end

[<IsReadOnly; Struct>]
type Reflection = {
    /// Built in default query interface for the runtimes Decision Sequence
    /// "AskFi.Runtime.DataModel.DecisionSequenceHead"
    Query: IReflectionQueries
}

/// Contains the code of a strategy decision, called upon each evolution of the Askbot Sessions Perspective (i.e. on every new observation).
type Decide = Reflection -> Perspective -> Decision

// ##############################
// ####   EXECUTION MODULE   ####
// ##############################

type IBroker<'Action> =
    abstract member Execute : 'Action -> Task
