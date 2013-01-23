
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue
Imports Intrigue.Parsing


<TestClass()> Public Class TokenizerTest

    <TestMethod()> Public Sub Tokenizer_Tokenize()

        Dim ts = Intrigue.Parsing.Tokenizer.Tokenize(
            Intrigue.Util.NewString(
                <string>
                   (alpha 1 2 3) # nada
                   # nada
                   bravo 4 5 6 # nada
                   "hello"
               </string>))

        Console.WriteLine(Tokenizer.TokensToString(ts))

        Assert.AreEqual(26, ts.Count)

        Assert.AreEqual(
            "newline off 0 lin 1 col 1 ><",
            ts(0).ToString)
        Assert.AreEqual(
            "number off 99 lin 4 col 30 >6<",
            ts(20).ToString)
        Assert.AreEqual(
            "string off 128 lin 5 col 20 >hello<",
            ts(23).ToString)
    End Sub
End Class
