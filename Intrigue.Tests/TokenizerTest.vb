
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

        'Console.WriteLine(Tokenizer.TokensToString(ts))

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

    <TestMethod()> Public Sub Tokenizer_Tokenize_and_strings_and_lines()

        Dim ts = Intrigue.Parsing.Tokenizer.Tokenize(
            Intrigue.Util.NewString(
                <string>
                    "nada"
                    "abc
cde" "volley"
                    "beach"
               </string>).Trim)

        'Console.WriteLine(Tokenizer.TokensToString(ts))

        Assert.AreEqual(9, ts.Count)

        Assert.AreEqual(
            "string off 0 lin 1 col 1 >nada<",
            ts(0).ToString)
        Assert.AreEqual(
            "string off 28 lin 2 col 21 >abc" & vbCrLf & "cde<",
            ts(3).ToString)
        Assert.AreEqual(
            "string off 39 lin 3 col 6 >volley<",
            ts(5).ToString)
        Assert.AreEqual(
            "string off 69 lin 4 col 21 >beach<",
            ts(8).ToString)
    End Sub
End Class
