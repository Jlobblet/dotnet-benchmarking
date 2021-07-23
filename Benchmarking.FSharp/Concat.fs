module Benchmarking.FSharp.Concat

let FSharpConcat = Seq.concat

let FSharpAppend = Seq.append

let FSharpSeqExpression l r = seq { yield! l; yield! r }

let FSharpSeqExpressionMany enumerables = Seq.reduce FSharpSeqExpression enumerables
