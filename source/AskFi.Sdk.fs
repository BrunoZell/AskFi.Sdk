module AskFi.Sdk
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Threading.Tasks

// ######################
// #### OBSERVATIONS ####
// ######################

/// An atomic appearance of sensory information.
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

/// Public query interface into a given Scene.
/// Used by strategies, visualizations, and standalone analysis code to retrieve information from a Scene.
type ISceneQueries = interface end

[<IsReadOnly; Struct>]
type Scene = {
    /// Built in default query interface for a given Scene
    Query: ISceneQueries
}

type Query<'Parameters, 'Result> =
    'Parameters -> Scene -> 'Result

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

/// Contains the code of a strategy decision, called upon each evolution of the Askbot Sessions Perspective (i.e. on every new observation).
type Strategy =
    Reflection -> Scene -> Decision

// ###################
// #### EXECUTION ####
// ###################

type IBroker<'Action> =
    abstract member Execute : 'Action -> Task
