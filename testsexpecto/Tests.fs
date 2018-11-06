module TestsExpecto

open Reaction
open Reaction.AsyncObservable

open Expecto
open Tests.Utils

[<Tests>]
let tests =
  testList "samples" [
    testAsync "Test ofSeq empty" {
      // Arrange
      let xs = ofSeq Seq.empty
      let obv = TestObserver<int>()

      // Act
      let! dispose = xs.SubscribeAsync obv

      do! obv.AwaitIgnore ()

      // Assert
      let actual = obv.Notifications |> Seq.toList
      let expected : Notification<int> list = [ OnCompleted ]

      Expect.equal actual expected "not equal"
    }

    testAsync "Test ofSeq non empty" {
      let xs = ofSeq <| seq { 1 .. 5 }
      let obv = TestObserver<int>()

      // Act
      let! dispose = xs.SubscribeAsync obv
      do! obv.AwaitIgnore ()

      // Assert
      let actual = obv.Notifications |> Seq.toList
      let expected = [ OnNext 1; OnNext 2; OnNext 3; OnNext 4; OnNext 5; OnCompleted ]

      Expect.equal actual expected "not equal"
    }
  ]