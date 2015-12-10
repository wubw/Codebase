open System

[<EntryPoint>]
let main argv = 
    match argv with
    | [|path|] -> Analyzer.start path
    | _ -> printfn "Please specify valid parameters."
    0
