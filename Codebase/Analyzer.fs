module Analyzer

open System
open System.IO

let filter file =
    let allowedExt = [|".fs"; ".cs"|]
    let ext = Path.GetExtension file
    Seq.contains ext allowedExt

let rec search path = 
    let ret = path |> Directory.EnumerateFiles
    let ret2 = path |> Directory.EnumerateDirectories |> Seq.collect (fun x -> search(x)) 
    Seq.append ret ret2 |> Seq.filter (fun x -> x |> filter)

let processFile file = 
    try
        let allLines = file |> File.ReadAllLines
        let notEmptyLines = Seq.filter (fun x -> x |> String.IsNullOrWhiteSpace |> not) allLines
        (file, allLines.Length, Seq.length notEmptyLines)
    with 
        |_ -> (String.Empty, 0, 0)

let start path = 
    if Directory.Exists(path) then
        let results = path |> search |> Seq.map (fun x -> x |> processFile)
        printfn "Codebase: %A, %A" path results
        let aggregateResult = results |> Seq.reduce (fun x y -> 
            let (_, x2, x3), (_, y2, y3) = x, y
            (String.Empty, x2+y2, x3+y3))
        printfn "All lines and not empty lines: %A" aggregateResult 
    else
        printfn "Directory %A does not exist." path
