module AskFi.Sdk
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Threading.Tasks
open System
open AskFi.Persistence

// ######################
// #### OBSERVATIONS ####
// ######################

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

// #####################
// ####   QUERIES   ####
// #####################

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
    /// This references a Sdk.Runtime.DataModel.PerspectiveSequenceHead, which in turn references
    /// all (in this perspective) available observations across all Perception-types.
    LatestPerspectiveSequenceHead: ContentId
    Query: IPerspectiveQueries
}

type Query<'Parameters, 'Result> = 'Parameters -> Perspective -> 'Result

// ######################
// ####   STRATEGY   ####
// ######################

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
    /// This references a Sdk.Runtime.DataModel.DecisionSequenceHead, which in turn references
    /// all (in this session) actions that have been decided in across all Action-types.
    LatestDecisionSequenceHead: ContentId
    Query: IReflectionQueries
}

/// Contains the code of a strategy decision, called upon each evolution of the Askbot Sessions Perspective (i.e. on every new observation).
type Decide = Reflection -> Perspective -> Decision

// ######################
// ####    ACTION    ####
// ######################

type IBroker<'Action> =
    abstract member Execute : 'Action -> Task
