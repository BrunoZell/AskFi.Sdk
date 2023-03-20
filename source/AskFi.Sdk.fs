module AskFi.Sdk
open System.Collections.Generic
open System.Runtime.CompilerServices
open System

// ######################
// #### OBSERVATIONS ####
// ######################

// This is a non-deterministic id generated by the IObserver implementation to correlate multiple observations into a continuum.
// Todo: implement equitable. And make it collision-resistant between Observer instances that use the same underlying bynary repsesentation but refer to different (i.e. their own) ObservationSessions.
// In the runtime, it's represented as untyped bytes. Three identities are namespaced within an IObserver-instance.
[<IsReadOnly; Struct>]
type ObservationSession =
    | ObservationSession of uint64

/// An atomic appearance of sensory information.
/// Includes a Perception, which is a typed (via a domain model) representaton of the newly observed information.
/// An observation can have multile Perceptions in case they all apperaed at the same instant (point in time).
/// An individual observation, by definition, appeared at a singular instant (point in time)
/// And references an ObservationSession, which can be used by the IObserver implementation to correlate multiple Observations into a continuity
/// (useful for expressing that the observation was exaustive, e.g. there were no trades in between these two observed trades because they are immediat "neighbors" within the same observation session)
[<IsReadOnly; Struct>]
type Observation<'Perception> = {
    Session: ObservationSession
    Perceptions: 'Perception ReadOnlyMemory
}

type IObserver<'Perception> =
    abstract member Observations : IAsyncEnumerable<Observation<'Perception>>

// #####################
// ####   QUERIES   ####
// #####################

[<IsReadOnly; Struct>]
type WorldState = {
    /// This references a Sdk.Runtime.WorldEventSequence, which in turn references
    /// all (in this world) available observations accross all Perception-types.
    HashOfLatestWorldEventSequence: int32
}

type Query<'Parameters, 'Result> = 'Parameters -> WorldState -> 'Result

/// Useful, reusable extensions to retrieve observations from a WorldState.
type WorldState with
    /// Get the latest received perception of the requested type.
    /// Returns `None` if no observation of the requested type has been made yet.
    member world.latest<'Perception> () : 'Perception option =
        None

    /// Get an iterator the all Observations of type `'Perception` since the passed `timestamp`
    /// (as determined by the runtime clock used during WorldEventStream sequencing).
    member world.since<'Perception> (timestamp: DateTime) : 'Perception seq =
        Seq.empty

    /// Todo: get an ordered sequenced of multiple Perception-types
    /// Get an iterator the all Observations of the two types `'Perception1` and `'Perception2` since (as by the runtime clock used for WorldEventStream sequencing) the passed `timestamp`.
    member world.since<'Perception1, 'Perception2> (timestamp: DateTime) : System.ValueTuple<'Perception1, 'Perception1> seq =
        Seq.empty

// ######################
// ####   STRATEGY   ####
// ######################

type ActionInitiation = {
    Action: obj
    Type: System.Type
}

type ActionSet = ActionSet of Initiatives:ActionInitiation ReadOnlyMemory

type Decision =
    | Inaction
    | Initiate of ActionSet:ActionSet

/// Describes the random part of an ActionId to disambiguate between actions that got initiated
/// at exactly the same timestamp.
type ActionIdNonce = uint64

/// A unique id created by the RABOT Runtime to uniquely identify an action initiation.
/// This helps to analyze logs and disambiguate actions that are otherwise exactly equal.
[<Struct>]
type ActionId =
    ActionId of timestamp:DateTime * nonce:ActionIdNonce

type StrategyReflection = {
    InitiatedActions: ActionId ReadOnlyMemory
}

/// Contains the code of a strategy decision, called upon each evolution of the RABOT Sessions WorldState (i.e. on every new observation).
type Decide = StrategyReflection -> WorldState -> Decision

// ######################
// ####    BROKER    ####
// ######################

type IBroker<'Action> =
    abstract member Execute : ActionId * 'Action -> unit
