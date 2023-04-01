namespace AskFi.Persistence
open System.Runtime.CompilerServices
open System

// #####################
// #### PERSISTENCE ####
// #####################

[<IsReadOnly; Struct>]
type ContentId = {
    // Todo: Make this an IPFS CID respecting multihash and multicodec
    Raw: byte array
}

type EncodedIdea = {
    /// Content-id of the idea. This is a hash of this ideas 'Content' and is used throughout
    /// the system to uniquely identify this idea in a content-addressed manner.
    Cid: ContentId

    /// Serialized data of the idea. Can be deserialized into an instance of the original 'Idea-type.
    Content: byte array
}

type Serializer =
    abstract member serialize<'Idea> : 'Idea -> EncodedIdea
    abstract member deserialize<'Idea> : EncodedIdea -> 'Idea
