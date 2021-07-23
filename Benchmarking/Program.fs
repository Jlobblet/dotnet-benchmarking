// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Collections.Generic
open System.Diagnostics
open System.Linq
open Benchmarking.CSharp
open Benchmarking.FSharp

[<Literal>]
let SomeBigNumber = 10_000

[<Literal>]
let TestNumber = 100_000

[<Struct>]
type Row = { Label: string; Time: int64 }

let Tabulate (rows: Row list) =
    let labelHeader = "Label"
    let timeHeader = "Elapsed milliseconds / ms"

    // Calculate the widths of the columns
    let labelWidth =
        labelHeader
        :: (rows |> List.map (fun r -> r.Label))
        |> List.map (fun l -> l.Length + 1)
        |> List.max

    let timeWidth =
        timeHeader
        :: (rows |> List.map (fun r -> r.Time.ToString()))
        |> List.map (fun l -> l.Length + 1)
        |> List.max

    // Label   Elapsed milliseconds / ms
    // Foobar                        621
    let rowString = $"{{0,{-labelWidth}}}{{1,{timeWidth}}}"

    String.Format(rowString, labelHeader, timeHeader)
    :: (rows
        |> List.map (fun r -> String.Format(rowString, r.Label, r.Time)))

[<EntryPoint>]
let main _ =
    let positives = HashSet<int> [ 1 .. SomeBigNumber ]
    let negatives = HashSet<int> [ -SomeBigNumber .. -1 ]

    let sw = Stopwatch()

    let record label func =
        printfn $"Starting %s{label}"
        sw.Restart()
        func ()
        sw.Stop()
        printfn $"Finished %s{label}"

        { Label = label
          Time = sw.ElapsedMilliseconds }

    let cases =
        let random = Random()
        // Optimised to be as fast as possible, since it's being timed
        let contains (s: seq<int>) =
            let mutable i = 0

            while i < TestNumber do
                // We don't actually care about the result
                s.Contains 0 |> ignore
                i <- i + 1

        [| "F# Concat",
           Concat.FSharpConcat [| negatives
                                  positives |]

           "F# Append", Concat.FSharpAppend negatives positives

           "F# Seq Expression", Concat.FSharpSeqExpression negatives positives

           "F# Seq Expression with Reduce",
           Concat.FSharpSeqExpressionMany [| negatives
                                             positives |]

           "F# Collect",
           Concat.FSharpCollect [| negatives
                                   positives |]

           "C# Linq Concat", negatives.LinqConcat positives
           
           "C# foreach loops", negatives.ForeachConcat positives

           "SelectMany",
           Concat.SelectManyConcat [| negatives
                                      positives |]

           "CollectionConcat", negatives.CollectionConcat positives |]
        // Shuffle array to try and reduce potential linkage
        |> Array.sortBy (fun _ -> random.Next())
        // Record the results of each test
        |> Array.map (fun (l, c) -> record l (fun () -> c |> contains))

    cases
    |> List.ofArray
    |> Tabulate
    |> String.concat "\n"
    |> printfn "%s"

    0 // return an integer exit code
