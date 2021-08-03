module Client.Tests


open Fable.Mocha

let arithmeticTests =
  testList "Arithmetic tests" [

    testCase "plus works" <| fun () ->
      Expect.equal (1 + 1) 2 "plus"

    testCase "Test for falsehood" <| fun () ->
      Expect.isFalse (1 = 2) "false"

    testCaseAsync "Test async code" <|
      async {
        let! x = async { return 21 }
        let answer = x * 2
        Expect.equal 42 answer "async"
      }
  ]

let client =
  testList "Client tests" [
    testCase "Mocha works" <| fun () ->
      Expect.equal 1 1 "Mocha works"
  ]

let all = 
  testList "All" [
    arithmeticTests
    client
  ]


Mocha.runTests all
|> ignore