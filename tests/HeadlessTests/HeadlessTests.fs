module Client.Tests

open Fable.Mocha
open Browser.Dom

[<EntryPoint>]
let main argv =
    "../public"
    |> System.IO.Path.GetFullPath
    |> Puppeteer.runTests
    |> Async.RunSynchronously